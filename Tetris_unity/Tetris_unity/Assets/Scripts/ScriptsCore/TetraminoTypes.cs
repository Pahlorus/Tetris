using System;
using UnityEngine;

namespace GameCore
{
    [Flags]
    public enum Rotation
    {
        Angle0 = 0,
        Angle90 = 1,
        Angle180 = 2,
        Angle270 = 3,
    }

    class TetraminoTypes
    {
        private Tile[,] _otetramino;
        private Tile[,] _itetramino;
        private Tile[,] _stetramino;
        private Tile[,] _ztetramino;
        private Tile[,] _ltetramino;
        private Tile[,] _jtetramino;
        private Tile[,] _ttetramino;
        private Vector2Int[] _otettraminoShiftVectors;
        private Vector2Int[] _itettraminoShiftVectors;
        private Vector2Int[] _stettraminoShiftVectors;
        private Vector2Int[] _ztettraminoShiftVectors;
        private Vector2Int[] _ltettraminoShiftVectors;
        private Vector2Int[] _jtettraminoShiftVectors;
        private Vector2Int[] _ttettraminoShiftVectors;

        internal TetraminoTypes()
        {
            TetraminoTypesArray = new Tile[7][,];
            TetraminoShiftVectorsArray = new Vector2Int[7][];

            #region Otetramino
            _otetramino = new Tile[4, 4];
            _otetramino[1, 1].State = true;
            _otetramino[1, 2].State = true;
            _otetramino[2, 1].State = true;
            _otetramino[2, 2].State = true;
            _otettraminoShiftVectors = new Vector2Int[4]
            {
                new Vector2Int(0, 1),
                new Vector2Int(-1, 0),
                new Vector2Int(0, -1),
                new Vector2Int(1, 0)
            };
            #endregion
            #region Itetramino
            _itetramino = new Tile[4, 4];
            _itetramino[1, 0].State = true;
            _itetramino[1, 1].State = true;
            _itetramino[1, 2].State = true;
            _itetramino[1, 3].State = true;
            _itettraminoShiftVectors = new Vector2Int[4]
            {
                new Vector2Int(-1, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(1, -2),
                new Vector2Int(1, 2)
            };
            #endregion
            #region Stetramino
            _stetramino = new Tile[4, 4];
            _stetramino[1, 2].State = true;
            _stetramino[1, 3].State = true;
            _stetramino[2, 1].State = true;
            _stetramino[2, 2].State = true;
            _stettraminoShiftVectors = new Vector2Int[4]
            {
                new Vector2Int(-1, 1),
                new Vector2Int(0, -1),
                new Vector2Int(0, 0),
                new Vector2Int(1, 0)
            };
            #endregion
            #region Ztetramino
            _ztetramino = new Tile[4, 4];
            _ztetramino[1, 1].State = true;
            _ztetramino[1, 2].State = true;
            _ztetramino[2, 2].State = true;
            _ztetramino[2, 3].State = true;
            _ztettraminoShiftVectors = new Vector2Int[4]
            {
                new Vector2Int(-1, 1),
                new Vector2Int(0, -1),
                new Vector2Int(0, 0),
                new Vector2Int(1, 0)
            };
            #endregion
            #region Ltetramino
            _ltetramino = new Tile[4, 4];
            _ltetramino[1, 1].State = true;
            _ltetramino[1, 2].State = true;
            _ltetramino[1, 3].State = true;
            _ltetramino[2, 1].State = true;
            _ltettraminoShiftVectors = new Vector2Int[4]
            {
                new Vector2Int(-1, 1),
                new Vector2Int(-1, -1),
                new Vector2Int(1, -1),
                new Vector2Int(1, 1)
            };
            #endregion
            #region Jtetramino
            _jtetramino = new Tile[4, 4];
            _jtetramino[1, 1].State = true;
            _jtetramino[1, 2].State = true;
            _jtetramino[1, 3].State = true;
            _jtetramino[2, 3].State = true;
            _jtettraminoShiftVectors = new Vector2Int[4]
            {
                new Vector2Int(-1, 1),
                new Vector2Int(-1, -1),
                new Vector2Int(1, -1),
                new Vector2Int(1, 1)
            };
            #endregion
            #region Ttetramino
            _ttetramino = new Tile[4, 4];
            _ttetramino[1, 1].State = true;
            _ttetramino[1, 2].State = true;
            _ttetramino[1, 3].State = true;
            _ttetramino[2, 2].State = true;
            _ttettraminoShiftVectors = new Vector2Int[4]
            {
                new Vector2Int(-1, 1),
                new Vector2Int(-1, -1),
                new Vector2Int(1, -1),
                new Vector2Int(1, 1)
            };
            #endregion

            TetraminoTypesArray[0] = _otetramino;
            TetraminoTypesArray[1] = _itetramino;
            TetraminoTypesArray[2] = _stetramino;
            TetraminoTypesArray[3] = _ztetramino;
            TetraminoTypesArray[4] = _ltetramino;
            TetraminoTypesArray[5] = _jtetramino;
            TetraminoTypesArray[6] = _ttetramino;

            TetraminoShiftVectorsArray[0] = _otettraminoShiftVectors;
            TetraminoShiftVectorsArray[1] = _itettraminoShiftVectors;
            TetraminoShiftVectorsArray[2] = _stettraminoShiftVectors;
            TetraminoShiftVectorsArray[3] = _ztettraminoShiftVectors;
            TetraminoShiftVectorsArray[4] = _ltettraminoShiftVectors;
            TetraminoShiftVectorsArray[5] = _jtettraminoShiftVectors;
            TetraminoShiftVectorsArray[6] = _ttettraminoShiftVectors;
        }

        internal Tile[][,] TetraminoTypesArray { get; private set; }

        internal Vector2Int[][] TetraminoShiftVectorsArray { get; private set; }
    }
}

