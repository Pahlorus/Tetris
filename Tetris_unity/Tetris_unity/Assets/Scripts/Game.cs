using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
  
    //private GameObject _text;
    [SerializeField]
    private Text _text;
    private bool[,] _glassful;
    Tetramino[] _tetraminoArray;
    private int _glassfulHigh = 20;
    private int _glassfulWidth = 10;
   

    void Awake()
    {
        enabled = true;

        _glassful = new bool[_glassfulHigh, _glassfulWidth];
        TetraminosCreate();
        InsertTetramino();
        TestDraw();

    }

    // Временно.

    void InsertTetramino()
    {
        int x = _tetraminoArray[2].Pos.x;
        int y = _tetraminoArray[2].Pos.y;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                _glassful[j + y, i + x] = _tetraminoArray[2].Tetranino[j, i];
            }

        }
    }



    void TestDraw()
    {
        string text =string.Empty ;
        for (int j = 0; j < _glassfulHigh;  j++)
        {
            for (int i = 0; i < _glassfulWidth; i++)
            {
                text = text + Convert.ToInt32(_glassful[j, i]).ToString();
            }
            text = text +"\n";
        }
        _text.text = text;
    }

    void TestDelete()
    {
        Vector2Int Pos;
        Pos = _tetraminoArray[2].Pos;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (_tetraminoArray[2].Tetranino[j, i])
                {
                    _glassful[j + Pos.y, i + Pos.x] = false;
                }
            }
       }
    }



    public event EventHandler OnCollision;

    // Use this for initialization

    void TetraminosCreate()
    {
        _tetraminoArray = new Tetramino[7];
        _tetraminoArray[0] = new OTetramino();
        _tetraminoArray[1] = new ITetramino();
        _tetraminoArray[2] = new STetramino();
        _tetraminoArray[3] = new ZTetramino();
        _tetraminoArray[4] = new LTetramino();
        _tetraminoArray[5] = new JTetramino();
        _tetraminoArray[6] = new TTetramino();
    }

    void CheckMoveable()
    {

    }

    void CheckRotatable()
    {

    }

    void CheckCollision()
    {

    }

    void CheckFillingLine()
    {

    }

    void Move(int direction)
    {
        TestDelete();
        Vector2Int Pos;
        Pos = _tetraminoArray[2].Pos;
        if (direction == 0)
        {
            Pos.y = Pos.y + 1;
        }
        if (direction == -1)
        {
            Pos.x = Pos.x - 1;
        }
        if (direction == +1)
        {
            Pos.x = Pos.x + 1;
        }

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (_tetraminoArray[2].Tetranino[j, i])
                {
                    _glassful[j + Pos.y, i + Pos.x] = _tetraminoArray[2].Tetranino[j, i];
                }
            }
        }
        _tetraminoArray[2].Pos = Pos;
        TestDraw();
    }
    


    void TetraminoDockAndDestroy()
    {
        //Tetramino p = new OTetramino();

    }

    void Start ()
    {

		
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(+1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(-1);
        }



    }
}
