﻿using System;
using System.Collections;
using UnityEngine;

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
        private int _score;
        private int _lineCount;
        private int _currentLevel;
        private float[] _tickTimesArray = new float[11] {1.0f, 0.9f,  0.8f, 0.7f, 0.6f, 0.5f, 0.4f, 0.3f, 0.2f, 0.1f, 0.06f };
        private int[] _scoreMultiplier = new int[5] { 10, 100, 300, 700, 1500 };
        private float _boostTime = 0.04f;
        private float _tickTime;
        private float _elapsedTime = 0f;

        private Tile[,] _glassful;
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

        public int Score { get { return _score; } }
        public int LineCount { get { return _lineCount; } }
        public int CurrentLevel { get { return _currentLevel; } }

        public Tile[,] GlassFull { get { return _glassful; } }
        public Tetramino ActiveTetramino { get { return _activeTetramino; } }
        public Tetramino IncomingTetramino { get { return _incomingTetramino; } }
        public Vector2Int ActiveTetraminoPos { get { return _activeTetraminoPos; } }
        public Vector2Int IncomingTetraminoPos { get { return _incomingTetraminoPos; } }
        public Rotation Rotation { get { return _rotation; } }

        public EventHandler onInsert;
        public EventHandler onRotate;
        public EventHandler onGameOver;

        void Awake()
        {
            _currentLevel = 0;
            _tickTime = _tickTimesArray[_currentLevel];
            _down.y = _step;
            _left.x = -_step;
            _right.x = _step;
            _startPosition.y = 4 * _step;
            _incomingTetraminoPos.y = _initialPosY;
            _incomingTetraminoPos.x = _initialPosX;
            _tetraminoTypes = new TetraminoTypes();
            _colorsArray = new Color32[_tetraminoTypes.TetraminoTypesArray.Length];
            _glassful = new Tile[_glassfulHigh, _glassfulWidth];
        }

        public void GameStart()
        {
            int tetraminoIndex = TetraminoIndexGenerate();
            _incomingTetramino = new Tetramino(_tetraminoTypes.TetraminoTypesArray[tetraminoIndex], _tetraminoTypes.TetraminoShiftVectorsArray[tetraminoIndex], tetraminoIndex);
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

        private int TetraminoIndexGenerate()
        {
            return UnityEngine.Random.Range(0, _tetraminoTypes.TetraminoTypesArray.Length);
        }

        private void NewTetraminoCreate()
        {
            _activeTetramino = _incomingTetramino;
            _activeTetraminoPos = _incomingTetraminoPos;
            _rotation = Rotation.Angle0;

            if (!TryTetraminoMove(_startPosition))
            {
                GameStop();
                onGameOver?.Invoke(this, EventArgs.Empty);
            }
            int tetraminoIndex = TetraminoIndexGenerate();
            _incomingTetramino = new Tetramino(_tetraminoTypes.TetraminoTypesArray[tetraminoIndex], _tetraminoTypes.TetraminoShiftVectorsArray[tetraminoIndex], tetraminoIndex);
        }

        private void TetraminoInsert(Tetramino activeTetramino)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (activeTetramino[j, i, _rotation].State)
                    {
                        _glassful[_activeTetraminoPos.y + j, _activeTetraminoPos.x + i] = activeTetramino[j, i, _rotation];

                    }
                }
            }
        }

        private bool IsCellHaveBlock(int y, int x)
        {
            bool isCheckActiveTetraminoBlocks = false;
            bool isCheckNextTetraminoBlocks = false;
            if ((y >= _activeTetraminoPos.y & y < _activeTetraminoPos.y + 4 & x >= _activeTetraminoPos.x & x < _activeTetraminoPos.x + 4))
            {
                isCheckActiveTetraminoBlocks = _activeTetramino[y - _activeTetraminoPos.y, x - _activeTetraminoPos.x, _rotation].State;
            }
            if ((y >= _incomingTetraminoPos.y & y < _incomingTetraminoPos.y + 4 & x >= _incomingTetraminoPos.x & x < _incomingTetraminoPos.x + 4))
            {
                isCheckNextTetraminoBlocks = _incomingTetramino[y - _incomingTetraminoPos.y, x - _incomingTetraminoPos.x, Rotation.Angle0].State;
            }
            return _glassful[y, x].State || isCheckActiveTetraminoBlocks || isCheckNextTetraminoBlocks;
        }

        private bool TryTetraminoRotate(Tetramino activeTetramino)
        {
            bool result = true;
            Rotation checkingRotation = (Rotation)(((int)_rotation + 1) % 4);
            int chekingNumberRotate = (int)checkingRotation;
            Vector2Int checkingShiftPos = _activeTetraminoPos + activeTetramino.ShiftVector[chekingNumberRotate]; ;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (!activeTetramino[j, i, checkingRotation].State)
                    {
                        continue;
                    }
                    if ((j + checkingShiftPos.y >= _glassfulHigh || i + checkingShiftPos.x >= _glassfulWidth || i + checkingShiftPos.x < 0 || _glassful[j + checkingShiftPos.y, i + checkingShiftPos.x].State))
                    {
                        result = false;
                    }
                }
            }
            if (result)
            {
                _rotation = checkingRotation;
                _activeTetraminoPos = checkingShiftPos;
            }
            return result;
        }

        private bool TryTetraminoMove(Vector2Int direct)
        {
            bool result = true;
            Vector2Int checkingNewPos = _activeTetraminoPos + direct;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (!_activeTetramino[j, i, _rotation].State)
                    {
                        continue;
                    }
                    if ((j + checkingNewPos.y >= _glassfulHigh || i + checkingNewPos.x >= _glassfulWidth || i + checkingNewPos.x < 0 || _glassful[j + checkingNewPos.y, i + checkingNewPos.x].State))
                    {
                        result = false;
                    }
                }
            }
            if (result)
            {
                _activeTetraminoPos = checkingNewPos;
            }
            return result;
        }

        private void LineEraseAndShiftGlassfull(int line)
        {
            for (int j = line; j >= _startPosition.y; j--)
            {
                for (int i = 0; i < _glassfulWidth; i++)
                {
                    _glassful[j, i] = _glassful[j - 1, i];
                }
            }
        }

        private void CheckFillingLineAndErase()
        {
            int lineCount = 0;
            bool isFillingLine;
            for (int j = 3; j < _glassfulHigh; j++)
            {
                isFillingLine = true;
                for (int i = 0; i < _glassfulWidth; i++)
                {
                    if (!_glassful[j, i].State)
                    {
                        isFillingLine = false;
                        break;
                    }
                }

                if (isFillingLine)
                {
                    LineEraseAndShiftGlassfull(j);
                    lineCount += 1;
                }
            }
            _score += _scoreMultiplier[lineCount];
            _lineCount += lineCount;
        }

        private void TickTimeUp()
        {
            _tickTime = _tickTimesArray[_currentLevel];
        }
        void LevelUp()
        {
            if (_currentLevel<10)
            _currentLevel = _lineCount/10;
        }


        private void Tick()
        {
            if (!TryTetraminoMove(_down))
            {
                TetraminoInsert(_activeTetramino);
                CheckFillingLineAndErase();
                NewTetraminoCreate();
                TickTimeUp();
                LevelUp();
                onInsert?.Invoke(this, EventArgs.Empty);
            }
        }

        private IEnumerator MoveWithDelay(Vector2Int direct)
        {
            while (true)
            {
                TryTetraminoMove(direct);
                yield return new WaitForSeconds(0.1f);
            }
        }

        void Update()
        {
            _elapsedTime += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _tickTime = _boostTime;
                _elapsedTime = _boostTime;
            }

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                _tickTime = _tickTimesArray[_currentLevel];
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
                if (TryTetraminoRotate(_activeTetramino))
                {
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
