using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    private bool[,] _glassful;
    private int _glassfulHigh = 20;
    private int _glassfulWidth = 10;


    void Awake()
    {
        _glassful = new bool[_glassfulHigh, _glassfulWidth];
    }


    public event EventHandler OnCollision;

    // Use this for initialization

    void TetraminoCreate()
    {
        //Tetramino p = new OTetramino();

    }

    void CheckMoveable()
    {

    }

    void CheckRotatable()
    {

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
		
	}
}
