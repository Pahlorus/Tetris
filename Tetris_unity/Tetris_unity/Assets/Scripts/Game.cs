using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore
{
    public class Game : MonoBehaviour
    {

        [SerializeField]
        // Временно, для отображения игрового поля.
        private Text _text;
        [SerializeField]
        Color32[] _colorsArray;

        private int _glassfulHigh = 21;
        private int _glassfulWidth = 10;
        private int _nextTetraminoIndex;
        private float _repeatTime;
        private float _currentTime;
        private bool[,] _glassful;

        Tetramino _activeTetramino;
        Tetramino[] _tetraminoArray;
        TetraminoTypes _tetraminoTypes;
        System.Random _random;


        void Awake()
        {
            enabled = true;
            _tetraminoTypes = new TetraminoTypes();
            _colorsArray = new Color32[_tetraminoTypes.TetraminoTypesArray.Length];
            /*  _tetraminoArray = new Tetramino[_tetraminoTypes.TetraminoTypesArray.Length];
              for (int i =0; i< _tetraminoTypes.TetraminoTypesArray.Length; i++)
              {
                  _tetraminoArray[i] = new Tetramino(_tetraminoTypes.TetraminoTypesArray[i], _tetraminoTypes.TetraminoShiftVectorsArray[i], _colorsArray[i]);
              }*/
            _glassful = new bool[_glassfulHigh, _glassfulWidth];
            NextTetraminoIndexGenerate();
            InsertNewTetramino(_nextTetraminoIndex);
            TestDraw();

        }


        void NextTetraminoIndexGenerate()
        {
            _random = new System.Random();
            _nextTetraminoIndex = _random.Next(_tetraminoTypes.TetraminoTypesArray.Length);
        }
        // Временно.
        void InsertNewTetramino(int tetraminoIndex)
        {
            _activeTetramino = new Tetramino(_tetraminoTypes.TetraminoTypesArray[tetraminoIndex], _tetraminoTypes.TetraminoShiftVectorsArray[tetraminoIndex], _colorsArray[tetraminoIndex]);
            int x = _activeTetramino.Pos.x;
            int y = _activeTetramino.Pos.y;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    _glassful[j + y, i + x] = _activeTetramino.TetraminoExemplar[j, i];
                }
            }
        }


        // Временно.
        void TestDraw()
        {
            string text = string.Empty;
            for (int j = 0; j < _glassfulHigh; j++)
            {
                for (int i = 0; i < _glassfulWidth; i++)
                {
                    text = text + Convert.ToInt32(_glassful[j, i]).ToString() + "  ";
                }
                text = text + "\n";
            }
            _text.text = text;
        }


        void Rotate(Tetramino activeTetramino)
        {
            TestDelete(activeTetramino);
            int numberRotate = activeTetramino.NumberRotate;
            activeTetramino.Rotate();
            Shift(activeTetramino, numberRotate);
            /*Vector2Int Pos;
            Pos = _tetraminoArray[2].Pos;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (_tetraminoArray[2].Tetranino[j, i])
                    {
                        _glassful[j + Pos.y, i + Pos.x] = _tetraminoArray[2].Tetranino[j, i];
                    }
                }
            }*/
            // TestDraw();

        }
        void TestDelete(Tetramino activeTetramino)
        {
            Vector2Int Pos;
            Pos = activeTetramino.Pos;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (activeTetramino.TetraminoExemplar[j, i])
                    {
                        _glassful[j + Pos.y, i + Pos.x] = false;
                    }
                }
            }
        }



        public event EventHandler OnCollision;

        // Use this for initialization

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

        void Shift(Tetramino activeTetramino, int numberRotate)
        {
            TestDelete(activeTetramino);
            Vector2Int Pos = activeTetramino.Pos; ;
            Vector2Int newPos = Pos + activeTetramino.ShiftVector[numberRotate];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (activeTetramino.TetraminoExemplar[j, i])
                    {
                        _glassful[j + newPos.y, i + newPos.x] = activeTetramino.TetraminoExemplar[j, i];
                    }
                }
            }
            activeTetramino.Pos = newPos;
            TestDraw();
        }

        void TetraminoFall(Tetramino activeTetramino)
        {
            TestDelete(activeTetramino);
            Vector2Int Pos = activeTetramino.Pos;
            Pos.y = Pos.y + 1;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (activeTetramino.TetraminoExemplar[j, i])
                    {
                        _glassful[j + Pos.y, i + Pos.x] = activeTetramino.TetraminoExemplar[j, i];
                    }
                }
            }
            activeTetramino.Pos = Pos;
            TestDraw();
        }

        void Move(int numberRotation, Vector2Int[] shiftVector)
        {/*
        TestDelete();
        Vector2Int Pos;
        Vector2Int newPos;
        Pos = _tetraminoArray[2].Pos;
        if (direction == 2)
        {
            newPos = Pos + shiftVector[numberRotation];
        }
        if (direction == -2)
        {
            Pos.y = Pos.y - 1;
        }
        if (direction == -1)
        {
            Pos.x = Pos.x - 1;
        }
        if (direction == 1)
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
        TestDraw();*/
        }



        void TetraminoDockAndDestroy()
        {
            //Tetramino p = new OTetramino();

        }

        void Start()
        {
            _currentTime = _repeatTime * 1000f;

        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                //Move(2);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                //Move(1);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                //Move(-1);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Rotate(_activeTetramino);
            }

            _currentTime -= Time.deltaTime;
            if (_currentTime <= 0)
            {
                TetraminoFall(_activeTetramino);
                _currentTime = _repeatTime * 1000f;



            }
        }

    }
}
