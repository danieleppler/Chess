using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(string _color) : base(_color)
        {
        }

        public override bool IsLegalMove(BoardLocation source, BoardLocation destination, string player, Piece[,] board)
        {
            if (!base.IsLegalMove(source,destination, player, board))
                return false;
           
            if (IsMovingDownRight(source, destination, board) || IsMovingDownLeft(source, destination, board) || IsMovingUpLeft(source, destination, board)
                || IsMovingUpRight(source, destination, board))
            {
                board[destination.row, destination.col] = board[source.row, source.col];
                board[source.row, source.col] = new EmptyPiece();
                return true;
            }
            return false;
        }

        bool IsMovingDownRight(BoardLocation source,BoardLocation destination, Piece[,] board)
        {
            if (source.row < destination.row && source.col < destination.col)
                for (int i = source.row + 1, j = source.col + 1; i <= destination.row && j <= destination.col; i++, j++)
                {
                    if (i == destination.row && j == destination.col) return true;
                    if (!(board[i, j] is EmptyPiece)) break;
                }
            return false;
        }

        bool IsMovingDownLeft(BoardLocation source, BoardLocation destination, Piece[,] board)
        {
            if (source.row < destination.row && source.col > destination.col)
                for (int i = source.row + 1, j = source.col - 1; i <= destination.row && j >= destination.col; i++, j--)
                {
                    if (i == destination.row && j == destination.col) return true;
                    if (!(board[i, j] is EmptyPiece)) break;
                }
            return false;
        }

        bool IsMovingUpRight(BoardLocation source, BoardLocation destination, Piece[,] board)
        {
            if (source.row > destination.row && source.col < destination.col)
                for (int i = source.row - 1, j = source.col + 1; i >= destination.row && j <= destination.col; i--, j++)
                {
                    if (i == destination.row && j == destination.col) return true;
                    if (board[i, j] != null) break;
                }
            return false;
        }

        bool IsMovingUpLeft(BoardLocation source, BoardLocation destination, Piece[,] board)
        {
            if (source.row > destination.row && source.col > destination.col)
                for (int i = source.row - 1, j = source.col - 1; i >= destination.row && j >= destination.col; i--, j--)
                {
                    if (i == destination.row && j == destination.col) return true;
                    if (board[i, j] != null) break;
                }
            return false ;
        }

        public override string ToString()
        {
            return "B" + base.ToString();
        }
    }
}
