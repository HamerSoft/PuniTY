using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILogger = HamerSoft.PuniTY.Logging.ILogger;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    /// <summary>
    /// VT100 screen
    /// <remarks>Origin is top-left with value (column) 1, (row) 1</remarks>
    /// </summary>
    public class Screen : IScreen
    {
        /// <summary>
        /// Default configuration for the screen
        /// </summary>
        public readonly struct DefaultScreenConfiguration : IScreenConfiguration
        {
            private const int DefaultTabStopSize = 8;
            private readonly int _customTabStopSize;

            public int TabStopSize => _customTabStopSize > 0 ? _customTabStopSize : DefaultTabStopSize;
            public ScreenDimensions ScreenDimensions { get; }
            public FontDimensions FontDimensions { get; }

            public DefaultScreenConfiguration(int rows, int columns, int tabStopSize, FontDimensions fontDimensions)
            {
                FontDimensions = fontDimensions;
                ScreenDimensions = new ScreenDimensions(rows, columns);
                _customTabStopSize = tabStopSize;
            }

            internal DefaultScreenConfiguration(ScreenDimensions screenDimensions, FontDimensions fontDimensions)
            {
                ScreenDimensions = screenDimensions;
                _customTabStopSize = 0;
                FontDimensions = fontDimensions;
            }

            internal DefaultScreenConfiguration(DefaultScreenConfiguration other)
            {
                _customTabStopSize = other.TabStopSize;
                ScreenDimensions = other.ScreenDimensions;
                FontDimensions = other.FontDimensions;
            }
        }

        private readonly ILogger _logger;
        private IScreenConfiguration _screenConfiguration;
        public int Rows { get; }
        public int Columns { get; }
        public ICursor Cursor { get; }
        public event Action<ICharacter> CharacterReceived;
        IScreenConfiguration IScreen.ScreenConfiguration => _screenConfiguration;

        private List<List<ICharacter>> _characters;
        private int _rowOffset;
        private Position? _savedCursorPosition;
        private GraphicAttributes _currentGraphicAttributes;
        private readonly HashSet<int> _clearedTabStops;
        private PointerMode _pointerMode;
        private readonly IPointerModeFactory _pointerModeFactory;
        private readonly ScreenIterator _screenIterator;
        private bool _areCharactersProtected;

        public Screen(ICursor cursor,
            ILogger logger,
            IScreenConfiguration screenConfiguration)
        {
            _screenConfiguration = screenConfiguration;
            _logger = logger;
            Rows = _screenConfiguration.ScreenDimensions.Rows;
            Columns = _screenConfiguration.ScreenDimensions.Columns;
            Cursor = cursor;
            Cursor.SetPosition(new Position(1, 1));
            _rowOffset = 0;
            _currentGraphicAttributes = new GraphicAttributes(AnsiColor.White, AnsiColor.Black);
            PopulateScreen(Rows, Columns);
            _clearedTabStops = new HashSet<int>();
            _screenIterator = new ScreenIterator(this);

            void PopulateScreen(int rows, int columns)
            {
                _characters = new List<List<ICharacter>>(rows);
                for (int i = 0; i < rows; i++)
                {
                    _characters.Add(new List<ICharacter>(columns));
                    for (int j = 0; j < Columns; j++)
                        _characters[i].Add(new Character(_currentGraphicAttributes));
                }
            }
        }

        public void SetCursorPosition(Position position)
        {
            if (position.IsValid(this))
                Cursor.SetPosition(position);
        }

        public ICharacter GetCharacter(Position position)
        {
            return position.IsValid(this)
                ? _characters[position.Row + _rowOffset - 1][position.Column - 1]
                : Character.Invalid();
        }

        void IScreen.SetCharacterProtection(bool isProtected)
        {
            _areCharactersProtected = isProtected;
        }

        public void AddCharacter(char character)
        {
            switch (character)
            {
                case '\n':
                case '\r':
                    SetCursorPosition(new Position(Cursor.Position.Row + _rowOffset + 1, 0));
                    break;
                default:
                    var position = new Position(Cursor.Position.Row + _rowOffset - 1, Cursor.Position.Column - 1);
                    _characters[position.Row][position.Column] =
                        new Character(_currentGraphicAttributes, _areCharactersProtected, character);
                    MoveCursor(1, Direction.Forward);
                    break;
            }
        }

        public void InsertCharacters(int charactersToInsert)
        {
            var inheritChar = _characters[Cursor.Position.Row + _rowOffset - 1][Cursor.Position.Column - 1];

            for (var i = 0; i < charactersToInsert; i++)
                _characters[Cursor.Position.Row + _rowOffset - 1].Insert(Cursor.Position.Column - 1,
                    new Character(inheritChar.GraphicAttributes));

            for (int i = Cursor.Position.Row + _rowOffset - 1; i < _characters.Count; i++)
            {
                var targetRow = _characters[i];
                var overflow = targetRow.Skip(Columns).ToList();
                if (overflow.Count == 0)
                    continue;
                if (targetRow.Count > Columns)
                {
                    targetRow.RemoveRange(Columns, targetRow.Count - Columns); // Remove excess characters

                    if (i == _characters.Count - 1)
                        _characters.Add(new List<ICharacter>());
                }

                _characters[i + 1].InsertRange(0, overflow);
            }

            for (int i = _characters[^1].Count; i < Columns; i++)
            {
                _characters[^1].Add(new Character(_currentGraphicAttributes));
            }
        }

        public void MoveCursor(int cells, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    SetCursorPosition(new Position(Cursor.Position.Row - cells, Cursor.Position.Column));
                    break;
                case Direction.Down:
                    SetCursorPosition(new Position(Cursor.Position.Row + cells, Cursor.Position.Column));
                    break;
                case Direction.Forward:
                    if (Cursor.Position.Column + cells <= Columns)
                        SetCursorPosition(new Position(Cursor.Position.Row, Cursor.Position.Column + cells));
                    else if (Cursor.Position.Row + 1 <= Rows)
                        SetCursorPosition(new Position(Cursor.Position.Row + 1,
                            cells));
                    else
                        _logger.LogWarning("Cannot move cursor forward.");
                    break;
                case Direction.Back:
                    if (Cursor.Position.Column - cells >= 1)
                        SetCursorPosition(new Position(Cursor.Position.Row, Cursor.Position.Column - cells));
                    else if (Cursor.Position.Row - 1 >= 1)
                        SetCursorPosition(new Position(Cursor.Position.Row - 1,
                            Columns - Math.Abs(cells - Cursor.Position.Column)));
                    else
                        _logger.LogWarning("Cannot move cursor backward.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public void Erase(Position? from = null, Position? to = null)
        {
            from ??= new Position(1, 1);
            to ??= new Position(Rows, Columns);

            for (int i = from.Value.Row; i <= to.Value.Row; i++)
            for (int j = from.Value.Row == i ? from.Value.Column : 1;
                 j <= (i < to.Value.Row ? Columns : to.Value.Column);
                 j++)
                if (i <= _characters.Count && j <= _characters[i - 1].Count)
                    _characters[i - 1][j - 1] =
                        new Character(_currentGraphicAttributes);
        }

        public void Scroll(int lines, Direction direction)
        {
            int currentRows = _characters.Count;
            switch (direction)
            {
                case Direction.Up:
                    _rowOffset += lines;
                    for (int i = currentRows; i < Rows + _rowOffset; i++)
                        _characters.Add(new List<ICharacter>(GenerateNewRow(Columns)));
                    break;
                case Direction.Down:
                    _rowOffset -= lines;
                    if (_rowOffset < 0)
                    {
                        var absolute = Math.Abs(_rowOffset);
                        _rowOffset = 0;
                        for (int i = 0; i < absolute; i++)
                            _characters.Insert(0, new List<ICharacter>(GenerateNewRow(Columns)));
                    }

                    break;
                case Direction.Forward:
                case Direction.Back:
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public void RestoreCursor()
        {
            if (_savedCursorPosition.HasValue)
                Cursor.SetPosition(_savedCursorPosition.Value);
            _savedCursorPosition = null;
        }

        public void SaveCursor()
        {
            _savedCursorPosition = Cursor.Position;
        }

        public void SetGraphicsRendition(params GraphicsPair[] _graphicRenditionPairs)
        {
            _currentGraphicAttributes =
                GraphicsRenditionParser.Parse(_currentGraphicAttributes, _graphicRenditionPairs, _logger);
        }

        public void InsertLines(int linesToInsert = 1)
        {
            linesToInsert.For(() =>
            {
                _characters.Insert(Cursor.Position.Row - 1 + _rowOffset,
                    new List<ICharacter>(GenerateNewRow(Columns)));
            });
        }

        public void DeleteLines(int linesToDelete = 1)
        {
            if (Cursor.Position.Row - 1 + _rowOffset + linesToDelete > _characters.Count)
            {
                linesToDelete -= Math.Abs(_characters.Count - (Cursor.Position.Row - 1 + _rowOffset + linesToDelete));
            }

            _characters.RemoveRange(Cursor.Position.Row - 1 + _rowOffset, linesToDelete);
            var newRows = _characters.Count;
            if (newRows <= Rows)
            {
                _rowOffset = 0;
                if (newRows < Rows)
                {
                    (Rows - newRows).For(() => _characters.Add(new List<ICharacter>(GenerateNewRow(Columns))));
                }
            }
        }

        public void SetScrollingArea(int top, int bottom)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<ICharacter> GenerateNewRow(int columns)
        {
            return Enumerable.Repeat<ICharacter>(new Character(_currentGraphicAttributes), columns);
        }

        public void ClearSaved()
        {
            _logger.Log("ScrollBack buffer not implemented.");
        }

        public void ResetTabStops()
        {
            _clearedTabStops.Clear();
            _screenConfiguration = new DefaultScreenConfiguration(_screenConfiguration.ScreenDimensions,
                _screenConfiguration.FontDimensions);
        }

        public void ClearTabStop(int? column)
        {
            var tabStopSize = _screenConfiguration.TabStopSize;
            if (column.HasValue)
            {
                var tabStopToClear = column.Value % tabStopSize == 0
                    ? column.Value / tabStopSize
                    : Math.Clamp(column.Value / tabStopSize, 0,
                        Columns / tabStopSize) + 1;
                _clearedTabStops.Add(tabStopToClear);
            }
            else
            {
                _screenConfiguration = new DefaultScreenConfiguration(_screenConfiguration.ScreenDimensions.Rows,
                    _screenConfiguration.ScreenDimensions.Columns, 1, _screenConfiguration.FontDimensions);
                _clearedTabStops.Clear();
            }
        }

        public int GetNextTabStop(int column)
        {
            var tabStopSize = _screenConfiguration.TabStopSize;
            var nextTabStop = Math.Clamp(column / tabStopSize + 1, 0,
                Columns / tabStopSize);
            while (_clearedTabStops.Contains(nextTabStop))
                nextTabStop += 1;

            return Math.Clamp(nextTabStop, 0, Columns / tabStopSize);
        }

        public int GetPreviousTabStop(int column)
        {
            var tabStopSize = _screenConfiguration.TabStopSize;
            var previousTabStop = Math.Clamp(column / tabStopSize - 1, 0,
                Columns / tabStopSize);
            while (_clearedTabStops.Contains(previousTabStop))
                previousTabStop -= 1;

            return Math.Clamp(previousTabStop, 0, Columns / tabStopSize);
        }

        public int GetCurrentTabStop(int column)
        {
            var tabStopSize = _screenConfiguration.TabStopSize;
            var currentTabStop = Math.Clamp(column / tabStopSize, 0,
                Columns / tabStopSize);
            while (_clearedTabStops.Contains(currentTabStop))
                currentTabStop -= 1;

            currentTabStop = Math.Clamp(currentTabStop, 0, Columns / _screenConfiguration.TabStopSize);
            while (_clearedTabStops.Contains(currentTabStop))
                currentTabStop += 1;

            currentTabStop = Math.Clamp(currentTabStop, 0, Columns / _screenConfiguration.TabStopSize);
            return currentTabStop;
        }

        public int TabStopToColumn(int tabStop)
        {
            while (_clearedTabStops.Contains(tabStop))
                tabStop += 1;

            return Math.Clamp(tabStop * _screenConfiguration.TabStopSize, 1, Columns);
        }

        public override string ToString()
        {
            _screenIterator.Reset();
            var stringBuilder = new StringBuilder();
            foreach (var character in _screenIterator)
                stringBuilder.Append(character);
            return stringBuilder.ToString();
        }
    }

    internal static class IntExtensions
    {
        public static List<T> Repeat<T>(this int amount, Func<T> generator)
        {
            if (generator == null)
                return default;

            var collection = new List<T>();
            for (var i = 0; i < amount; i++)
                collection.Add(generator());

            return collection;
        }

        public static void For(this int amount, Action generator)
        {
            if (generator == null)
                return;

            for (var i = 0; i < amount; i++)
                generator();
        }
    }
}