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
        public bool IsDraw(Piece[,] board, Game currGame,string playerTurn)
        {

            Piece[,] newBoard;

            int[] KingsPlace;
            KingsPlace = currGame.getKingPlace(playerTurn);
            if (currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1],board)) //if the kings is in check its not a draw
            {
                    return false;
            }

            bool canMove = false ;

            //for every player's piece - check if the piece have valid move. if there is - return true
            for (int i = 0;i < 8;i++) 
                for (int j = 0;j < 8; j++)
                {
                    if (board[i, j] != null && board[i, j].getColor() == playerTurn)
                    {
                        if (board[i, j] is Pawn && playerTurn=="white")
                        {

                            if (i != 0 && board[i - 1, j] == null) //move straight , and then check if the king is in check. if not, its a valid move 
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 1, j] = newBoard[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    canMove = true;
                            }
                            if (i != 0 && j != 0 && board[i - 1, j - 1] != null && board[i - 1, j - 1].getColor() != playerTurn) //capture up left
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 1, j - 1] = newBoard[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    canMove = true;
                            }
                            if (i != 0 && j != 7 && board[i - 1, j + 1] != null && board[i - 1, j + 1].getColor() != playerTurn) //capture up right
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 1, j + 1] = newBoard[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    canMove = true;
                            }
                            if (i == 2 && board[i - 2, j] != null && board[i - 2, j].getColor() != playerTurn && ((Pawn)board[i, j]).GetMoveNumber() == 0) //move 2 tiles up
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 2, j] = newBoard[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    canMove = true;
                            }


                        }
                        if (board[i, j] is Pawn && playerTurn == "black")
                        {
                                if (i != 7 && board[i + 1, j] == null) //move straight , and then check if the king is in check. if not, its a valid move 
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i + 1, j] = newBoard[i, j];
                                    newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        canMove = true;
                                }
                                if (i != 7 && j != 0 && board[i + 1, j - 1] != null && board[i + 1, j - 1].getColor() != playerTurn) //capture down left
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i + 1, j - 1] = newBoard[i, j];
                                    newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    canMove = true;
                            }
                                if (i != 7 && j != 7 && board[i + 1, j + 1] != null && board[i + 1, j + 1].getColor() != playerTurn) //capture down right
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i + 1, j + 1] = newBoard[i, j];
                                    newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    canMove = true;
                            }
                                if (i == 5 && board[i + 2, j] != null && board[i + 2, j].getColor() != playerTurn && ((Pawn)board[i, j]).GetMoveNumber() == 0) //move 2 tiles up
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i + 2, j] = newBoard[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    canMove = true;
                            }
                            
                        }
                        if (board[i, j] is Rook || board[i, j] is Queen)
                        {
                            for (int k = j + 1; k < 8; k++) // try moving right
                            {
                                if (board[i, k] != null && board[i, k].getColor() == playerTurn) break;
                                if (board[i, k] == null || (board[i, k] != null && board[i, k].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i, k] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1],newBoard))
                                        canMove = true;
                                }
                            }
                            for (int k = j - 1; k >= 0; k--) // try moving left
                            {
                                if (board[i, k] != null && board[i, k].getColor() == playerTurn) break;
                                if (board[i, k] == null || (board[i, k] != null && board[i, k].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i, k] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        canMove = true;
                                }
                            }
                            for (int k = i - 1; k >= 0; k--) // try moving up
                            {
                                if (board[k, j] != null && board[k, j].getColor() == playerTurn) break;
                                if (board[k, j] == null || (board[k, j] != null && board[k, j].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[k, j] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        canMove = true;
                                }
                            }
                            for (int k = i + 1; k < 8; k++) // try moving down
                            {
                                if (board[k, j] != null && board[k, j].getColor() == playerTurn) break;
                                if (board[k, j] == null || (board[k, j] != null && board[k, j].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[k, j] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        canMove = true;
                                }
                            }
                        }
                        if (board[i, j] is Bishop || board[i,j] is Queen)
                        {
                            for (int n = i+1,m=j+1; n < 8 && m < 8; n++,m++) //try move diagonly down right
                            {
                                if (board[n, m] != null && board[n, m].getColor() == playerTurn) break;
                                if (board[n, m] == null || (board[n, m] != null && board[n, m].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[n, m] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        canMove = true;
                                }
                            }
                            for (int n = i + 1, m = j - 1; n < 8 && m >= 0; n++, m--) //try move diagonly down left
                            {
                                if (board[n, m] != null && board[n, m].getColor() == playerTurn) break;
                                if (board[n, m] == null || (board[n, m] != null && board[n, m].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[n, m] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        canMove = true;
                                }
                            }
                            for (int n = i - 1, m = j + 1; n >= 0 && m < 8; n--, m++) //try move diagonly up right
                            {
                                if (board[n, m] != null && board[n, m].getColor() == playerTurn) break;
                                if (board[n, m] == null || (board[n, m] != null && board[n, m].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[n, m] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        canMove = true;
                                }
                            }
                            for (int n = i - 1, m = j - 1; n >= 0 && m >= 0; n--, m--) //try move diagonly up left
                            {
                                if (board[n, m] != null && board[n, m].getColor() == playerTurn) break;
                                if (board[n, m] == null || (board[n, m] != null && board[n, m].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[n, m] = board[i, j];
                                    newBoard[i, j] = null;
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        canMove = true;
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
                                    canMove = true;
                            }
                            if ((i >= 2 && j > 0) && (board[i - 2, j - 1] == null || (board[i - 2, j - 1].getColor() != playerTurn)))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 2, j - 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    canMove = true;
                            }
                            if ((i >= 1 && j < 6) && (board[i - 1, j + 2] == null || (board[i - 1, j + 2].getColor() != playerTurn))) 
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 1, j +2] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    canMove = true;
                            }
                            if ((i >= 1 && j > 1) && (board[i - 1, j - 2] == null || (board[i - 1, j - 2].getColor() != playerTurn)))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 1, j - 2] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    canMove = true;
                            }
                            if ((i < 7 && j < 6) && (board[i + 1, j + 2] == null || (board[i + 1, j + 2].getColor() != playerTurn)))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 1, j + 2] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    canMove = true;
                            }
                            if ((i < 7 && j > 1) && (board[i + 1, j - 2] == null || (board[i + 1, j - 2].getColor() != playerTurn)))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 1, j - 2] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    canMove = true;
                            }
                            if ((i < 6 && j < 7) && (board[i + 2, j + 1] == null || (board[i + 2, j + 1].getColor() != playerTurn)))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 2, j+ 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    canMove = true;
                            }
                            if ((i < 6 && j > 0) && (board[i + 2, j - 1] == null || (board[i + 2, j - 1].getColor() != playerTurn)))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 2, j - 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    canMove = true;
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
                                    canMove = true;
                            } // up left
                            if (i != 0 && (board[i - 1, j] == null || board[i - 1, j].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 1, j ] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, i - 1, j, newBoard))
                                    canMove = true;
                            } // up left
                            if (i != 0 && j != 7 && (board[i - 1, j + 1] == null || board[i - 1, j + 1].getColor() != playerTurn)) {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 1, j + 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, i - 1, j + 1, newBoard))
                                    canMove = true;
                            } // up right
                            if (j != 7 && (board[i, j + 1] == null || board[i, j + 1].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i , j + 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, i, j + 1, newBoard))
                                    canMove = true;
                            } // right
                            if (i != 7 && j != 7 && (board[i + 1, j + 1] == null || board[i + 1, j + 1].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 1, j + 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, i + 1, j + 1, newBoard))
                                    canMove = true;
                            } // down right
                            if (i != 7 && (board[i + 1, j] == null || board[i + 1, j].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 1, j ] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, i+1, j, newBoard))
                                    canMove = true;
                            } // down
                            if (i != 7 && j != 0 && (board[i + 1, j - 1] == null || board[i + 1, j - 1].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 1, j - 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, i + 1, j - 1, newBoard))
                                    canMove = true;
                            } // down left
                            if (j != 0 && (board[i, j - 1] == null || board[i, j - 1].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i , j - 1] = board[i, j];
                                newBoard[i, j] = null;
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, i, j - 1, newBoard))
                                    canMove = true;
                            } // left
                        }
                        }
                    }

            // if we checked all pieces scenerios and we dont see a option to move in the players turn, its stalemate
            if (!canMove ) {
                Console.Write("Stalemate - ");
                return true;
            }
            return false; 
        }
    }
}
