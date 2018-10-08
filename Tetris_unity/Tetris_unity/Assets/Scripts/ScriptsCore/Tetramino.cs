using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    class Tetramino
    {
        private int _size = 4;
        private bool[,] _tetramino;

        public Tetramino(bool[,] tetraminoType, Vector2Int[] tetraminoShiftVectors, Color32 color)
        {
            _tetramino = tetraminoType;
            ShiftVector = tetraminoShiftVectors;
            Color = color;
        }

        public Color32 Color { get; private set; }
        public Vector2Int[] ShiftVector { get; private set; }

        public bool this[int y, int x, Rotation rot]
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
    }
}


