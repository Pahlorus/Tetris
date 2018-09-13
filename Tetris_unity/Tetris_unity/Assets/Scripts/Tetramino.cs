using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Tetramino
{
    protected bool[,] _tetramino;
    protected Color32 _color;
    protected Vector2Int _tetraminoPos;

    protected Tetramino()
    {
        _tetramino = new bool[4, 4];
        _tetraminoPos.x = 0;
        _tetraminoPos.y = 0;
    }

    protected  void Rotate()
    {

    }

    protected void Move()
    {

    }

}

class OTetramino : Tetramino
{
    OTetramino()
    { 
        _tetramino[1, 1] = true;
        _tetramino[1, 2] = true;
        _tetramino[2, 1] = true;
        _tetramino[2, 2] = true;
    }

}


