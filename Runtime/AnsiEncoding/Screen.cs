using System;
using System.Collections.Generic;
using System.Linq;
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
        public event Action<byte[]> Output;
        public int Rows { get; }
        public int Columns { get; }
        public ICursor Cursor { get; }
        IScreenConfiguration IScreen.ScreenConfiguration => _screenConfiguration;

        private List<List<ICharacter>> _characters;
        private int _rowOffset;
        private Position? _savedCursorPosition;
        private GraphicAttributes _currentGraphicAttributes;
        private readonly HashSet<int> _clearedTabStops;

        public Screen(Dimensions dimensions, ICursor cursor, ILogger logger, IScreenConfiguration screenConfiguration)
        {
            _screenConfiguration = screenConfiguration;
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

        public void SetGraphicsRendition(params GraphicsPair[] _graphicRenditions)
        {
            foreach (GraphicsPair graphicRendition in _graphicRenditions)
            {
                switch (graphicRendition.GraphicRendition)
                {
                    case GraphicRendition.Reset:
                        _currentGraphicAttributes.Reset();
                        break;
                    case GraphicRendition.Bold:
                        _currentGraphicAttributes.IsBold = true;
                        break;
                    case GraphicRendition.Faint:
                        _currentGraphicAttributes.IsFaint = true;
                        break;
                    case GraphicRendition.Italic:
                        _currentGraphicAttributes.IsItalic = true;
                        break;
                    case GraphicRendition.Framed:
                        _currentGraphicAttributes.IsFramed = true;
                        break;
                    case GraphicRendition.OverLined:
                        _currentGraphicAttributes.IsOverLined = true;
                        break;
                    case GraphicRendition.NoOverLined:
                        _currentGraphicAttributes.IsOverLined = false;
                        break;
                    case GraphicRendition.Encircled:
                        _currentGraphicAttributes.IsEncircled = true;
                        break;
                    case GraphicRendition.NotFramedOrEncircled:
                        _currentGraphicAttributes.IsFramed = false;
                        _currentGraphicAttributes.IsEncircled = false;
                        break;
                    case GraphicRendition.StrikeThrough:
                        _currentGraphicAttributes.IsStrikeThrough = true;
                        break;
                    case GraphicRendition.Underline:
                        _currentGraphicAttributes.UnderlineMode = UnderlineMode.Single;
                        break;
                    case GraphicRendition.BlinkSlow:
                        _currentGraphicAttributes.BlinkSpeed = BlinkSpeed.Slow;
                        break;
                    case GraphicRendition.BlinkRapid:
                        _currentGraphicAttributes.BlinkSpeed = BlinkSpeed.Rapid;
                        break;
                    case GraphicRendition.Positive:
                    case GraphicRendition.Inverse:
                        (_currentGraphicAttributes.Foreground, _currentGraphicAttributes.Background) = (
                            _currentGraphicAttributes.Background, _currentGraphicAttributes.Foreground);
                        (_currentGraphicAttributes.ForegroundRGBColor, _currentGraphicAttributes.BackgroundRGBColor) = (
                            _currentGraphicAttributes.BackgroundRGBColor, _currentGraphicAttributes.ForegroundRGBColor);
                        _currentGraphicAttributes.UnderLineColorRGBColor = _currentGraphicAttributes.ForegroundRGBColor;
                        break;
                    case GraphicRendition.Conceal:
                        _currentGraphicAttributes.IsConcealed = true;
                        break;
                    case GraphicRendition.UnderlineDouble:
                        _currentGraphicAttributes.UnderlineMode = UnderlineMode.Double;
                        break;
                    case GraphicRendition.NormalIntensity:
                        _currentGraphicAttributes.IsBold = false;
                        _currentGraphicAttributes.IsFaint = false;
                        break;
                    case GraphicRendition.NoItalic:
                        _currentGraphicAttributes.IsItalic = false;
                        break;
                    case GraphicRendition.NoUnderline:
                        _currentGraphicAttributes.UnderlineMode = UnderlineMode.None;
                        break;
                    case GraphicRendition.NoBlink:
                        _currentGraphicAttributes.BlinkSpeed = BlinkSpeed.None;
                        break;
                    case GraphicRendition.Reveal:
                        _currentGraphicAttributes.IsConcealed = false;
                        break;
                    case GraphicRendition.NoStrikeThrough:
                        _currentGraphicAttributes.IsStrikeThrough = false;
                        break;
                    case GraphicRendition.ProportionalSpacing:
                        _currentGraphicAttributes.IsProportionalSpaced = true;
                        break;
                    case GraphicRendition.NoProportionalSpacing:
                        _currentGraphicAttributes.IsProportionalSpaced = false;
                        break;
                    case GraphicRendition.ForegroundNormalBlack:
                        _currentGraphicAttributes.Foreground = AnsiColor.Black;
                        break;
                    case GraphicRendition.ForegroundNormalRed:
                        _currentGraphicAttributes.Foreground = AnsiColor.Red;
                        break;
                    case GraphicRendition.ForegroundNormalGreen:
                        _currentGraphicAttributes.Foreground = AnsiColor.Green;
                        break;
                    case GraphicRendition.ForegroundNormalYellow:
                        _currentGraphicAttributes.Foreground = AnsiColor.Yellow;
                        break;
                    case GraphicRendition.ForegroundNormalBlue:
                        _currentGraphicAttributes.Foreground = AnsiColor.Blue;
                        break;
                    case GraphicRendition.ForegroundNormalMagenta:
                        _currentGraphicAttributes.Foreground = AnsiColor.Magenta;
                        break;
                    case GraphicRendition.ForegroundNormalCyan:
                        _currentGraphicAttributes.Foreground = AnsiColor.Cyan;
                        break;
                    case GraphicRendition.ForegroundNormalWhite:
                        _currentGraphicAttributes.Foreground = AnsiColor.White;
                        break;
                    case GraphicRendition.ForegroundNormalReset:
                        _currentGraphicAttributes.Foreground = AnsiColor.White;
                        break;
                    case GraphicRendition.BackgroundNormalBlack:
                        _currentGraphicAttributes.Background = AnsiColor.Black;
                        break;
                    case GraphicRendition.BackgroundNormalRed:
                        _currentGraphicAttributes.Background = AnsiColor.Red;
                        break;
                    case GraphicRendition.BackgroundNormalGreen:
                        _currentGraphicAttributes.Background = AnsiColor.Green;
                        break;
                    case GraphicRendition.BackgroundNormalYellow:
                        _currentGraphicAttributes.Background = AnsiColor.Yellow;
                        break;
                    case GraphicRendition.BackgroundNormalBlue:
                        _currentGraphicAttributes.Background = AnsiColor.Blue;
                        break;
                    case GraphicRendition.BackgroundNormalMagenta:
                        _currentGraphicAttributes.Background = AnsiColor.Magenta;
                        break;
                    case GraphicRendition.BackgroundNormalCyan:
                        _currentGraphicAttributes.Background = AnsiColor.Cyan;
                        break;
                    case GraphicRendition.BackgroundNormalWhite:
                        _currentGraphicAttributes.Background = AnsiColor.White;
                        break;
                    case GraphicRendition.BackgroundNormalReset:
                        _currentGraphicAttributes.Background = AnsiColor.Black;
                        break;
                    case GraphicRendition.ForegroundBrightBlack:
                        _currentGraphicAttributes.Foreground = AnsiColor.BrightBlack;
                        break;
                    case GraphicRendition.ForegroundBrightRed:
                        _currentGraphicAttributes.Foreground = AnsiColor.BrightRed;
                        break;
                    case GraphicRendition.ForegroundBrightGreen:
                        _currentGraphicAttributes.Foreground = AnsiColor.BrightGreen;
                        break;
                    case GraphicRendition.ForegroundBrightYellow:
                        _currentGraphicAttributes.Foreground = AnsiColor.BrightYellow;
                        break;
                    case GraphicRendition.ForegroundBrightBlue:
                        _currentGraphicAttributes.Foreground = AnsiColor.BrightBlue;
                        break;
                    case GraphicRendition.ForegroundBrightMagenta:
                        _currentGraphicAttributes.Foreground = AnsiColor.BrightMagenta;
                        break;
                    case GraphicRendition.ForegroundBrightCyan:
                        _currentGraphicAttributes.Foreground = AnsiColor.BrightCyan;
                        break;
                    case GraphicRendition.ForegroundBrightWhite:
                        _currentGraphicAttributes.Foreground = AnsiColor.BrightWhite;
                        break;
                    case GraphicRendition.ForegroundBrightReset:
                        _currentGraphicAttributes.Foreground = AnsiColor.White;
                        break;
                    case GraphicRendition.BackgroundBrightBlack:
                        _currentGraphicAttributes.Background = AnsiColor.BrightBlack;
                        break;
                    case GraphicRendition.BackgroundBrightRed:
                        _currentGraphicAttributes.Background = AnsiColor.BrightRed;
                        break;
                    case GraphicRendition.BackgroundBrightGreen:
                        _currentGraphicAttributes.Background = AnsiColor.BrightGreen;
                        break;
                    case GraphicRendition.BackgroundBrightYellow:
                        _currentGraphicAttributes.Background = AnsiColor.BrightYellow;
                        break;
                    case GraphicRendition.BackgroundBrightBlue:
                        _currentGraphicAttributes.Background = AnsiColor.BrightBlue;
                        break;
                    case GraphicRendition.BackgroundBrightMagenta:
                        _currentGraphicAttributes.Background = AnsiColor.BrightMagenta;
                        break;
                    case GraphicRendition.BackgroundBrightCyan:
                        _currentGraphicAttributes.Background = AnsiColor.BrightCyan;
                        break;
                    case GraphicRendition.BackgroundBrightWhite:
                        _currentGraphicAttributes.Background = AnsiColor.BrightWhite;
                        break;
                    case GraphicRendition.BackgroundBrightReset:
                        _currentGraphicAttributes.Background = AnsiColor.Black;
                        break;
                    case GraphicRendition.ForegroundColor:
                        _currentGraphicAttributes.Foreground = AnsiColor.Rgb;
                        var hasCustomUnderline = _currentGraphicAttributes.ForegroundRGBColor !=
                                                 _currentGraphicAttributes.UnderLineColorRGBColor;
                        _currentGraphicAttributes.ForegroundRGBColor = new RgbColor(graphicRendition.Color[0].Value,
                            graphicRendition.Color[1].Value, graphicRendition.Color[2].Value);
                        if (!hasCustomUnderline)
                            _currentGraphicAttributes.UnderLineColorRGBColor =
                                _currentGraphicAttributes.ForegroundRGBColor;
                        break;
                    case GraphicRendition.BackgroundColor:
                        _currentGraphicAttributes.Background = AnsiColor.Rgb;
                        _currentGraphicAttributes.BackgroundRGBColor = new RgbColor(graphicRendition.Color[0].Value,
                            graphicRendition.Color[1].Value, graphicRendition.Color[2].Value);
                        break;
                    case GraphicRendition.UnderLineColor:
                        _currentGraphicAttributes.UnderLineColorRGBColor = new RgbColor(graphicRendition.Color[0].Value,
                            graphicRendition.Color[1].Value, graphicRendition.Color[2].Value);
                        break;
                    case GraphicRendition.ResetUnderLineColor:
                        _currentGraphicAttributes.UnderLineColorRGBColor = _currentGraphicAttributes.ForegroundRGBColor;
                        break;
                    case GraphicRendition.SuperScript:
                        _currentGraphicAttributes.ScriptMode = ScriptMode.SuperScript;
                        break;
                    case GraphicRendition.Subscript:
                        _currentGraphicAttributes.ScriptMode = ScriptMode.SubScript;
                        break;
                    case GraphicRendition.NoSuperOrSubScript:
                        _currentGraphicAttributes.ScriptMode = ScriptMode.None;
                        break;
                    case GraphicRendition.AlternativeFont1:
                    case GraphicRendition.AlternativeFont2:
                    case GraphicRendition.AlternativeFont3:
                    case GraphicRendition.AlternativeFont4:
                    case GraphicRendition.AlternativeFont5:
                    case GraphicRendition.AlternativeFont6:
                    case GraphicRendition.AlternativeFont7:
                    case GraphicRendition.AlternativeFont8:
                    case GraphicRendition.AlternativeFont9:
                    case GraphicRendition.Fraktur:
                    case GraphicRendition.Font1:
                        _logger.LogWarning($"Trying to set GraphicsRendition Font {graphicRendition} now what!?");
                        break;
                    case GraphicRendition.RightSideLine:
                    case GraphicRendition.DoubleRightSideLine:
                    case GraphicRendition.LeftSideLine:
                    case GraphicRendition.DoubleLeftSideLine:
                    case GraphicRendition.IdeogramStressMarking:
                    case GraphicRendition.NoIdeogram:
                        _logger.LogWarning($"SGR command {(int)graphicRendition.GraphicRendition} not supported.");
                        break;
                    default:
                        throw new Exception("Unknown rendition command");
                }
            }
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
                        Columns / tabStopSize) + 1) ;// tabStopSize;
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