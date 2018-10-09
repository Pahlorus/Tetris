using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using GameCore;

namespace GameView
{
    class GameView : MonoBehaviour
    {
        [SerializeField]
        private GameObject _board;
        [SerializeField]
        private TileView _tilePref;
        private float _step = 0.64f;
        private float _shiftX = 5f;
        private float _shiftY = 11.5f;
        private TileView[,] _activeTetramino;
        private TileView[,] _incomingTetramino;
        private TileView[,] _glassFul;
        private List<TileView> _tileList;
        private Game _game;

        void Awake()
        {
            enabled = false;
            _tileList = new List<TileView>();
            _glassFul = new TileView[23, 10];
            _activeTetramino = new TileView[4, 4];
            _incomingTetramino = new TileView[4, 4];
            StartGame();
            TetraminoViewSet(_game.IncomingTetramino, _incomingTetramino);
            TetraminoViewSet(_game.ActiveTetramino, _activeTetramino);
            enabled = true;
        }

        public void StartGame()
        {
            _game = new GameObject("Game", typeof(Game)).GetComponent<Game>();
            _game.onInsert += _game_OnInsert;
            _game.onRotate += _game_OnRotate;
            _game.transform.SetParent(this.transform);
            _game.GameStart();
        }

        public void StopGame()
        {
            Destroy(_game.gameObject);
        }

        private void _game_OnRotate(object sender, EventArgs e)
        {
            TetraminoViewClear(_activeTetramino);
            TetraminoViewSet(_game.ActiveTetramino, _activeTetramino);
        }

        private void _game_OnInsert(object sender, EventArgs e)
        {
            TetraminoViewClear(_incomingTetramino);
            TetraminoViewClear(_activeTetramino);
            GlassFullReDraw();
            TetraminoViewSet(_game.IncomingTetramino, _incomingTetramino);
            TetraminoViewSet(_game.ActiveTetramino, _activeTetramino);
        }

        void TetraminoViewClear(TileView[,] tetraminoView)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (tetraminoView[j, i])
                    {
                        ReturnTile(tetraminoView[j, i]);
                    }
                }
            }
        }

        void TetraminoViewSet(Tetramino tetramino, TileView[,] tetraminoView)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (tetramino[j, i, _game.Rotation])
                    {
                        tetraminoView[j, i] = GetTile();
                    }
                }
            }
        }

        void TetraminoTilesSetPosition(TileView[,] tetraminoView, Vector2Int Pos)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    float x = _step * (Pos.x + i - _shiftX + 0.5f);
                    float y = _step * (-Pos.y - j + _shiftY - 0.5f);
                    if (tetraminoView[j, i])
                    {
                        tetraminoView[j, i].transform.position = new Vector3(x, y);
                    }
                }
            }
        }

        TileView GetTile(/*float x, float y*/)
        {
            TileView tile = Instantiate(_tilePref, _board.transform);
            tile.transform.localScale = Vector3.one;
            return tile;
        }
        void ReturnTile(TileView tile)
        {
            Destroy(tile.gameObject);
        }



        void GlassFullReDraw()
        {
            Vector2Int activePos = _game.ActiveTetraminoPos;

            for (int j = 0; j < 23; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (_glassFul[j, i])
                    {
                        ReturnTile(_glassFul[j, i]);
                    }
                }
            }

            for (int j = 0; j < 23; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (_game.GlassFull[j, i])
                    {
                        _glassFul[j, i] = GetTile();

                        float x = (_step * i) - _shiftX * _step + 0.32f;
                        float y = -(_step * j) + _shiftY * _step - 0.32f; ;
                        _glassFul[j, i].transform.position = new Vector3(x, y);
                    }
                }
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopGame();
            }
            TetraminoTilesSetPosition(_activeTetramino, _game.ActiveTetraminoPos);
            TetraminoTilesSetPosition(_incomingTetramino, _game.IncomingTetraminoPos);

        }
    }
}
