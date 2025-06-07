using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Pieces
{
    public class Rook : Piece
    {
        public int moveNumber;
        public Rook(string _color) : base(_color)
        {
          this.moveNumber = 0;
        }

        public override bool IsLegalMove(BoardLocation source, BoardLocation destination, string player, Piece[,] board)
        {

            if (!base.IsLegalMove(source, destination, player, board))
                return false;

            if (isMovingDown(source, destination, board) || isMovingLeft(source, destination, board) || 
                isMovingRight(source, destination, board) || isMovingUp(source, destination, board))
            {
                return true;
            }
            return false;

        }
        bool isMovingUp(BoardLocation source,BoardLocation destination, Piece[,] board)
        {
            if (source.row > destination.row && source.col == destination.col)
                for (int i = source.row - 1; i >= destination.row; i--)
                {
                    if (i == destination.row) return true;
                    if (!(board[i, destination.col] is EmptyPiece)) break;
               }
            return false;
        }
        bool isMovingDown(BoardLocation source, BoardLocation destination, Piece[,] board)
        {
            if (source.row < destination.row && source.col == destination.col)
                for (int i = source.row + 1; i <= destination.row; i++)
                {
                    if (i == destination.row) return true;
                    if (!(board[i, destination.col] is EmptyPiece)) break;
                }
            return false;
        }
        bool isMovingRight(BoardLocation source, BoardLocation destination, Piece[,] board)
        {
            if (source.row == destination.row && source.col < destination.col)
                for (int i = source.col + 1; i <= destination.col; i++)
                {
                    if (i == destination.col) return true;
                    if (!(board[source.row, i] is EmptyPiece)) break;
                }
            return false;
        }
        bool isMovingLeft(BoardLocation source, BoardLocation destination, Piece[,] board)
        {
            if (source.row == destination.row && source.col > destination.col)
                for (int i = source.col - 1; i >= destination.col; i--)
                {
                    if (i == destination.col) return true;
                    if (!(board[source.row, i] is EmptyPiece)) break;
                }
            return false;
        }

        public override string ToString()
        {
            return "R"+ base.ToString();
        }
    }
}
