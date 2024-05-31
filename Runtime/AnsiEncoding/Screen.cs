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
        public event Action<byte[]> Output;
        public int Rows { get; }
        public int Columns { get; }
        public ICursor Cursor { get; }
        private List<List<ICharacter>> _characters;
        private int _rowOffset;
        private Position? _savedCursorPosition;
        private GraphicAttributes _currentGraphicAttributes;

        public Screen(Dimensions dimensions, ICursor cursor, ILogger logger)
        {
            _logger = logger;
            Rows = dimensions.Rows;
            Columns = dimensions.Columns;
            Cursor = cursor;
            Cursor.SetPosition(new Position(1, 1));
            _rowOffset = 0;
            _currentGraphicAttributes = new GraphicAttributes(AnsiColor.White, AnsiColor.Black);
            PopulateScreen(Rows, Columns);
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
                    for (int i = currentRows; i < currentRows + _rowOffset; i++)
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

        public void SetGraphicsRendition(int?[] customColor, params GraphicRendition[] _graphicRenditions)
        {
            foreach (GraphicRendition graphicRendition in _graphicRenditions)
            {
                switch (graphicRendition)
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
                    case GraphicRendition.NoUnderline:
                        _currentGraphicAttributes.UnderlineMode = UnderlineMode.None;
                        break;
                    case GraphicRendition.NoBlink:
                        _currentGraphicAttributes.BlinkSpeed = BlinkSpeed.None;
                        break;
                    case GraphicRendition.Reveal:
                        _currentGraphicAttributes.IsConcealed = false;
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
                    case GraphicRendition.Font1:
                        _logger.LogWarning("Trying to set GraphicsRendition 'Font1' now what!?");
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

        private IEnumerable<ICharacter> GenerateNewRow(int columns)
        {
            return Enumerable.Repeat<ICharacter>(new Character(), columns);
        }

        public void ClearSaved()
        {
            _logger.Log("ScrollBack buffer not implemented.");
        }
    }
}