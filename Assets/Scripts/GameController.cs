using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameController : MonoBehaviour
    {
        // player pieces, and focus prefab
        public GameObject gamePieceOne;
        public GameObject gamePieceTwo;
        public GameObject gameFocus;
        public Text playerOne;
        public Text playerTwo;
        public Text winText;

        private GameObject focus; // holds focus so I can change its transform
        private bool playerOneActive; // true = player ones turn, false = player twos turn
        private bool allActivce; // allows player to make a move in any sub-board, true when the board the player is sent to is already full
        private Vector2 activeBoard; // a vector to hold the coordinates of the currently active board
        private Board[,] boards; // array to hold board objects
        private List<GameObject> pieces;

        // Use this for initialization
        void Start()
        {
            Screen.SetResolution(1280, 960, false);
            boards = new Board[3, 3]; // board that holds each of the sub-boards - 0 ==  empty, 1 == player one, 2 ==  player two
            pieces = new List<GameObject>();
            
            // create the sub boards
            for (int x = 0; x <= 6; x += 3)
            {
                for (int y = 0; y <= 6; y += 3)
                {
                    boards[x / 3, y / 3] = new Board(x, y);
                }
            }
            
            Cursor.visible = true; // make cursor visable
            winText.text = ""; // hide the win text
            
            playerOneActive = true; // make it player one's turn

            // set the middle board as active
            allActivce = false;
            activeBoard = new Vector2(3, 3);

            // set the game focus location (the active area that the player can use)
            focus = Instantiate(gameFocus, new Vector3(activeBoard.x + 1, activeBoard.y + 1, 0), Quaternion.identity);
        }

        // Update is called once per frame
        void Update()
        {
            
            if (Input.GetMouseButtonDown(0)) // check to see if the user has clicked
            {
                
                Vector3 mouseLocation = Input.mousePosition; // finds mouse position at click
                Vector3 worldPos = this.GetComponent<Camera>().ScreenToWorldPoint(mouseLocation); // compares mouse position on screen to scene position on screen

                // rounds the position variables to align with the game grid
                int xPos = (int)Mathf.Round(worldPos.x);
                int yPos = (int)Mathf.Round(worldPos.y);

                if (boards[xPos / 3, yPos / 3].Solved == 0 &&
                    (allActivce || ((xPos >= activeBoard.x) && (xPos < activeBoard.x + 3) && (yPos >= activeBoard.y) && (yPos < activeBoard.y + 3))) 
                    && boards[xPos / 3, yPos / 3][xPos % 3, yPos % 3] == 0)
                {
                    // instantiates the game piece, changes the active player, and add the move to the board array
                    if (playerOneActive)
                    {
                        GameObject move = Instantiate(gamePieceOne, new Vector3((float)xPos, (float)yPos, 0), Quaternion.identity);
                        boards[xPos / 3, yPos / 3].Add(move);
                        playerOneActive = false;
                        boards[xPos / 3, yPos / 3][xPos % 3, yPos % 3] = 1;
                        if (boards[xPos / 3, yPos / 3].CheckForWin())
                        {
                            GameObject winner = Instantiate(gamePieceOne, new Vector3((xPos / 3) * 3 + 1, (yPos / 3) * 3 + 1, -1), Quaternion.identity);
                            winner.transform.localScale *= 3;
                            pieces.Add(winner);
                            foreach (GameObject piece in boards[xPos / 3, yPos / 3].Clear())
                            {
                                Debug.Log("destroy");
                                Destroy(piece);
                            }
                            if (CheckForWin())
                            {
                                winText.text = "Player One Wins!";
                                focus.transform.position = new Vector3(4, -6, -1.25f);
                                focus.transform.localScale = new Vector3(.77f, 2, 1);
                            }
                            else if (CheckForTie())
                            {
                                winText.text = "It's a Tie!";
                                focus.transform.position = new Vector3(4, -6, -1.25f);
                                focus.transform.localScale = new Vector3(.77f, 2, 1);
                            }
                        }
                    }
                    else
                    {
                        GameObject move = Instantiate(gamePieceTwo, new Vector3((float)xPos, (float)yPos, 0), Quaternion.identity);
                        boards[xPos / 3, yPos / 3].Add(move);
                        playerOneActive = true;
                        boards[xPos / 3, yPos / 3][xPos % 3, yPos % 3] = 2;
                        if (boards[xPos / 3, yPos / 3].CheckForWin())
                        {
                            GameObject winner = Instantiate(gamePieceTwo, new Vector3((xPos / 3) * 3 + 1, (yPos / 3) * 3 + 1, -1), Quaternion.identity);
                            winner.transform.localScale *= 3;
                            pieces.Add(winner);
                            Debug.Log("added");
                            foreach (GameObject piece in boards[xPos / 3, yPos / 3].Clear())
                            {
                                Debug.Log("destroy");
                                Destroy(piece);
                            }
                            if(CheckForWin())
                            {
                                winText.text = "Player Two Wins!";
                                focus.transform.position = new Vector3(4, -6, -1.25f);
                                focus.transform.localScale = new Vector3(.77f, 2, 1);
                            }
                            else if (CheckForTie())
                            {
                                winText.text = "It's a Tie!";
                                focus.transform.position = new Vector3(4, -6, -1.25f);
                                focus.transform.localScale = new Vector3(.77f, 2, 1);
                            }
                        }
                    }

                    activeBoard = new Vector2((xPos % 3) * 3, (yPos % 3) * 3);

                    // set game focus and active area
                    if (!CheckForTie() && !CheckForWin())
                    {
                        if (boards[(int)(activeBoard.x / 3), (int)(activeBoard.y / 3)].Solved == 0)
                        {
                            focus.transform.position = new Vector3((xPos % 3) * 3 + 1, (yPos % 3) * 3 + 1, 0);
                            focus.transform.localScale = new Vector3(.77f, .77f, 1);
                            allActivce = false;
                        }
                        else
                        {
                            focus.transform.position = new Vector3(4, 4, 0);
                            focus.transform.localScale = new Vector3(2.4f, 2.4f, 1);
                            allActivce = true;
                        }
                    }
                    // TO IMPLEMENT: check for win state

                    // change active player marker
                    if (playerOneActive)
                    {
                        playerOne.text = "__";
                        playerTwo.text = "";
                    }
                    else
                    {
                        playerOne.text = "";
                        playerTwo.text = "__";
                    }
                }
            }
        }
        
        public bool CheckForWin()
        {
            // check columns
            for (int x = 0; x < 3; x++)
            {
                if (boards[x, 0].Solved == boards[x, 1].Solved && boards[x, 1].Solved == boards[x, 2].Solved && boards[x, 1].Solved != 0)
                {
                    return true;
                }
            }

            // check rows
            for (int y = 0; y < 3; y++)
            {
                if (boards[0, y].Solved == boards[1, y].Solved && boards[1, y].Solved == boards[2, y].Solved && boards[1, y].Solved != 0)
                {
                    return true;
                }
            }

            // check diagonals
            if (boards[0, 0].Solved == boards[1, 1].Solved && boards[1, 1].Solved == boards[2, 2].Solved && boards[1, 1].Solved != 0)
            {
                return true;
            }
            if (boards[2, 0].Solved == boards[1, 1].Solved && boards[1, 1].Solved == boards[0, 2].Solved && boards[1, 1].Solved != 0)
            {
                return true;
            }

            return false;
        }

        public bool CheckForTie()
        {
            foreach (Board b in boards)
            {
                if (b.Solved == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
