using System;
using System.Collections.Generic;
using System.Linq;
using HamerSoft.PuniTY.AnsiEncoding.ColorScheme;
using UnityEngine;
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
        private IColorScheme _colorScheme;

        public Screen(Dimensions dimensions, ICursor cursor, ILogger logger, IColorScheme colorScheme)
        {
            _colorScheme = colorScheme;
            _logger = logger;
            Rows = dimensions.Rows;
            Columns = dimensions.Columns;
            Cursor = cursor;
            Cursor.SetPosition(new Position(1, 1));
            _rowOffset = 0;
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
            var parsedCustomColor = ParseCustomColor(customColor);

            Color GetColor(Color colorSchemeColor)
            {
                return parsedCustomColor ?? colorSchemeColor;
            }


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
                        _currentGraphicAttributes.Foreground = GetColor(_colorScheme.Black);
                        break;
                    case GraphicRendition.ForegroundNormalRed:
                        _currentGraphicAttributes.Foreground = GetColor(_colorScheme.Red);
                        break;
                    case GraphicRendition.ForegroundNormalGreen:
                        _currentGraphicAttributes.Foreground = GetColor(_colorScheme.Green);
                        break;
                    case GraphicRendition.ForegroundNormalYellow:
                        _currentGraphicAttributes.Foreground = GetColor(_colorScheme.Yellow);
                        break;
                    case GraphicRendition.ForegroundNormalBlue:
                        _currentGraphicAttributes.Foreground = GetColor(_colorScheme.Blue);
                        break;
                    case GraphicRendition.ForegroundNormalMagenta:
                        _currentGraphicAttributes.Foreground = GetColor(_colorScheme.Magenta);
                        break;
                    case GraphicRendition.ForegroundNormalCyan:
                        _currentGraphicAttributes.Foreground = GetColor(_colorScheme.Cyan);
                        break;
                    case GraphicRendition.ForegroundNormalWhite:
                        _currentGraphicAttributes.Foreground = GetColor(_colorScheme.White);
                        break;
                    case GraphicRendition.ForegroundNormalReset:
                        _currentGraphicAttributes.Foreground = GetColor(_colorScheme.White);
                        break;
                    case GraphicRendition.BackgroundNormalBlack:
                        _currentGraphicAttributes.Background = GetColor(_colorScheme.Black);
                        break;
                    case GraphicRendition.BackgroundNormalRed:
                        _currentGraphicAttributes.Background = GetColor(_colorScheme.Red);
                        break;
                    case GraphicRendition.BackgroundNormalGreen:
                        _currentGraphicAttributes.Background = GetColor(_colorScheme.Green);
                        break;
                    case GraphicRendition.BackgroundNormalYellow:
                        _currentGraphicAttributes.Background = GetColor(_colorScheme.Yellow);
                        break;
                    case GraphicRendition.BackgroundNormalBlue:
                        _currentGraphicAttributes.Background = GetColor(_colorScheme.Blue);
                        break;
                    case GraphicRendition.BackgroundNormalMagenta:
                        _currentGraphicAttributes.Background = GetColor(_colorScheme.Magenta);
                        break;
                    case GraphicRendition.BackgroundNormalCyan:
                        _currentGraphicAttributes.Background = GetColor(_colorScheme.Cyan);
                        break;
                    case GraphicRendition.BackgroundNormalWhite:
                        _currentGraphicAttributes.Background = GetColor(_colorScheme.White);
                        break;
                    case GraphicRendition.BackgroundNormalReset:
                        _currentGraphicAttributes.Background = GetColor(_colorScheme.Black);
                        break;
                    case GraphicRendition.ForegroundBrightBlack:
                        _currentGraphicAttributes.Foreground = GetColor(_colorScheme.BrightBlack);
                        break;
                    case GraphicRendition.ForegroundBrightRed:
                        _currentGraphicAttributes.Foreground = GetColor(_colorScheme.BrightRed);
                        break;
                    case GraphicRendition.ForegroundBrightGreen:
                        _currentGraphicAttributes.Foreground = GetColor(_colorScheme.BrightGreen);
                        break;
                    case GraphicRendition.ForegroundBrightYellow:
                        _currentGraphicAttributes.Foreground = GetColor(_colorScheme.BrightYellow);
                        break;
                    case GraphicRendition.ForegroundBrightBlue:
                        _currentGraphicAttributes.Foreground = GetColor(_colorScheme.BrightBlue);
                        break;
                    case GraphicRendition.ForegroundBrightMagenta:
                        _currentGraphicAttributes.Foreground = GetColor(_colorScheme.BrightMagenta);
                        break;
                    case GraphicRendition.ForegroundBrightCyan:
                        _currentGraphicAttributes.Foreground = GetColor(_colorScheme.BrightCyan);
                        break;
                    case GraphicRendition.ForegroundBrightWhite:
                        _currentGraphicAttributes.Foreground = GetColor(_colorScheme.BrightWhite);
                        break;
                    case GraphicRendition.ForegroundBrightReset:
                        _currentGraphicAttributes.Foreground = GetColor(_colorScheme.White);
                        break;
                    case GraphicRendition.BackgroundBrightBlack:
                        _currentGraphicAttributes.Background = GetColor(_colorScheme.BrightBlack);
                        break;
                    case GraphicRendition.BackgroundBrightRed:
                        _currentGraphicAttributes.Background = GetColor(_colorScheme.BrightRed);
                        break;
                    case GraphicRendition.BackgroundBrightGreen:
                        _currentGraphicAttributes.Background = GetColor(_colorScheme.BrightGreen);
                        break;
                    case GraphicRendition.BackgroundBrightYellow:
                        _currentGraphicAttributes.Background = GetColor(_colorScheme.BrightYellow);
                        break;
                    case GraphicRendition.BackgroundBrightBlue:
                        _currentGraphicAttributes.Background = GetColor(_colorScheme.BrightBlue);
                        break;
                    case GraphicRendition.BackgroundBrightMagenta:
                        _currentGraphicAttributes.Background = GetColor(_colorScheme.BrightMagenta);
                        break;
                    case GraphicRendition.BackgroundBrightCyan:
                        _currentGraphicAttributes.Background = GetColor(_colorScheme.BrightCyan);
                        break;
                    case GraphicRendition.BackgroundBrightWhite:
                        _currentGraphicAttributes.Background = GetColor(_colorScheme.BrightWhite);
                        break;
                    case GraphicRendition.BackgroundBrightReset:
                        _currentGraphicAttributes.Background = GetColor(_colorScheme.Black);
                        break;
                    case GraphicRendition.Font1:
                        _logger.LogWarning("Trying to set GraphicsRendition 'Font1' now what!?");
                        break;
                    default:
                        throw new Exception("Unknown rendition command");
                }
            }
        }

        private Color? ParseCustomColor(int?[] customColor)
        {
            if (customColor.Length == 0 || customColor[0] == null)
                return null;

            // check for eight bit color
            if (customColor[1] == null)
                return EightBitColorToRGB(customColor[0].Value);

            // check if full RGB color
            if (customColor[2] != null)
                return new Color(customColor[0].Value, customColor[1].Value, customColor[2].Value);

            _logger.LogWarning(
                $"Failed to parse custom Color, its neither an 8 bit or an RGB color {string.Join(';', customColor)}.");
            return null;
        }

        /// <summary>
        /// Convert an 8bit color into RGB
        /// </summary>
        /// <param name="c">color number</param>
        /// <returns>rgb color</returns>
        private static Color EightBitColorToRGB(int c)
        {
            return new Color(((c >> 4) % 4) * 64,
                ((c >> 2) % 4) * 64,
                (c % 4) * 64,
                (c >> 6) * 64);
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