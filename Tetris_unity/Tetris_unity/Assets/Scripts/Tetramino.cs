using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    class Tetramino
    {
        private bool[,] _tetramino;
        private Vector2Int[] _shiftVectors;
        private Color32 _tetraminoColor;
        private Vector2Int _tetraminoPos;
        private Vector2Int _tetraimnoInitialPos;
        private int _initialPosX = 3;
        private int _initialPosY = 0;

        private int _numberRotate;

        public Tetramino(bool[,] tetraminoType, Vector2Int[] tetraminoShiftVectors, Color32 color)
        {
            _tetramino = tetraminoType;
            _shiftVectors = tetraminoShiftVectors;
            _tetraminoColor = color;
            _tetraminoPos.y = _initialPosY;
            _tetraminoPos.x = _initialPosX;
        }

        public bool[,] TetraminoExemplar
        {
            get { return _tetramino; }
        }

        public Color32 Color
        {
            get { return _tetraminoColor; }
        }

        public Vector2Int Pos
        {
            get { return _tetraminoPos; }

        }

        public Vector2Int[] ShiftVector
        {
            get { return _shiftVectors; }
        }

        public int NumberRotate
        {
            get { return _numberRotate; }
        }

        public void Rotate()
        {
            bool[,] _newTetramino = new bool[4, 4];
            for (int i = 3; i >= 0; --i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    _newTetramino[j, 3 - i] = _tetramino[i, j];
                }
            }
            _tetramino = _newTetramino;
            if (_numberRotate < 3)
            {
                _numberRotate = _numberRotate + 1;
            }
            else
            {
                _numberRotate = 0;
            }
        }

        public void Move(Vector2Int direct)
        {
            _tetraminoPos = _tetraminoPos + direct;
        }
    }
}


