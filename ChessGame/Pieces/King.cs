using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Pieces
{
    public class King : Piece
    {
        public King(string _color) : base(_color)
        {
        }

        public override bool Move(int currPieceRow, int currPieceColumn, int destRow, int destColumn, string player, Piece[,] board)
        {
            if (!base.Move(currPieceRow, currPieceColumn, destRow, destColumn, player, board))
                return false;

            bool validMove = false;

            //up
            if (destRow == currPieceRow - 1 && ((currPieceColumn == destColumn - 1) || (currPieceColumn == destColumn + 1) || (currPieceColumn == destColumn)))
                validMove = true;
            //down
            if (destRow == currPieceRow + 1 && ((currPieceColumn == destColumn - 1) || (currPieceColumn == destColumn + 1) || (currPieceColumn == destColumn)))
                validMove = true;
            //left
            if (destColumn == currPieceColumn - 1 && ((currPieceRow == destRow - 1) || (currPieceRow == destRow + 1) || (currPieceRow == destRow)))
                validMove = true;
            //right
            if (destColumn == currPieceColumn + 1 && ((currPieceRow == destRow - 1) || (currPieceRow == destRow + 1) || (currPieceRow == destRow)))
                validMove = true;

            bool castling = false;
            int CastlingRow = player == "white" ? 7 : 0;
            //castling
            if (this.MoveNumber == 0 && ((destColumn == currPieceColumn + 2) || (destColumn == currPieceColumn -2)) && !validMove && currPieceRow == destRow)
            {
               
                if ((board[CastlingRow, 0] is Rook && ((Rook) board[CastlingRow, 0]).GetMoveNumber() == 0) || (board[CastlingRow, 7] is Rook && ((Rook)board[CastlingRow, 7]).GetMoveNumber() == 0))
                {
                    if (destColumn > currPieceColumn)
                    {
                        if (board[CastlingRow, 6] == null & board[CastlingRow, 5] == null)
                        {
                            validMove = true;
                            castling = true;
                        }
                    }
                    else
                    {
                        if (board[CastlingRow, 3] == null & board[CastlingRow, 2] == null)
                        {
                            validMove = true;
                            castling = true;
                        }
                    }

                }
            }

            if (!validMove)
                return false;

            this.MoveNumber++;

            if (castling && destColumn > currPieceColumn)
            {
                board[CastlingRow, 5] = board[CastlingRow, 7];
                board[CastlingRow, 7] = null;
                Console.WriteLine("player used castling");
            }

            if(castling && destColumn < currPieceColumn)
            {
                board[CastlingRow, 3] = board[CastlingRow, 7];
                board[CastlingRow, 0] = null;
                Console.WriteLine("player used castling");
            }

            board[destRow, destColumn] = board[currPieceRow, currPieceColumn];
            board[currPieceRow, currPieceColumn] = null;
            return true;
        }
        public override string ToString()
        {
            return "K"+base.ToString();
        }
    }
}
