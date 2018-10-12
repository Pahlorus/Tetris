using UnityEngine;

namespace GameView
{
    internal class TileView: MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        internal SpriteRenderer SpriteRenderer { get { return _spriteRenderer; } }
    }
}
