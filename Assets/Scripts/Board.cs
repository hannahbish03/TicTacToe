using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Board
    {
        // feilds
        int xPos;
        int yPos;
        int[,] boardPlaces;
        List<GameObject> pieces;
        public int Solved; // 0 for unsolved, 1 for player one win, 2 for player 2 win

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public int this[int x, int y]
        {
            get
            {
                return boardPlaces[x, y];
            }
            set
            {
                boardPlaces[x, y] = value;
            }
        }

        public Board(int x, int y)
        {
            xPos = x;
            yPos = y;
            Solved = 0;
            boardPlaces = new int[3, 3];
            pieces = new List<GameObject>();
            for (int a = 0;a < 3; a++)
            {
                for (int b = 0; b < 3; b++)
                {
                    boardPlaces[a, b] = 0;
                }
            }
        }

        public bool CheckForWin()
        {
            // check columns
            for (int x = 0; x < 3; x ++)
            {
                if (boardPlaces[x,0] == boardPlaces[x,1] && boardPlaces[x, 1]  == boardPlaces[x,2] && boardPlaces[x, 1] != 0)
                {
                    Solved =  boardPlaces[x, 0];
                    return true;
                }
            }

            // check rows
            for (int y = 0; y < 3; y++)
            {
                if (boardPlaces[0, y] == boardPlaces[1, y] && boardPlaces[1, y] == boardPlaces[2, y] && boardPlaces[1, y] != 0)
                {
                    Solved = boardPlaces[0,y];
                    return true;
                }
            }

            // check diagonals
            if (boardPlaces[0, 0] == boardPlaces[1, 1] && boardPlaces[1, 1] == boardPlaces[2, 2] && boardPlaces[1, 1] != 0)
            {
                Solved = boardPlaces[1, 1];
                return true;
            }
            if (boardPlaces[2, 0] == boardPlaces[1, 1] && boardPlaces[1, 1] == boardPlaces[0, 2] && boardPlaces[1, 1] != 0)
            {
                Solved = boardPlaces[1, 1];
                return true;
            }

            return false;
        }

        // add a piece to the board
        public void Add(GameObject piece)
        {
            pieces.Add(piece);
        }

        public List<GameObject> Clear() // sends list of game objects back to be destroyed
        {
            for (int a = 0; a < 3; a++)
            {
                for (int b = 0; b < 3; b++)
                {
                    boardPlaces[a, b] = 0;
                }
            }
            return pieces;
        }
    }
}
