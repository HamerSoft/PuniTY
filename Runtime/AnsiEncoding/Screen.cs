using System;
using System.Collections.Generic;
using System.Linq;
using HamerSoft.PuniTY.AnsiEncoding.TerminalModes;
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

            public DefaultScreenConfiguration(int tabStopSize)
            {
                _customTabStopSize = tabStopSize;
            }

            public DefaultScreenConfiguration(DefaultScreenConfiguration other)
            {
                _customTabStopSize = other.TabStopSize;
            }
        }

        public readonly struct Dimensions
        {
            public readonly int Rows;
            public readonly int Columns;

            public Dimensions(int rows, int columns)
            {
                Rows = rows;
                Columns = columns;
            }
        }

        private readonly ILogger _logger;
        private IScreenConfiguration _screenConfiguration;
        private readonly IModeFactory _modeFactory;
        public event Action<byte[]> Output;
        public int Rows { get; }
        public int Columns { get; }
        public ICursor Cursor { get; }

        public event Action<IPointerMode> PointerModeChanged;

        IScreenConfiguration IScreen.ScreenConfiguration => _screenConfiguration;

        private List<List<ICharacter>> _characters;
        private int _rowOffset;
        private Position? _savedCursorPosition;
        private GraphicAttributes _currentGraphicAttributes;
        private readonly HashSet<int> _clearedTabStops;
        private Dictionary<AnsiMode, IMode> _activeModes;
        private PointerMode _pointerMode;
        private readonly IPointerModeFactory _pointerModeFactory;

        public Screen(Dimensions dimensions,
            ICursor cursor,
            ILogger logger,
            IScreenConfiguration screenConfiguration,
            IModeFactory modeFactory,
            IPointerModeFactory pointerModeFactory)
        {
            _activeModes = new Dictionary<AnsiMode, IMode>();
            _screenConfiguration = screenConfiguration;
            _modeFactory = modeFactory;
            _pointerModeFactory = pointerModeFactory;
            _logger = logger;
            Rows = dimensions.Rows;
            Columns = dimensions.Columns;
            Cursor = cursor;
            Cursor.SetPosition(new Position(1, 1));
            _rowOffset = 0;
            _currentGraphicAttributes = new GraphicAttributes(AnsiColor.White, AnsiColor.Black);
            PopulateScreen(Rows, Columns);
            _clearedTabStops = new HashSet<int>();
        }

        private void PopulateScreen(int rows, int columns)
        {
            _characters = new List<List<ICharacter>>(rows);
            for (int i = 0; i < rows; i++)
            {
                _characters.Add(new List<ICharacter>(columns));
                for (int j = 0; j < Columns; j++)
                    _characters[i].Add(new Character());
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
                : new Character();
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
                    _characters[Cursor.Position.Row + _rowOffset - 1][Cursor.Position.Column - 1] =
                        new Character(_currentGraphicAttributes, character);
                    MoveCursor(1, Direction.Forward);
                    break;
            }
        }

        public void InsertCharacters(int charactersToInsert)
        {
            for (var i = 0; i < charactersToInsert; i++)
                _characters[Cursor.Position.Row + _rowOffset - 1].Insert(Cursor.Position.Column - 1, new Character());

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
                _characters[^1].Add(new Character());
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
                    _characters[i - 1][j - 1] = new Character(_currentGraphicAttributes);
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

        void IScreen.Transmit(byte[] data)
        {
            Output?.Invoke(data);
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

        private IEnumerable<ICharacter> GenerateNewRow(int columns)
        {
            return Enumerable.Repeat<ICharacter>(new Character(), columns);
        }

        public void ClearSaved()
        {
            _logger.Log("ScrollBack buffer not implemented.");
        }

        public void ResetTabStops()
        {
            _clearedTabStops.Clear();
            _screenConfiguration = new DefaultScreenConfiguration();
        }

        public void ClearTabStop(int? column)
        {
            var tabStopSize = _screenConfiguration.TabStopSize;
            if (column.HasValue)
            {
                var tabStopToClear = column.Value % tabStopSize == 0
                    ? column.Value / tabStopSize
                    : (Math.Clamp(column.Value / tabStopSize, 0,
                        Columns / tabStopSize) + 1); // tabStopSize;
                _clearedTabStops.Add(tabStopToClear);
            }
            else
            {
                _screenConfiguration = new DefaultScreenConfiguration(1);
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

        public void SetMode(AnsiMode mode)
        {
            if (_activeModes.ContainsKey(mode))
            {
                _logger.LogWarning($"Mode: {mode} already active.");
                return;
            }

            var iMode = _modeFactory.Create(mode, this);
            if (iMode != null)
            {
                _activeModes.Add(mode, iMode);
                iMode.Enable(this);
            }
            else
            {
                _logger.LogWarning($"No implementation for terminal mode {mode}. Skipping command...");
            }
        }

        public void ResetMode(AnsiMode mode)
        {
            if (_activeModes.Remove(mode, out var iMode))
                iMode.Disable(this);
            else
                _logger.LogWarning($"Mode: {mode} is not active.");
        }

        public bool HasMode(params AnsiMode[] mode)
        {
            for (int i = 0; i < mode.Length; i++)
                if (!_activeModes.ContainsKey(mode[i]))
                    return false;
            return true;
        }

        void IPointerable.SetPointerMode(PointerMode mode)
        {
            if (mode == _pointerMode)
                return;
            _pointerMode = mode;
            PointerModeChanged?.Invoke(_pointerModeFactory.Create(this, mode));
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