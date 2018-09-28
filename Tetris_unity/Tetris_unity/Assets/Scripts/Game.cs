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
        private System.Random _random;


        void Awake()
        {
            _down.y = _step;
            _left.x = -_step;
            _right.x = _step;
            _startPosition.y = 4* _step;
            _tetraminoTypes = new TetraminoTypes();
            _colorsArray = new Color32[_tetraminoTypes.TetraminoTypesArray.Length];
            _glassful = new bool[_glassfulHigh, _glassfulWidth];
            GameStart();
        }

        public void GameStart()
        {
            TetraminoIndexGenerate();
            _nextTetramino = new Tetramino(_tetraminoTypes.TetraminoTypesArray[_tetraminoIndex], _tetraminoTypes.TetraminoShiftVectorsArray[_tetraminoIndex], _colorsArray[_tetraminoIndex]);
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
            _nextTetramino = new Tetramino(_tetraminoTypes.TetraminoTypesArray[_tetraminoIndex], _tetraminoTypes.TetraminoShiftVectorsArray[_tetraminoIndex], _colorsArray[_tetraminoIndex]);

            if (IsMoveable(_activeTetramino, _startPosition))
            {
                TetraminoMove(_activeTetramino, _startPosition);
                TetraminoInsert(_nextTetramino);
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
                    if (activeTetramino.TetraminoExemplar[j, i])
                    {
                        _glassful[Pos.y + j, Pos.x + i] = activeTetramino.TetraminoExemplar[j, i];
                    }
                }
            }
        }

        void TetraminoDelete(Tetramino activeTetramino)
        {
            Vector2Int Pos = activeTetramino.Pos;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (activeTetramino.TetraminoExemplar[j, i])
                    {
                        _glassful[j + Pos.y, i + Pos.x] = false;
                    }
                }
            }
        }

        void TetraminoMove(Tetramino activeTetramino, Vector2Int direct)
        {
            TetraminoDelete(activeTetramino);
            activeTetramino.Move(direct);
            TetraminoInsert(activeTetramino);
        }

        void TetraminoRotate(Tetramino activeTetramino)
        {
            TetraminoDelete(activeTetramino);
            int numberRotate = activeTetramino.NumberRotate;
            activeTetramino.Rotate();
            TetraminoShiftAfterRotate(activeTetramino, numberRotate);
            TetraminoInsert(activeTetramino);
            //TestDraw();
        }

        void TetraminoShiftAfterRotate(Tetramino activeTetramino, int numberRotate)
        {
            Vector2Int direct = activeTetramino.ShiftVector[numberRotate];
            activeTetramino.Move(direct);
        }

        bool IsRotatable(Tetramino activeTetramino)
        {
            TetraminoDelete(activeTetramino);
            int numberRotate = activeTetramino.NumberRotate;
            activeTetramino.Rotate();
            TetraminoShiftAfterRotate(activeTetramino, numberRotate);
            bool result = true;
            Vector2Int Pos = activeTetramino.Pos;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (!activeTetramino.TetraminoExemplar[j, i])
                    {
                        continue;
                    }
                    if ((j + Pos.y >= _glassfulHigh || i + Pos.x >= _glassfulWidth || i + Pos.x < 0 || _glassful[j + Pos.y, i + Pos.x]))
                    {
                        result = false;
                    }
                }
            }
            numberRotate = activeTetramino.NumberRotate;
            activeTetramino.Rotate();
            TetraminoShiftAfterRotate(activeTetramino, numberRotate);
            numberRotate = activeTetramino.NumberRotate;
            activeTetramino.Rotate();
            TetraminoShiftAfterRotate(activeTetramino, numberRotate);
            numberRotate = activeTetramino.NumberRotate;
            activeTetramino.Rotate();
            TetraminoShiftAfterRotate(activeTetramino, numberRotate);
            TetraminoInsert(activeTetramino);
            return result;
        }

        bool IsMoveable(Tetramino activeTetramino, Vector2Int direct)
        {
            TetraminoDelete(activeTetramino);
            bool result = true;
            Vector2Int Pos;
            Pos = activeTetramino.Pos;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (!activeTetramino.TetraminoExemplar[j, i])
                    {
                        continue;
                    }
                    if ((j + Pos.y + direct.y >= _glassfulHigh || i + Pos.x + direct.x >= _glassfulWidth || i + Pos.x + direct.x < 0 || _glassful[j + Pos.y + direct.y, i + Pos.x + direct.x]))
                    {
                        result = false;
                    }
                }
            }
            TetraminoInsert(activeTetramino);
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
                TetraminoMove(_activeTetramino, _down);
            }
            else
            {
                CheckFillingLineAndErase();
                NewTetraminoCreate();
            }
        }

        // Временно.
        void TestDraw()
        {
            string text = string.Empty;
            for (int j = 0; j < _glassfulHigh; j++)
            {
                for (int i = 0; i < _glassfulWidth; i++)
                {
                    text = text + Convert.ToInt32(_glassful[j, i]).ToString() + "  ";
                }
                text = text + "\n";
            }
            _text.text = text;
        }

        void Update()
        {

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (IsMoveable(_activeTetramino, _down))
                {
                    TetraminoMove(_activeTetramino, _down);
                }

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
                if (IsRotatable(_activeTetramino))
                {
                    TetraminoRotate(_activeTetramino);
                }

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
