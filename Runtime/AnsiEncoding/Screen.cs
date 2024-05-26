using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ILogger _logger;
        public event Action<byte[]> Output;
        public int Rows { get; }
        public int Columns { get; }
        public ICursor Cursor { get; }
        private List<List<ICharacter>> _characters;
        private int _rowOffset;
        private Position? _savedCursorPosition;
        private GraphicAttributes _currentGraphicAttributes;

        public Screen(int rows, int columns, ICursor cursor, ILogger logger)
        {
            _logger = logger;
            Rows = rows;
            Columns = columns;
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

        public void SetGraphicsRendition(params GraphicRendition[] _graphicRenditions)
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
                        _currentGraphicAttributes.Foreground = Color.black;
                        break;
                    case GraphicRendition.ForegroundNormalRed:
                        _currentGraphicAttributes.Foreground = new Color(222, 56, 43);
                        break;
                    case GraphicRendition.ForegroundNormalGreen:
                        _currentGraphicAttributes.Foreground = new Color(57, 181, 74);
                        break;
                    case GraphicRendition.ForegroundNormalYellow:
                        _currentGraphicAttributes.Foreground = new Color(255, 199, 6);
                        break;
                    case GraphicRendition.ForegroundNormalBlue:
                        _currentGraphicAttributes.Foreground = new Color(0, 111, 184);
                        break;
                    case GraphicRendition.ForegroundNormalMagenta:
                        _currentGraphicAttributes.Foreground = new Color(118, 38, 113);
                        break;
                    case GraphicRendition.ForegroundNormalCyan:
                        _currentGraphicAttributes.Foreground = new Color(44, 181, 233);
                        break;
                    case GraphicRendition.ForegroundNormalWhite:
                        _currentGraphicAttributes.Foreground = new Color(204, 204, 204);
                        break;
                    case GraphicRendition.ForegroundNormalReset:
                        _currentGraphicAttributes.Foreground = new Color(204, 204, 204);
                        break;
                    case GraphicRendition.BackgroundNormalBlack:
                        _currentGraphicAttributes.Background = Color.black;
                        break;
                    case GraphicRendition.BackgroundNormalRed:
                        _currentGraphicAttributes.Background = new Color(222, 56, 43);
                        break;
                    case GraphicRendition.BackgroundNormalGreen:
                        _currentGraphicAttributes.Background = new Color(57, 181, 74);
                        break;
                    case GraphicRendition.BackgroundNormalYellow:
                        _currentGraphicAttributes.Background = new Color(255, 199, 6);
                        break;
                    case GraphicRendition.BackgroundNormalBlue:
                        _currentGraphicAttributes.Background = new Color(0, 111, 184);
                        break;
                    case GraphicRendition.BackgroundNormalMagenta:
                        _currentGraphicAttributes.Background = new Color(118, 38, 113);
                        break;
                    case GraphicRendition.BackgroundNormalCyan:
                        _currentGraphicAttributes.Background = new Color(44, 181, 233);
                        break;
                    case GraphicRendition.BackgroundNormalWhite:
                        _currentGraphicAttributes.Background = new Color(204, 204, 204);
                        break;
                    case GraphicRendition.BackgroundNormalReset:
                        _currentGraphicAttributes.Background = Color.black;
                        break;
                    case GraphicRendition.ForegroundBrightBlack:
                        _currentGraphicAttributes.Foreground = new Color(128, 128, 128);
                        break;
                    case GraphicRendition.ForegroundBrightRed:
                        _currentGraphicAttributes.Foreground = new Color(255, 0, 0);
                        break;
                    case GraphicRendition.ForegroundBrightGreen:
                        _currentGraphicAttributes.Foreground = new Color(0, 255, 0);
                        break;
                    case GraphicRendition.ForegroundBrightYellow:
                        _currentGraphicAttributes.Foreground = new Color(255, 255, 0);
                        break;
                    case GraphicRendition.ForegroundBrightBlue:
                        _currentGraphicAttributes.Foreground = new Color(0, 0, 255);
                        break;
                    case GraphicRendition.ForegroundBrightMagenta:
                        _currentGraphicAttributes.Foreground = new Color(255, 0, 255);
                        break;
                    case GraphicRendition.ForegroundBrightCyan:
                        _currentGraphicAttributes.Foreground = new Color(0, 255, 255);
                        break;
                    case GraphicRendition.ForegroundBrightWhite:
                        _currentGraphicAttributes.Foreground = new Color(255, 255, 255);
                        break;
                    case GraphicRendition.ForegroundBrightReset:
                        _currentGraphicAttributes.Foreground = new Color(204, 204, 204);
                        break;
                    case GraphicRendition.BackgroundBrightBlack:
                        _currentGraphicAttributes.Background = new Color(128, 128, 128);
                        break;
                    case GraphicRendition.BackgroundBrightRed:
                        _currentGraphicAttributes.Background = new Color(255, 0, 0);
                        break;
                    case GraphicRendition.BackgroundBrightGreen:
                        _currentGraphicAttributes.Background = new Color(0, 255, 0);
                        break;
                    case GraphicRendition.BackgroundBrightYellow:
                        _currentGraphicAttributes.Background = new Color(255, 255, 0);
                        break;
                    case GraphicRendition.BackgroundBrightBlue:
                        _currentGraphicAttributes.Background = new Color(0, 0, 255);
                        break;
                    case GraphicRendition.BackgroundBrightMagenta:
                        _currentGraphicAttributes.Background = new Color(255, 0, 255);
                        break;
                    case GraphicRendition.BackgroundBrightCyan:
                        _currentGraphicAttributes.Background = new Color(0, 255, 255);
                        break;
                    case GraphicRendition.BackgroundBrightWhite:
                        _currentGraphicAttributes.Background = new Color(255, 255, 255);
                        break;
                    case GraphicRendition.BackgroundBrightReset:
                        _currentGraphicAttributes.Background = Color.black;
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