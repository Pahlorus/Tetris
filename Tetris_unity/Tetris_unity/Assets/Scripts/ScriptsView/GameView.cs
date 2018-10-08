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
        private TileView[,] _nextTetramino;
        private TileView[,] _glassFul;
        private List<TileView> _tileList;
        private Game _game;

        public void StartGame()
        {
            _game = new GameObject("Game", typeof(Game)).GetComponent<Game>();
            _game.onInsert += _game_OnInsert;
            _game.onRotate += _game_OnRotate;
            _game.transform.SetParent(this.transform);
            _game.GameStart();
        }

        private void _game_OnRotate(object sender, EventArgs e)
        {
            TetraminoViewDelete();
            TetraminoViewCreate();
        }

        private void _game_OnInsert(object sender, EventArgs e)
        {
            TetraminoViewDelete();
            GlassFullReDraw();
            TetraminoViewCreate();
        }

        public void StopGame()
        {
            Destroy(_game.gameObject);
        }

        void Awake()
        {
            enabled = false;
            _tileList = new List<TileView>();
            _glassFul = new TileView[23,10];
            _activeTetramino = new TileView[4, 4];
            _nextTetramino = new TileView[4, 4];
            StartGame();
            TetraminoViewCreate();
            enabled = true;
        }


        void TetraminoViewDelete()
        {


            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (_activeTetramino[j, i])
                    {
                        ReturnTile(_activeTetramino[j, i]);
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (_nextTetramino[j, i])
                    {
                        ReturnTile(_nextTetramino[j, i]);
                    }
                }
            }
            Array.Clear(_activeTetramino, 0, _activeTetramino.Length);
            Array.Clear(_nextTetramino, 0, _nextTetramino.Length);
        }



        void TetraminoViewCreate()
        {
            Array.Clear(_activeTetramino, 0, _activeTetramino.Length);
            Array.Clear(_nextTetramino, 0, _nextTetramino.Length);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (_game.ActiveTetramino[j, i, _game.Rotation])
                    {
                        _activeTetramino[j, i] = GetTile();
                    }
                }
            }

           for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (_game.NextTetramino[j, i,Rotation.Angle0])
                    {
                        _nextTetramino[j, i] = GetTile();
                    }
                }
            }


        }

        void TetraminoDraw()
        {
            Vector2Int activePos = _game.ActiveTetraminoPos;
            for (int i = 0; i < 4; i++)
            {
                
                for (int j = 0; j < 4; j++)
                {
                    float x = (_step*activePos.x+_step * i) - _shiftX*_step+0.32f;
                    float y = -(_step*activePos.y+_step * j)+_shiftY*_step - 0.32f;
                    //float z = 0;
                    if (_activeTetramino[j, i])
                    {
                        _activeTetramino[j, i].transform.position = new Vector3(x, y);
                    }
                   
                }
            }

            Vector2Int previewPos = _game.PreviewTetraminoPos;

            for (int i = 0; i < 4; i++)
            {

                for (int j = 0; j < 4; j++)
                {
                    float x = (_step * previewPos.x + _step * i) - _shiftX * _step + 0.32f;
                    float y = -(_step * previewPos.y + _step * j) + _shiftY * _step - 0.32f;
                    //float z = 0;
                    if (_nextTetramino[j, i])
                    {
                        _nextTetramino[j, i].transform.position = new Vector3(x, y);
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
            //tile = null;
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

                        float x = ( _step * i) - _shiftX * _step+0.32f;
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
            TetraminoDraw();

        }
    }
}
