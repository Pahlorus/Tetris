using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameCore
{
    struct Tile
    {
        private bool _state;
        private int _colorIndex;

        public bool State
        {
            get { return _state; }
            set { _state = value; }
        }
        public int Color
        {
            get { return _colorIndex; }
            set { _colorIndex = value; }
        }
    }
}
