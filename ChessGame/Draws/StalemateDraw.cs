using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ChessGame.Pieces;

namespace ChessGame.Draws
{
    internal class StalemateDraw : IDraw
    {
        public bool IsDraw(Piece[,] board, Game currGame)
        {
            string playerTurn;
            Piece[,] newBoard;
            playerTurn = "black";

            int[] KingsPlace;
            KingsPlace = currGame.getKingPlace(playerTurn);
            if (currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1],board)) //if the kings is in check its not a draw
            {
                //check for white
                playerTurn = "white";
                KingsPlace = currGame.getKingPlace(playerTurn);
                if (currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], board)) //if the kings is in check its not a draw
                    return false;
            }
            playerTurn = "black";
            KingsPlace = currGame.getKingPlace(playerTurn);

            bool WhiteCanMove = false;
            bool BlackCanMove = false;

            //for every player's piece - check if the piece have valid move. if there is - return true
            for (int i = 0;i<7;i++) 
                for (int j = 0;j < 7; j++)
                {
                    if (board[i, j] != null && board[i, j].getColor() == playerTurn)
                    {
                        if (board[i, j] is Pawn)
                        {
                                if (i != 7 && board[i + 1, j] == null) //move straight , and then check if the king is in check. if not, its a valid move 
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i + 1, j] = newBoard[i, j];
                                    newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        BlackCanMove = true;
                                }
                                if (i != 7 && j != 0 && board[i + 1, j - 1] != null && board[i + 1, j - 1].getColor() != playerTurn) //capture down left
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i + 1, j - 1] = newBoard[i, j];
                                    newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    BlackCanMove = true;
                            }
                                if (i != 7 && j != 7 && board[i + 1, j + 1] != null && board[i + 1, j + 1].getColor() != playerTurn) //capture down right
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i + 1, j + 1] = newBoard[i, j];
                                    newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    BlackCanMove = true;
                            }
                                if (i == 5 && board[i + 2, j] != null && board[i + 2, j].getColor() != playerTurn && ((Pawn)board[i, j]).GetMoveNumber() == 0) //move 2 tiles up
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i + 2, j] = newBoard[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    BlackCanMove = true;
                            }
                            
                        }
                        if (board[i, j] is Rook || board[i, j] is Queen)
                        {
                            for (int k = j + 1; k < 7; k++) // try moving right
                            {
                                if (board[i, k] != null && board[i, k].getColor() == playerTurn) break;
                                if (board[i, k] == null || (board[i, k] != null && board[i, k].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i, k] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1],newBoard))
                                        BlackCanMove = true;
                                }
                            }
                            for (int k = j - 1; k > 0; k--) // try moving left
                            {
                                if (board[i, k] != null && board[i, k].getColor() == playerTurn) break;
                                if (board[i, k] == null || (board[i, k] != null && board[i, k].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i, k] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        BlackCanMove = true;
                                }
                            }
                            for (int k = i - 1; k > 0; k--) // try moving up
                            {
                                if (board[k, j] != null && board[k, j].getColor() == playerTurn) break;
                                if (board[k, j] == null || (board[k, j] != null && board[i, k].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[k, j] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        BlackCanMove = true;
                                }
                            }
                            for (int k = i + 1; k < 7; k++) // try moving down
                            {
                                if (board[k, j] != null && board[k, j].getColor() == playerTurn) break;
                                if (board[k, j] == null || (board[k, j] != null && board[k, j].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[k, j] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        BlackCanMove = true;
                                }
                            }
                        }
                        if (board[i, j] is Bishop || board[i,j] is Queen)
                        {
                            for (int n = i+1,m=j+1; n < 7 && m < 7; n++,m++) //try move diagonly down right
                            {
                                if (board[n, m] != null && board[n, m].getColor() == playerTurn) break;
                                if (board[n, m] == null || (board[n, m] != null && board[n, m].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[n, m] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        BlackCanMove = true;
                                }
                            }
                            for (int n = i + 1, m = j - 1; n < 7 && m > 0; n++, m--) //try move diagonly down left
                            {
                                if (board[n, m] != null && board[n, m].getColor() == playerTurn) break;
                                if (board[n, m] == null || (board[n, m] != null && board[n, m].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[n, m] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        BlackCanMove = true;
                                }
                            }
                            for (int n = i - 1, m = j + 1; n > 0 && m < 7; n--, m++) //try move diagonly up right
                            {
                                if (board[n, m] != null && board[n, m].getColor() == playerTurn) break;
                                if (board[n, m] == null || (board[n, m] != null && board[n, m].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[n, m] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        BlackCanMove = true;
                                }
                            }
                            for (int n = i - 1, m = j - 1; n > 0 && m > 0; n--, m--) //try move diagonly up left
                            {
                                if (board[n, m] != null && board[n, m].getColor() == playerTurn) break;
                                if (board[n, m] == null || (board[n, m] != null && board[n, m].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[n, m] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        BlackCanMove = true;
                                }
                            }
                            }
                        if (board[i,j] is Knight)
                        {
                            if ((i >= 2 && j < 7) && (board[i - 2, j + 1] == null || (board[i - 2, j + 1].getColor() != playerTurn)))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 2, j + 1] = board[i , j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, i, j, newBoard))
                                    BlackCanMove = true;
                            }
                            if ((i >= 2 && j > 0) && (board[i - 2, j - 1] == null || (board[i - 2, j - 1].getColor() != playerTurn)))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 2, j - 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    BlackCanMove = true;
                            }
                            if ((i >= 1 && j < 6) && (board[i - 1, j + 2] == null || (board[i - 1, j + 2].getColor() != playerTurn))) 
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 1, j +2] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    BlackCanMove = true;
                            }
                            if ((i >= 1 && j > 1) && (board[i - 1, j - 2] == null || (board[i - 1, j - 2].getColor() != playerTurn)))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 1, j - 2] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    BlackCanMove = true;
                            }
                            if ((i < 7 && j < 6) && (board[i + 1, j + 2] == null || (board[i + 1, j + 2].getColor() != playerTurn)))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 1, j + 2] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    BlackCanMove = true;
                            }
                            if ((i < 7 && j > 1) && (board[i + 1, j - 2] == null || (board[i + 1, j - 2].getColor() != playerTurn)))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 1, j - 2] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    BlackCanMove = true;
                            }
                            if ((i < 6 && j < 7) && (board[i + 2, j + 1] == null || (board[i + 2, j + 1].getColor() != playerTurn)))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 2, j+ 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    BlackCanMove = true;
                            }
                            if ((i < 6 && j > 0) && (board[i + 2, j - 1] == null || (board[i + 2, j - 1].getColor() != playerTurn)))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 2, j - 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    BlackCanMove = true;
                            }
                        }
                        if (board[i,j] is King)
                        {
                            if (i != 0 && j != 0 && (board[i - 1, j - 1] == null || board[i - 1, j - 1].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 1, j - 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, i - 1, j - 1, newBoard))
                                    BlackCanMove = true;
                            } // up left
                            if (i != 0 && (board[i - 1, j] == null || board[i - 1, j].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 1, j ] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, i - 1, j, newBoard))
                                    BlackCanMove = true;
                            } // up left
                            if (i != 0 && j != 7 && (board[i - 1, j + 1] == null || board[i - 1, j + 1].getColor() != playerTurn)) {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 1, j + 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, i - 1, j + 1, newBoard))
                                    BlackCanMove = true;
                            } // up right
                            if (j != 7 && (board[i, j + 1] == null || board[i, j + 1].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i , j + 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, i, j + 1, newBoard))
                                    BlackCanMove = true;
                            } // right
                            if (i != 7 && j != 7 && (board[i + 1, j + 1] == null || board[i + 1, j + 1].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 1, j + 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, i + 1, j + 1, newBoard))
                                    BlackCanMove = true;
                            } // down right
                            if (i != 7 && (board[i + 1, j] == null || board[i + 1, j].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 1, j ] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, i+1, j, newBoard))
                                    BlackCanMove = true;
                            } // down
                            if (i != 7 && j != 0 && (board[i + 1, j - 1] == null || board[i + 1, j - 1].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 1, j - 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, i + 1, j - 1, newBoard))
                                    BlackCanMove = true;
                            } // down left
                            if (j != 0 && (board[i, j - 1] == null || board[i, j - 1].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i , j - 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, i, j - 1, newBoard))
                                    BlackCanMove = true;
                            } // left
                        }
                        }
                    }

            playerTurn = "white";

            KingsPlace = currGame.getKingPlace(playerTurn);


            //for every player's piece - check if the piece have valid move. if there is - return true
            for (int i = 0; i < 7; i++)
                for (int j = 0; j < 7; j++)
                {
                    if (board[i, j] != null && board[i, j].getColor() == playerTurn)
                    {
                        if (board[i, j] is Pawn)
                        {
                            
                                if (i != 0 && board[i - 1, j] == null) //move straight , and then check if the king is in check. if not, its a valid move 
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i - 1, j] = newBoard[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        WhiteCanMove = true;
                                }
                                if (i != 0 && j != 0 && board[i - 1, j - 1] != null && board[i - 1, j - 1].getColor() != playerTurn) //capture up left
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i - 1, j - 1] = newBoard[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        WhiteCanMove = true;
                            }
                                if (i != 0 && j != 7 && board[i - 1, j + 1] != null && board[i - 1, j + 1].getColor() != playerTurn) //capture up right
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i - 1, j + 1] = newBoard[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        WhiteCanMove = true;
                            }
                                if (i == 2 && board[i - 2, j] != null && board[i - 2, j].getColor() != playerTurn && ((Pawn)board[i, j]).GetMoveNumber() == 0) //move 2 tiles up
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i - 2, j] = newBoard[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        WhiteCanMove = true;
                            }
                            
                           
                        }
                        if (board[i, j] is Rook || board[i, j] is Queen)
                        {
                            for (int k = j + 1; k < 7; k++) // try moving right
                            {
                                if (board[i, k] != null && board[i, k].getColor() == playerTurn) break;
                                if (board[i, k] == null || (board[i, k] != null && board[i, k].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i, k] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        WhiteCanMove = true;
                                }
                            }
                            for (int k = j - 1; k > 0; k--) // try moving left
                            {
                                if (board[i, k] != null && board[i, k].getColor() == playerTurn) break;
                                if (board[i, k] == null || (board[i, k] != null && board[i, k].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i, k] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        WhiteCanMove = true;
                                }
                            }
                            for (int k = i - 1; k > 0; k--) // try moving up
                            {
                                if (board[k, j] != null && board[k, j].getColor() == playerTurn) break;
                                if (board[k, j] == null || (board[k, j] != null && board[k, j].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[k, j] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        WhiteCanMove = true;
                                }
                            }
                            for (int k = i + 1; k < 7; k++) // try moving down
                            {
                                if (board[k, j] != null && board[k, j].getColor() == playerTurn) break;
                                if (board[k, j] == null || (board[k, j] != null && board[k, j].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[k, j] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        WhiteCanMove = true;
                                }
                            }
                        }
                        if (board[i, j] is Bishop || board[i, j] is Queen)
                        {
                            for (int n = i + 1, m = j + 1; n < 7 && m < 7; n++, m++) //try move diagonly down right
                            {
                                if (board[n, m] != null && board[n, m].getColor() == playerTurn) break;
                                if (board[n, m] == null || (board[n, m] != null && board[n, m].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[n, m] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        WhiteCanMove = true;
                                }
                            }
                            for (int n = i + 1, m = j - 1; n < 7 && m > 0; n++, m--) //try move diagonly down left
                            {
                                if (board[n, m] != null && board[n, m].getColor() == playerTurn) break;
                                if (board[n, m] == null || (board[n, m] != null && board[n, m].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[n, m] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        WhiteCanMove = true;
                                }
                            }
                            for (int n = i - 1, m = j + 1; n > 0 && m < 7; n--, m++) //try move diagonly up right
                            {
                                if (board[n, m] != null && board[n, m].getColor() == playerTurn) break;
                                if (board[n, m] == null || (board[n, m] != null && board[n, m].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[n, m] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        WhiteCanMove = true;
                                }
                            }
                            for (int n = i - 1, m = j - 1; n > 0 && m > 0; n--, m--) //try move diagonly up left
                            {
                                if (board[n, m] != null && board[n, m].getColor() == playerTurn) break;
                                if (board[n, m] == null || (board[n, m] != null && board[n, m].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[n, m] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        WhiteCanMove = true;
                                }
                            }
                        }
                        if (board[i, j] is Knight)
                        {
                            if ((i >= 2 && j < 7) && (board[i - 2, j + 1] == null || (board[i - 2, j + 1].getColor() != playerTurn)))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 2, j + 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, i, j, newBoard))
                                    WhiteCanMove = true;
                            }
                            if ((i >= 2 && j > 0) && (board[i - 2, j - 1] == null || (board[i - 2, j - 1].getColor() != playerTurn)))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 2, j - 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    WhiteCanMove = true;
                            }
                            if ((i >= 1 && j < 6) && (board[i - 1, j + 2] == null || (board[i - 1, j + 2].getColor() != playerTurn)))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 1, j + 2] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    WhiteCanMove = true;
                            }
                            if ((i >= 1 && j > 1) && (board[i - 1, j - 2] == null || (board[i - 1, j - 2].getColor() != playerTurn)))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 1, j - 2] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    WhiteCanMove = true;
                            }
                            if ((i < 7 && j < 6) && (board[i + 1, j + 2] == null || (board[i + 1, j + 2].getColor() != playerTurn)))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 1, j + 2] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    WhiteCanMove = true;
                            }
                            if ((i < 7 && j > 1) && (board[i + 1, j - 2] == null || (board[i + 1, j - 2].getColor() != playerTurn)))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 1, j - 2] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    WhiteCanMove = true;
                            }
                            if ((i < 6 && j < 7) && (board[i + 2, j + 1] == null || (board[i + 2, j + 1].getColor() != playerTurn)))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 2, j + 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    WhiteCanMove = true;
                            }
                            if ((i < 6 && j > 0) && (board[i + 2, j - 1] == null || (board[i + 2, j - 1].getColor() != playerTurn)))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 2, j - 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    WhiteCanMove = true;
                            }
                        }
                        if (board[i, j] is King)
                        {
                            if (i != 0 && j != 0 && (board[i - 1, j - 1] == null || board[i - 1, j - 1].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 1, j - 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    WhiteCanMove = true;
                            } // up left
                            if (i != 0 && (board[i - 1, j] == null || board[i - 1, j].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 1, j] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    WhiteCanMove = true;
                            } // up left
                            if (i != 0 && j != 7 && (board[i - 1, j + 1] == null || board[i - 1, j + 1].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 1, j + 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    WhiteCanMove = true;
                            } // up right
                            if (j != 7 && (board[i, j + 1] == null || board[i, j + 1].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i, j + 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    WhiteCanMove = true;
                            } // right
                            if (i != 7 && j != 7 && (board[i + 1, j + 1] == null || board[i + 1, j + 1].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 1, j + 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    WhiteCanMove = true;
                            } // down right
                            if (i != 7 && (board[i + 1, j] == null || board[i + 1, j].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 1, j] = board[i, j];
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, i, j, newBoard))
                                    WhiteCanMove = true;
                            } // down
                            if (i != 7 && j != 0 && (board[i + 1, j - 1] == null || board[i + 1, j - 1].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 1, j - 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    WhiteCanMove = true;
                            } // down left
                            if (j != 0 && (board[i, j - 1] == null || board[i, j - 1].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i, j - 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    WhiteCanMove = true;
                            } // left
                        }
                    }
                }

            // if we checked all pieces scenerios and we dont see a option to move in the players turn, its stalemate
            if (!BlackCanMove || !WhiteCanMove) return true;
            return false; 
        }
    }
}
