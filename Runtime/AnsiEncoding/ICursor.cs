using UnityEngine;

namespace HamerSoft.PuniTY.AnsiEncoding
{
    public interface ICursor
    {
        public Vector2Int Position { get; }

        internal void SetPosition(Vector2Int position);
    }
}