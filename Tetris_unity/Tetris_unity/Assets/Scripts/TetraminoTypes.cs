using System;
using UnityEngine;



namespace GameCore
{
    [Flags]
    public enum Rotation
    {
        Cw0 = 0,
        Cw90 = 1,
        Cw180 = 2,
        Cw270 = 3,
    }

    class TetraminoTypes
    {
        private bool[,] _otetramino;
        private bool[,] _itetramino;
        private bool[,] _stetramino;
        private bool[,] _ztetramino;
        private bool[,] _ltetramino;
        private bool[,] _jtetramino;
        private bool[,] _ttetramino;
        private Vector2Int[] _otettraminoShiftVectors;
        private Vector2Int[] _itettraminoShiftVectors;
        private Vector2Int[] _stettraminoShiftVectors;
        private Vector2Int[] _ztettraminoShiftVectors;
        private Vector2Int[] _ltettraminoShiftVectors;
        private Vector2Int[] _jtettraminoShiftVectors;
        private Vector2Int[] _ttettraminoShiftVectors;
        private bool[][,] _tetraminoTypesArray;
        private Vector2Int[][] _tetraminoShiftVectorsArray;

        public TetraminoTypes()
        {
            _tetraminoTypesArray = new bool[7][,];
            _tetraminoShiftVectorsArray = new Vector2Int[7][];

            #region Otetramino
            _otetramino = new bool[4, 4];
            _otetramino[1, 1] = true;
            _otetramino[1, 2] = true;
            _otetramino[2, 1] = true;
            _otetramino[2, 2] = true;
            _otettraminoShiftVectors = new Vector2Int[4]
            {
                new Vector2Int(0, 1),
                new Vector2Int(-1, 0),
                new Vector2Int(0, -1),
                new Vector2Int(1, 0)
            };
            #endregion
            #region Itetramino
            _itetramino = new bool[4, 4];
            _itetramino[1, 0] = true;
            _itetramino[1, 1] = true;
            _itetramino[1, 2] = true;
            _itetramino[1, 3] = true;
            _itettraminoShiftVectors = new Vector2Int[4]
            {
                new Vector2Int(-1, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(1, -2),
                new Vector2Int(1, 2)
            };
            #endregion
            #region Stetramino
            _stetramino = new bool[4, 4];
            _stetramino[1, 2] = true;
            _stetramino[1, 3] = true;
            _stetramino[2, 1] = true;
            _stetramino[2, 2] = true;
            _stettraminoShiftVectors = new Vector2Int[4]
            {
                new Vector2Int(-1, 1),
                new Vector2Int(0, -1),
                new Vector2Int(0, 0),
                new Vector2Int(1, 0)
            };
            #endregion
            #region Ztetramino
            _ztetramino = new bool[4, 4];
            _ztetramino[1, 1] = true;
            _ztetramino[1, 2] = true;
            _ztetramino[2, 2] = true;
            _ztetramino[2, 3] = true;
            _ztettraminoShiftVectors = new Vector2Int[4]
            {
                new Vector2Int(-1, 1),
                new Vector2Int(0, -1),
                new Vector2Int(0, 0),
                new Vector2Int(1, 0)
            };
            #endregion
            #region Ltetramino
            _ltetramino = new bool[4, 4];
            _ltetramino[1, 1] = true;
            _ltetramino[1, 2] = true;
            _ltetramino[1, 3] = true;
            _ltetramino[2, 1] = true;
            _ltettraminoShiftVectors = new Vector2Int[4]
            {
                new Vector2Int(-1, 1),
                new Vector2Int(-1, -1),
                new Vector2Int(1, -1),
                new Vector2Int(1, 1)
            };
            #endregion
            #region Jtetramino
            _jtetramino = new bool[4, 4];
            _jtetramino[1, 1] = true;
            _jtetramino[1, 2] = true;
            _jtetramino[1, 3] = true;
            _jtetramino[2, 3] = true;
            _jtettraminoShiftVectors = new Vector2Int[4]
            {
                new Vector2Int(-1, 1),
                new Vector2Int(-1, -1),
                new Vector2Int(1, -1),
                new Vector2Int(1, 1)
            };
            #endregion
            #region Ttetramino
            _ttetramino = new bool[4, 4];
            _ttetramino[1, 1] = true;
            _ttetramino[1, 2] = true;
            _ttetramino[1, 3] = true;
            _ttetramino[2, 2] = true;
            _ttettraminoShiftVectors = new Vector2Int[4]
            {
                new Vector2Int(-1, 1),
                new Vector2Int(-1, -1),
                new Vector2Int(1, -1),
                new Vector2Int(1, 1)
            };
            #endregion

            _tetraminoTypesArray[0] = _otetramino;
            _tetraminoTypesArray[1] = _itetramino;
            _tetraminoTypesArray[2] = _stetramino;
            _tetraminoTypesArray[3] = _ztetramino;
            _tetraminoTypesArray[4] = _ltetramino;
            _tetraminoTypesArray[5] = _jtetramino;
            _tetraminoTypesArray[6] = _ttetramino;

            _tetraminoShiftVectorsArray[0] = _otettraminoShiftVectors;
            _tetraminoShiftVectorsArray[1] = _itettraminoShiftVectors;
            _tetraminoShiftVectorsArray[2] = _stettraminoShiftVectors;
            _tetraminoShiftVectorsArray[3] = _ztettraminoShiftVectors;
            _tetraminoShiftVectorsArray[4] = _ltettraminoShiftVectors;
            _tetraminoShiftVectorsArray[5] = _jtettraminoShiftVectors;
            _tetraminoShiftVectorsArray[6] = _ttettraminoShiftVectors;
        }

        public bool[][,] TetraminoTypesArray
        {
            get { return _tetraminoTypesArray; }
        }

        public Vector2Int[][] TetraminoShiftVectorsArray
        {
            get { return _tetraminoShiftVectorsArray; }
        }
    }
}

