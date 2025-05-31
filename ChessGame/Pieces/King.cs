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

        public override bool Move(int currPawnRow, int currPawnColumn, int destRow, int destColumn, string player, Piece[,] board)
        {
            if (!base.Move(currPawnRow, currPawnColumn, destRow, destColumn, player, board))
                return false;

            bool validMove = false;

            //up
            if (destRow == currPawnRow - 1 && ((currPawnColumn == destColumn - 1) || (currPawnColumn == destColumn + 1) || (currPawnColumn == destColumn)))
                validMove = true;
            //down
            if (destRow == currPawnRow + 1 && ((currPawnColumn == destColumn - 1) || (currPawnColumn == destColumn + 1) || (currPawnColumn == destColumn)))
                validMove = true;
            //left
            if (destColumn == currPawnColumn - 1 && ((currPawnRow == destRow - 1) || (currPawnRow == destRow + 1) || (currPawnRow == destRow)))
                validMove = true;
            //right
            if (destColumn == currPawnColumn + 1 && ((currPawnRow == destRow - 1) || (currPawnRow == destRow + 1) || (currPawnRow == destRow)))
                validMove = true;

            bool castling = false;
            int CastlingRow = player == "white" ? 7 : 0;
            //castling
            if (this.MoveNumber == 0 && ((destColumn == currPawnColumn + 2) || (destColumn == currPawnColumn -2)) && !validMove && currPawnRow == destRow)
            {
               
                if ((board[CastlingRow, 0] is Rook && ((Rook) board[CastlingRow, 0]).getFirstMove()) || (board[CastlingRow, 7] is Rook && ((Rook)board[CastlingRow, 7]).getFirstMove()))
                {
                    if (destColumn > currPawnColumn)
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

            if (castling && destColumn > currPawnColumn)
            {
                board[CastlingRow, 5] = board[CastlingRow, 7];
                board[CastlingRow, 7] = null;
                Console.WriteLine("player used castling");
            }

            if(castling && destColumn < currPawnColumn)
            {
                board[CastlingRow, 3] = board[CastlingRow, 7];
                board[CastlingRow, 0] = null;
                Console.WriteLine("player used castling");
            }

            board[destRow, destColumn] = board[currPawnRow, currPawnColumn];
            board[currPawnRow, currPawnColumn] = null;
            return true;
        }
        public override string ToString()
        {
            return "K"+base.ToString();
        }
    }
}
