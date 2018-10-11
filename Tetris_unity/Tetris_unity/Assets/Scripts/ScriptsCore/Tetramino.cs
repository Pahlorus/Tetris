using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    class Tetramino
    {
        private int _size = 4;
        private Tile[,] _tetramino;

        internal Tetramino(Tile[,] tetraminoType, Vector2Int[] tetraminoShiftVectors, int colorIndex)
        {
            _tetramino = tetraminoType;
            ShiftVector = tetraminoShiftVectors;
            Color = colorIndex;
            ColorSet(colorIndex);
        }
        internal int Color { get; private set; }
        internal Vector2Int[] ShiftVector { get; private set; }

        internal Tile this[int y, int x, Rotation rot]
        {
            get
            {
                var yy = y;
                var xx = x;
                
                if (rot.HasFlag(Rotation.Angle90))
                {
                    yy = -x;
                    xx = y;
                }
                if (rot.HasFlag(Rotation.Angle180))
                {
                    yy = -yy;
                    xx = -xx;
                }
                return _tetramino[(yy + _size) % _size, (xx + _size) % _size];
            }
        }

        private void ColorSet(int colorIndex)
        {
            for (int j = 0; j < _size; j++)
            {
                for (int i = 0; i < _size; i++)
                {
                    if (_tetramino[j, i].State)
                        _tetramino[j, i].Color = colorIndex;
                }
            }

        }

    }
}


