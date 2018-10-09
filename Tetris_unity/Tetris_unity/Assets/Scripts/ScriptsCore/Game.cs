﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore
{
    class Game : MonoBehaviour
    {
        [SerializeField]
        Color32[] _colorsArray;
        private int _glassfulHigh = 23;
        private int _glassfulWidth = 10;
        private int _step = 1;
        private int _initialPosX = 3;
        private int _initialPosY = 0;
        private float _tickTime = 1f;
        private float _elapsedTime = 0f;

        private bool[,] _glassful;
        private Vector2Int _down;
        private Vector2Int _left;
        private Vector2Int _right;
        private Vector2Int _startPosition;
        private Vector2Int _activeTetraminoPos;
        private Vector2Int _incomingTetraminoPos;
        private Tetramino _activeTetramino;
        private Tetramino _incomingTetramino;
        private Tetramino[] _tetraminoArray;
        private TetraminoTypes _tetraminoTypes;
        private Rotation _rotation;

        public bool[,] GlassFull { get { return _glassful; } }
        public Tetramino ActiveTetramino { get { return _activeTetramino; } }
        public Tetramino IncomingTetramino { get { return _incomingTetramino; } }
        public Vector2Int ActiveTetraminoPos { get { return _activeTetraminoPos; } }
        public Vector2Int IncomingTetraminoPos { get { return _incomingTetraminoPos; } }
        public Rotation Rotation { get { return _rotation; } }

        public EventHandler onInsert;
        public EventHandler onRotate;

        void Awake()
        {
            _down.y = _step;
            _left.x = -_step;
            _right.x = _step;
            _startPosition.y = 4 * _step;
            _incomingTetraminoPos.y = _initialPosY;
            _incomingTetraminoPos.x = _initialPosX;
            _tetraminoTypes = new TetraminoTypes();
            _colorsArray = new Color32[_tetraminoTypes.TetraminoTypesArray.Length];
            _glassful = new bool[_glassfulHigh, _glassfulWidth];
        }

        public void GameStart()
        {
            int tetraminoIndex = TetraminoIndexGenerate();
            _incomingTetramino = new Tetramino(_tetraminoTypes.TetraminoTypesArray[tetraminoIndex], _tetraminoTypes.TetraminoShiftVectorsArray[tetraminoIndex], _colorsArray[tetraminoIndex]);
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

        int TetraminoIndexGenerate()
        {
            return UnityEngine.Random.Range(0, _tetraminoTypes.TetraminoTypesArray.Length);
        }

        void NewTetraminoCreate()
        {
            int tetraminoIndex = TetraminoIndexGenerate();
            _activeTetramino = _incomingTetramino;
            _incomingTetramino = new Tetramino(_tetraminoTypes.TetraminoTypesArray[tetraminoIndex], _tetraminoTypes.TetraminoShiftVectorsArray[tetraminoIndex], _colorsArray[tetraminoIndex]);
            _rotation = Rotation.Angle0;
            _activeTetraminoPos = _incomingTetraminoPos;

            if (IsMoveable(_activeTetramino, _startPosition))
            {
                TetraminoMove(_startPosition);
            }
            else
            {
                GameStop();
            }
        }

        void TetraminoInsert(Tetramino activeTetramino)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (activeTetramino[j, i, _rotation])
                    {
                        _glassful[_activeTetraminoPos.y + j, _activeTetraminoPos.x + i] = activeTetramino[j, i, _rotation];
                    }
                }
            }
        }

        void TetraminoMove(Vector2Int direct)
        {
            _activeTetraminoPos = _activeTetraminoPos + direct;
        }

        void TetraminoRotate()
        {
            _rotation = (Rotation)(((int)_rotation + 1) % 4);
            TetraminoShiftAfterRotate(_activeTetramino, _rotation);
        }

        void TetraminoShiftAfterRotate(Tetramino activeTetramino, Rotation rotation)
        {
            int numberRotate = (int)rotation;
            Vector2Int direct = activeTetramino.ShiftVector[numberRotate];
            TetraminoMove(direct);
        }

        bool IsCellHaveBlock(int y, int x)
        {
            bool isCheckActiveTetraminoBlocks = false;
            bool isCheckNextTetraminoBlocks = false;
            if ((y >= _activeTetraminoPos.y & y < _activeTetraminoPos.y + 4 & x >= _activeTetraminoPos.x & x < _activeTetraminoPos.x + 4))
            {
                isCheckActiveTetraminoBlocks = _activeTetramino[y - _activeTetraminoPos.y, x - _activeTetraminoPos.x, _rotation];
            }
            // TODO: Временно, для отображения следующей фигуры
            if ((y >= _incomingTetraminoPos.y & y < _incomingTetraminoPos.y + 4 & x >= _incomingTetraminoPos.x & x < _incomingTetraminoPos.x + 4))
            {
                isCheckNextTetraminoBlocks = _incomingTetramino[y - _incomingTetraminoPos.y, x - _incomingTetraminoPos.x, Rotation.Angle0];
            }
            return _glassful[y, x] || isCheckActiveTetraminoBlocks || isCheckNextTetraminoBlocks;
        }

        bool IsRotatable(Tetramino activeTetramino)
        {
            TetraminoRotate();
            bool result = true;
            Vector2Int Pos = _activeTetraminoPos;
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
            TetraminoRotate();
            TetraminoRotate();
            TetraminoRotate();
            return result;
        }

        bool IsMoveable(Tetramino activeTetramino, Vector2Int direct)
        {
            bool result = true;
            Vector2Int Pos = _activeTetraminoPos;
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
                TetraminoMove(_down);
            }
            else
            {
                TetraminoInsert(_activeTetramino);
                CheckFillingLineAndErase();
                NewTetraminoCreate();
                onInsert?.Invoke(this, EventArgs.Empty);
            }
        }

        void Tick2(Vector2Int direct)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (IsMoveable(_activeTetramino, direct))
                {
                    TetraminoMove(direct);
                }
            }

        }

        IEnumerator MoveWithDelay(Vector2Int direct)
        {
            while (true)
            {
                if (IsMoveable(_activeTetramino, direct))
                {
                    TetraminoMove(direct);
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        void Update()
        {
            _elapsedTime += Time.deltaTime;
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
                StartCoroutine(MoveWithDelay(_right));
            }

            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                StopAllCoroutines();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                StartCoroutine(MoveWithDelay(_left));
            }

            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                StopAllCoroutines();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (IsRotatable(_activeTetramino))
                {
                    TetraminoRotate();
                    onRotate?.Invoke(this, EventArgs.Empty);
                }
            }

            if (_elapsedTime >= _tickTime)
            {
                _elapsedTime -= _tickTime;
                Tick();
            }
        }
    }
}
