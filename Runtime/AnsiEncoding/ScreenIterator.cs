using System.Collections;
using System.Collections.Generic;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public sealed class ScreenIterator : IEnumerable<ICharacter>
    {
        private readonly IScreen _screen;
        private readonly Position _startPosition;
        private readonly Position _endPosition;
        private Position _currentPosition;

        public ScreenIterator(IScreen screen) : this(
            screen,
            new Position(1, 1),
            new Position(screen?.Rows ?? 1, screen?.Columns ?? 1))
        {
        }

        public ScreenIterator(IScreen screen, Position startPosition, Position endPosition)
        {
            _screen = screen;
            _startPosition = startPosition.IsValid(screen) ? startPosition : new Position(1, 1);
            _endPosition = endPosition.IsValid(screen)
                ? endPosition
                : new Position(screen?.Rows ?? 1, screen?.Columns ?? 1);
            _currentPosition = _startPosition;
        }

        public IEnumerator<ICharacter> GetEnumerator()
        {
            if (_screen == null || _endPosition < _startPosition)
                yield break;

            for (int i = _startPosition.Row; i <= _endPosition.Row; i++)
            {
                for (int j = i == _startPosition.Row ? _startPosition.Column : 1;
                     j <= (i == _endPosition.Row ? _endPosition.Column : _screen.Columns);
                     j++)
                    yield return _screen.GetCharacter(_currentPosition = new Position(i, j));
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Position CurrentPosition => _currentPosition;

        public void Reset()
        {
            _currentPosition = new Position(1, 1);
        }
    }
}