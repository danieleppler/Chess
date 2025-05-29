using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ChessGame.Pieces;

namespace ChessGame.Draws
{
    internal class StalemateDraw : IDraw
    {
        public bool IsDraw(Piece[,] board, string playerTurn, Game currGame)
        {
            Piece[,] newBoard;
            int[] KingsPlace = currGame.getKingPlace(playerTurn);
            if (currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1],board)) //if the kings is in check its not a draw
                return false;

            //for every player's piece - check if the piece have valid move. if there is - return true
            for (int i = 0;i<7;i++) 
                for (int j = 0;j < 7; j++)
                {
                    if (board[i, j] != null && board[i, j].getColor() == playerTurn)
                    {
                        if (board[i, j] is Pawn)
                        {
                            if (playerTurn == "white")
                            {
                                if (i != 0 && board[i - 1, j] == null) //move straight , and then check if the king is in check. if not, its a valid move 
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i - 1, j] = newBoard[i, j];
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        return false;
                                }
                                if (i != 0 && j != 0 && board[i - 1, j - 1] != null && board[i - 1, j - 1].getColor() != playerTurn) //capture up left
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i - 1, j - 1] = newBoard[i, j];
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        return false;
                                }
                                if (i != 0 && j != 7 && board[i - 1, j + 1] != null && board[i - 1, j + 1].getColor() != playerTurn) //capture up right
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i - 1, j + 1] = newBoard[i, j];
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        return false;
                                }
                                if (i == 2 && board[i - 2, j] != null && board[i - 2, j].getColor() != playerTurn && ((Pawn)board[i, j]).GetMoveNumber() == 0) //move 2 tiles up
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i - 2, j] = newBoard[i, j];
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        return false;
                                }
                            }
                            else
                            {
                                if (i != 7 && board[i + 1, j] == null) //move straight , and then check if the king is in check. if not, its a valid move 
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i + 1, j] = newBoard[i, j];
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        return false;
                                }
                                if (i != 7 && j != 0 && board[i + 1, j - 1] != null && board[i - 1, j - 1].getColor() != playerTurn) //capture down left
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i + 1, j - 1] = newBoard[i, j];
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        return false;
                                }
                                if (i != 7 && j != 7 && board[i + 1, j + 1] != null && board[i - 1, j + 1].getColor() != playerTurn) //capture down right
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i + 1, j + 1] = newBoard[i, j];
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        return false;
                                }
                                if (i == 5 && board[i + 2, j] != null && board[i + 2, j].getColor() != playerTurn && ((Pawn)board[i, j]).GetMoveNumber() == 0) //move 2 tiles up
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i + 2, j] = newBoard[i, j];
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        return false;
                                }
                            }
                        }
                        if (board[i, j] is Rook || board[i, j] is Queen)
                        {
                            for (int k = j + 1; k < 7; k++) // try moving right
                            {
                                if (board[i, k] == null || (board[i, k] != null && board[i, k].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i, k] = board[i, j];
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1],newBoard))
                                        return false;
                                }
                            }
                            for (int k = j - 1; k > 0; k--) // try moving left
                            {
                                if (board[i, k] == null || (board[i, k] != null && board[i, k].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[i, k] = board[i, j];
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        return false;
                                }
                            }
                            for (int k = i - 1; k > 0; k--) // try moving up
                            {
                                if (board[k, j] == null || (board[k, j] != null && board[i, k].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[k, j] = board[i, j];
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        return false;
                                }
                            }
                            for (int k = i + 1; k < 7; k++) // try moving down
                            {
                                if (board[k, j] == null || (board[k, j] != null && board[k, j].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[k, j] = board[i, j];
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        return false;
                                }
                            }
                        }
                        if (board[i, j] is Bishop || board[i,j] is Queen)
                        {
                            for (int n = i+1,m=j+1; n < 7 && m < 7; n++,m++) //try move diagonly down right
                            {
                                if (board[n, m] == null || (board[n, m] != null && board[n, m].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[n, m] = board[i, j];
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        return false;
                                }
                            }
                            for (int n = i + 1, m = j - 1; n < 7 && m > 0; n++, m--) //try move diagonly down left
                            {
                                if (board[n, m] == null || (board[n, m] != null && board[n, m].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[n, m] = board[i, j];
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        return false;
                                }
                            }
                            for (int n = i - 1, m = j + 1; n > 0 && m < 7; n--, m++) //try move diagonly up right
                            {
                                if (board[n, m] == null || (board[n, m] != null && board[n, m].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[n, m] = board[i, j];
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        return false;
                                }
                            }
                            for (int n = i - 1, m = j - 1; n > 0 && m > 0; n--, m--) //try move diagonly up left                            {
                                if (board[n, m] == null || (board[n, m] != null && board[n, m].getColor() != playerTurn))
                                {
                                    newBoard = currGame.MakenewCopyOfBoard(board);
                                    newBoard[n, m] = board[i, j];
                                    if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                        return false;
                                }
                            }
                        if (board[i,j] is Knight)
                        {
                            if ((i >= 2 && j < 7) && board[i - 2, j + 1] is Knight && (board[i - 2, j + 1].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 2, j + 1] = board[i , j];
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, i, j, newBoard))
                                    return false;
                            }
                            if ((i >= 2 && j > 0) && board[i - 2, j - 1] is Knight && (board[i - 2, j - 1].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 2, j - 1] = board[i, j];
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    return false;
                            }
                            if ((i >= 1 && j < 6) && board[i - 1, j + 2] is Knight && (board[i - 1, j + 2].getColor() != playerTurn)) return true;
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 1, j +2] = board[i, j];
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    return false;
                            }
                            if ((i >= 1 && j > 1) && board[i - 1, j - 2] is Knight && (board[i - 1, j - 2].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 1, j - 2] = board[i, j];
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    return false;
                            }
                            if ((i < 7 && j < 6) && board[i + 1, j + 2] is Knight && (board[i + 1, j + 2].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 1, j + 2] = board[i, j];
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    return false;
                            }
                            if ((i < 7 && j > 1) && board[i + 1, j - 2] is Knight && (board[i + 1, j - 2].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 1, j - 2] = board[i, j];
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    return false;
                            }
                            if ((i < 6 && j < 7) && board[i + 2, j + 1] is Knight && (board[i + 2, j + 1].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 2, j+ 1] = board[i, j];
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    return false;
                            }
                            if ((i < 6 && j > 0) && board[i + 2, j - 1] is Knight && (board[i + 2, j - 1].getColor() != playerTurn))
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 2, j - 1] = board[i, j];
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    return false;
                            }
                        }
                        if (board[i,j] is King)
                        {
                            if (i != 0 && j != 0 && board[i - 1, j - 1] is King && board[i - 1, j - 1].getColor() != playerTurn)
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 1, j - 1] = board[i, j];
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    return false;
                            } // up left
                            if (i != 0 && board[i - 1, j] is King && board[i - 1, j].getColor() != playerTurn)
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 1, j ] = board[i, j];
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    return false;
                            } // up left
                            if (i != 0 && j != 7 && board[i - 1, j + 1] is King && board[i - 1, j + 1].getColor() != playerTurn) {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i - 1, j + 1] = board[i, j];
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    return false;
                            } // up right
                            if (j != 7 && board[i, j + 1] is King && board[i, j + 1].getColor() != playerTurn)
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i , j + 1] = board[i, j];
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    return false;
                            } // right
                            if (i != 7 && j != 7 && board[i + 1, j + 1] is King && board[i + 1, j + 1].getColor() != playerTurn)
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 1, j + 1] = board[i, j];
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    return false;
                            } // down right
                            if (i != 7 && board[i + 1, j] is King && board[i + 1, j].getColor() != playerTurn)
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 1, j ] = board[i, j];
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, i, j, newBoard))
                                    return false;
                            } // down
                            if (i != 7 && j != 0 && board[i + 1, j - 1] is King && board[i + 1, j - 1].getColor() != playerTurn)
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i + 1, j - 1] = board[i, j];
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    return false;
                            } // down left
                            if (j != 0 && board[i, j - 1] is King && board[i, j - 1].getColor() != playerTurn)
                            {
                                newBoard = currGame.MakenewCopyOfBoard(board);
                                newBoard[i , j - 1] = board[i, j];
                                if (!currGame.IsPieceInCaptureDanger(playerTurn, KingsPlace[0], KingsPlace[1], newBoard))
                                    return false;
                            } // left
                        }
                        }
                    }
            return true; // if we checked all pieces scenerios and we dont see a option to move in the players turn, its stalemate
        }
    }
}
