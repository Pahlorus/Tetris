using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore
{
    class Game : MonoBehaviour
    {

        [SerializeField]
        // Временно, для отображения игрового поля.
        private Text _text;
        [SerializeField]
        Color32[] _colorsArray;
        private int _glassfulHigh = 23;
        private int _glassfulWidth = 10;
        private int _step = 1;
        private int _tetraminoIndex;
        private float _tickTime = 1f;
        private float _elapsedTime = 0f;
        private bool[,] _glassful;
        private Vector2Int _down;
        private Vector2Int _left;
        private Vector2Int _right;
        private Vector2Int _startPosition;
        private Tetramino _activeTetramino;
        private Tetramino _nextTetramino;
        private Tetramino[] _tetraminoArray;
        private TetraminoTypes _tetraminoTypes;
        private Rotation _rotation;
        private System.Random _random;


        void Awake()
        {
            _down.y = _step;
            _left.x = -_step;
            _right.x = _step;
            _startPosition.y = 4 * _step;
            _tetraminoTypes = new TetraminoTypes();
            _colorsArray = new Color32[_tetraminoTypes.TetraminoTypesArray.Length];
            _glassful = new bool[_glassfulHigh, _glassfulWidth];
            GameStart();
        }

        public void GameStart()
        {
            TetraminoIndexGenerate();
            _nextTetramino = new Tetramino(_tetraminoTypes.TetraminoTypesArray[3], _tetraminoTypes.TetraminoShiftVectorsArray[3], _colorsArray[_tetraminoIndex]);
            //_nextTetramino = new Tetramino(_tetraminoTypes.TetraminoTypesArray[_tetraminoIndex], _tetraminoTypes.TetraminoShiftVectorsArray[_tetraminoIndex], _colorsArray[_tetraminoIndex]);
            NewTetraminoCreate();
            GameRun();
        }

        public void GameRun()
        {
            enabled = true;
        }

        public void GameStop()
        {
            enabled = false;
        }

        void TetraminoIndexGenerate()
        {
            _random = new System.Random();
            _tetraminoIndex = _random.Next(_tetraminoTypes.TetraminoTypesArray.Length);
        }

        void NewTetraminoCreate()
        {
            TetraminoIndexGenerate();
            _activeTetramino = _nextTetramino;
            _nextTetramino = new Tetramino(_tetraminoTypes.TetraminoTypesArray[3], _tetraminoTypes.TetraminoShiftVectorsArray[3], _colorsArray[_tetraminoIndex]);
            //_nextTetramino = new Tetramino(_tetraminoTypes.TetraminoTypesArray[_tetraminoIndex], _tetraminoTypes.TetraminoShiftVectorsArray[_tetraminoIndex], _colorsArray[_tetraminoIndex]);

            if (IsMoveable(_activeTetramino, _startPosition))
            {
                TetraminoMove(_activeTetramino, _startPosition);
            }
            else
            {
                GameStop();
            }
        }

        void TetraminoInsert(Tetramino activeTetramino)
        {
            Vector2Int Pos = activeTetramino.Pos;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (activeTetramino[j, i, _rotation])
                    {
                        _glassful[Pos.y + j, Pos.x + i] = activeTetramino[j, i, _rotation];
                    }
                }
            }
        }

        void TetraminoMove(Tetramino activeTetramino, Vector2Int direct)
        {
            activeTetramino.Move(direct);
        }

        /*void TetraminoRotate(Tetramino activeTetramino)
        {
            int numberRotate = activeTetramino.NumberRotate;
            activeTetramino.Rotate();
            TetraminoShiftAfterRotate(activeTetramino, numberRotate);
        }*/

        void TetraminoRotate()
        {
            _rotation = (Rotation)(((int)_rotation + 1) % 4);
            TetraminoShiftAfterRotate(_activeTetramino, _rotation);
        }




        void TetraminoShiftAfterRotate(Tetramino activeTetramino, Rotation rotation)
        {
            int numberRotate = (int)rotation;
            Vector2Int direct = activeTetramino.ShiftVector[numberRotate];
            activeTetramino.Move(direct);
        }

        bool IsCellHaveBlock(int y, int x)
        {
            bool isCheckActiveTetraminoBlocks = false;
            bool isCheckNextTetraminoBlocks = false;
            var posActiveTetramino = _activeTetramino.Pos;
            var posNextTetramino = _nextTetramino.Pos;

            if ((y >= posActiveTetramino.y & y < posActiveTetramino.y + 4 & x >= posActiveTetramino.x & x < posActiveTetramino.x + 4))
            {
                isCheckActiveTetraminoBlocks = _activeTetramino[y - posActiveTetramino.y, x - posActiveTetramino.x, _rotation];
            }
            // TODO: Временно, для отображения следующей фигуры
            if ((y >= posNextTetramino.y & y < posNextTetramino.y + 4 & x >= posNextTetramino.x & x < posNextTetramino.x + 4))
            {
                isCheckNextTetraminoBlocks = _nextTetramino.TetraminoExemplar[y - posNextTetramino.y, x - posNextTetramino.x];
            }



            return _glassful[y, x] || isCheckActiveTetraminoBlocks || isCheckNextTetraminoBlocks;
        }


        bool IsRotatable(Tetramino activeTetramino)
        {
          // Rotation numberRotate = _rotation;
            TetraminoRotate();
            //TetraminoShiftAfterRotate(activeTetramino, numberRotate);
            bool result = true;
            Vector2Int Pos = activeTetramino.Pos;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (!activeTetramino[j, i, _rotation])
                    {
                        continue;
                    }
                    if ((j + Pos.y >= _glassfulHigh || i + Pos.x >= _glassfulWidth || i + Pos.x < 0 || _glassful[j + Pos.y, i + Pos.x]))
                    {
                        result = false;
                    }
                }
            }
            /*numberRotate = _rotation;
            TetraminoRotate();
            TetraminoShiftAfterRotate(activeTetramino, numberRotate);
            numberRotate = _rotation;
            TetraminoRotate();
            TetraminoShiftAfterRotate(activeTetramino, numberRotate);
            numberRotate = _rotation;
            TetraminoRotate();
            TetraminoShiftAfterRotate(activeTetramino, numberRotate);*/
            return result;
        }

        bool IsMoveable(Tetramino activeTetramino, Vector2Int direct)
        {
            bool result = true;
            Vector2Int Pos;
            Pos = activeTetramino.Pos;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (!activeTetramino[j, i, _rotation])
                    {
                        continue;
                    }
                    if ((j + Pos.y + direct.y >= _glassfulHigh || i + Pos.x + direct.x >= _glassfulWidth || i + Pos.x + direct.x < 0 || _glassful[j + Pos.y + direct.y, i + Pos.x + direct.x]))
                    {
                        result = false;
                    }
                }
            }
            return result;
        }

        void LineEraseAndShiftGlassfull(int line)
        {
            for (int j = line; j >= _startPosition.y; j--)
            {
                for (int i = 0; i < _glassfulWidth; i++)
                {
                    _glassful[j, i] = _glassful[j - 1, i];
                }
            }
        }

        void CheckFillingLineAndErase()
        {
            bool isFillingLine;
            for (int j = 3; j < _glassfulHigh; j++)
            {
                isFillingLine = true;
                for (int i = 0; i < _glassfulWidth; i++)
                {
                    if (!_glassful[j, i])
                    {
                        isFillingLine = false;
                        break;
                    }
                }

                if (isFillingLine)
                {
                    LineEraseAndShiftGlassfull(j);
                }
            }
        }

        void Tick()
        {
            if (IsMoveable(_activeTetramino, _down))
            {
               // TetraminoMove(_activeTetramino, _down);
            }
            else
            {
                TetraminoInsert(_activeTetramino);
                CheckFillingLineAndErase();
                NewTetraminoCreate();
            }
        }

        // TODO: Временно.
        void TestDraw()
        {
            string text = string.Empty;
            for (int j = 0; j < _glassfulHigh; j++)
            {
                for (int i = 0; i < _glassfulWidth; i++)
                {
                    if (IsCellHaveBlock(j, i))
                        text = text + "0" + " ";
                    else
                        text = text + ".." + " ";
                }
                text = text + "\n";
            }
            _text.text = text;
        }

        void Update()
        {

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _tickTime = 0.05f;
                _elapsedTime = 0.05f;
            }

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                _tickTime = 1f;
                _elapsedTime = 0f;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (IsMoveable(_activeTetramino, _right))
                {
                    TetraminoMove(_activeTetramino, _right);
                }

            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (IsMoveable(_activeTetramino, _left))
                {
                    TetraminoMove(_activeTetramino, _left);
                }

            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                /*if (IsRotatable(_activeTetramino))
                {
                    TetraminoRotate();
                }*/

                TetraminoRotate();
            }

            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= _tickTime)
            {
                _elapsedTime -= _tickTime;
                Tick();
            }

            TestDraw();
        }
    }
}
