using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using GameCore;

namespace GameView
{
    class GameView : MonoBehaviour
    {
        [SerializeField]
        Color32[] _colorsArray;
        [SerializeField]
        private GameObject _board;
        [SerializeField]
        private GameObject _pauseText;
        [SerializeField]
        private GameObject _gameOverText;
        [SerializeField]
        private Text _score;
        [SerializeField]
        private Text _lineCount;
        [SerializeField]
        private GameObject _tilePref;

        private float _step = 0.64f;
        private float _shiftX = 5f;
        private float _shiftY = 11.5f;
        private bool _gameState;
        private GameObject [,] _activeTetramino;
        private GameObject[,] _incomingTetramino;
        private GameObject[,] _glassFul;
        private List<GameObject> _tileList;
        private Game _game;

        void Awake()
        {
            enabled = false;
            _gameState = true;
            _tileList = new List<GameObject>();
            _glassFul = new GameObject[23, 10];
            _activeTetramino = new GameObject[4, 4];
            _incomingTetramino = new GameObject [4, 4];
            GameStart();
            TetraminoViewSet(_game.IncomingTetramino, _incomingTetramino);
            TetraminoViewSet(_game.ActiveTetramino, _activeTetramino);
            enabled = true;
        }

        public void GameStart()
        {
            _game = new GameObject("Game", typeof(Game)).GetComponent<Game>();
            _game.onInsert += _game_OnInsert;
            _game.onRotate += _game_OnRotate;
            _game.onGameOver += _game_OnGameOver;
            _game.transform.SetParent(this.transform);
            _game.GameStart();
        }

        public void GamePause()
        {
            if(_gameState)
            {
                _game.GameStop();
                enabled = false;
                _gameState = false;
                _pauseText.gameObject.SetActive(true);
            }
            
            else
            {
                _game.GameRun();
                enabled = true;
                _gameState = true;
                _pauseText.gameObject.SetActive(false);
            }
        }

        public void GameQuit()
        {
            Destroy(_game.gameObject);
            Application.Quit();
        }

        private void _game_OnRotate(object sender, EventArgs e)
        {
            TetraminoViewClear(_activeTetramino);
            TetraminoViewSet(_game.ActiveTetramino, _activeTetramino);
        }

        private void _game_OnInsert(object sender, EventArgs e)
        {
            if(enabled)
            {
                TetraminoViewClear(_incomingTetramino);
                TetraminoViewClear(_activeTetramino);
                GlassFullReDraw();
                TetraminoViewSet(_game.IncomingTetramino, _incomingTetramino);
                TetraminoViewSet(_game.ActiveTetramino, _activeTetramino);
                _score.text = "Score: "+_game.Score.ToString();
                _lineCount.text = "Lines : "+_game.LineCount.ToString();
            }
        }

        private void _game_OnGameOver(object sender, EventArgs e)
        {
            enabled = false;
            _gameOverText.gameObject.SetActive(true);
        }

        void TetraminoViewClear(GameObject[,] tetraminoView)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (tetraminoView[j, i])
                    {
                        ReturnTile(tetraminoView[j, i]);
                        tetraminoView[j, i] = null;
                    }
                }
            }
        }

        void TetraminoViewSet(Tetramino tetramino, GameObject[,] tetraminoView)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (tetramino[j, i, _game.Rotation].State)
                    {
                        int colorIndex = tetramino[j, i, _game.Rotation].Color;
                        tetraminoView[j, i] = GetTile(_colorsArray[colorIndex]);
                    }
                }
            }
        }

        void TetraminoTilesSetPosition(GameObject[,] tetraminoView, Vector2Int Pos)
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

        GameObject GetTile(Color32 color)
        {
            GameObject tile;
            if (_tileList.Count ==0)
            {
                tile = Instantiate(_tilePref, _board.transform);
                tile.transform.localScale = Vector3.one;
                tile.GetComponent<SpriteRenderer>().color = color;
            }
            else
            {
                tile = _tileList[0];
                tile.GetComponent<SpriteRenderer>().color = color;
                tile.gameObject.SetActive(true);
               _tileList.RemoveAt(0);
            }
            return tile;
        }
        void ReturnTile(GameObject tile)
        {
             tile.transform.position = new Vector3(0, 0, 0);
            tile.gameObject.SetActive(false);
            _tileList.Add(tile);
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
                        _glassFul[j, i] = null;
                    }
                }
            }

            for (int j = 0; j < 23; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (_game.GlassFull[j, i].State)
                    {
                        int colorIndex = _game.GlassFull[j, i].Color;
                        _glassFul[j, i] = GetTile(_colorsArray[colorIndex]);

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
                GameQuit();
            }
            TetraminoTilesSetPosition(_activeTetramino, _game.ActiveTetraminoPos);
            TetraminoTilesSetPosition(_incomingTetramino, _game.IncomingTetraminoPos);
        }
    }
}
