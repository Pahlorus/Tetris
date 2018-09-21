using System.Collections;
using System.Collections.Generic;
using UnityEngine;


abstract class Tetramino
{
    protected bool[,] _tetramino;
    protected bool[][,] _tetraminoType;
    protected Color32 _color;
    protected Vector2Int _tetraminoPos;

    protected Tetramino()
    {
        _tetramino = new bool[4, 4];
        _tetraminoType = new bool[7][,];
        _tetraminoType[0] = new bool[4, 4];
        _tetraminoType[1] = new bool[4, 4];
        _tetraminoType[2] = new bool[4, 4];
        _tetraminoType[3] = new bool[4, 4];
        _tetraminoType[4] = new bool[4, 4];
        _tetraminoType[5] = new bool[4, 4];
        _tetraminoType[6] = new bool[4, 4];
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
   public OTetramino()
    { 
        _tetramino[1, 1] = true;
        _tetramino[1, 2] = true;
        _tetramino[2, 1] = true;
        _tetramino[2, 2] = true;
    }

}

class ITetramino : Tetramino
{
    public ITetramino()
    {
        _tetramino[1, 0] = true;
        _tetramino[1, 1] = true;
        _tetramino[1, 2] = true;
        _tetramino[1, 3] = true;
    }

}


