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
        private TileView[,] _activeTetramino;
        private TileView[,] _nextTetramino;
        private TileView[,] _glassFul;
        private Game _game;

        public void StartGame()
        {
            _game = new GameObject("Game", typeof(Game)).GetComponent<Game>();
            _game.transform.SetParent(this.transform);
            _game.GameStart();
        }

        public void StopGame()
        {
            Destroy(_game.gameObject);
        }

        void Awake()
        {
            enabled = false;
            _glassFul = new TileView[23,10];
            _activeTetramino = new TileView[4, 4];
            _nextTetramino = new TileView[4, 4];
            StartGame();
            TetraminoViewCreate();
            enabled = true;
        }

        void TetraminoViewCreate()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (_game.ActiveTetramino[j, i, _game.Rotation])
                    {
                        TileView tile = Instantiate(_tilePref, _board.transform);
                        tile.transform.localScale = Vector3.one;
                        _activeTetramino[j, i] = tile;
                    }
                }
            }

           for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (_game.NextTetramino[j, i,Rotation.Angle0])
                    {
                        TileView tile = Instantiate(_tilePref, _board.transform);
                        tile.transform.localScale = Vector3.one;
                        _nextTetramino[j, i] = tile;
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
                    float x = _step*activePos.x+_step * i;
                    float y = _step*activePos.y+_step * j;
                    //float z = 0;
                    if(_activeTetramino[j, i])
                    {
                        _activeTetramino[j, i].transform.position = new Vector3(x, y);
                    }
                   
                }
            }
            for (int i = 0; i < 4; i++)
            {

                for (int j = 0; j < 4; j++)
                {
                    float x = _step * i;
                    float y = _step * j;
                    //float z = 0;
                    if (_nextTetramino[j, i])
                    {
                        _nextTetramino[j, i].transform.position = new Vector3(x, y);
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
