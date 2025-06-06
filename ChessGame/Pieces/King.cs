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
        int moveNumber;
        public King(string _color) : base(_color)
        {
            moveNumber = 0;
        }
        public int getMoveNumber()
        {
            return moveNumber;
        }

        public override bool IsLegalMove(BoardLocation source, BoardLocation destination, string player, Piece[,] board)
        {
            if (!base.IsLegalMove(source, destination, player, board))
                return false;
            bool validMove = false;
            //up
            if (destination.row == source.row - 1 && ((source.col == destination.col - 1) || (source.col == destination.col + 1) || (source.col == destination.col)))
                validMove = true;
            //down
            if (destination.row == source.row + 1 && ((source.col == destination.col - 1) || (source.col == destination.col + 1) || (source.col == destination.col)))
                validMove = true;
            //left
            if (destination.col == source.col - 1 && ((source.row == destination.row - 1) || (source.row == destination.row + 1) || (source.row == destination.row)))
                validMove = true;
            //right
            if (destination.col == source.col + 1 && ((source.row == destination.row - 1) || (source.row == destination.row + 1) || (source.row == destination.row)))
                validMove = true;
            if (validMove)
            {
                board[destination.row, destination.col] = board[source.row, source.col];
                board[source.row, source.col] = new EmptyPiece();
                this.moveNumber++;
                return true;
            }
            if (isMovingIsCastling(source, destination, player, board))
                return true;
            else return false;         
        }

        bool isMovingIsCastling(BoardLocation source,BoardLocation destination,string player, Piece[,] board)
        {
            int CastlingRow = player == "white" ? 7 : 0;
            int CastlingDireciton = (destination.col == source.col + 2) ? 1 : -1; //-1 for left 1 for right
            if (this.moveNumber == 0 && CastlingDireciton != 0 && source.row == destination.row)
            {
                if ((board[CastlingRow, 0] is Rook && ((Rook)board[CastlingRow, 0]).GetMoveNumber() == 0) || (board[CastlingRow, 7] is Rook && ((Rook)board[CastlingRow, 7]).GetMoveNumber() == 0))
                {
                        if (board[CastlingRow, source.col + CastlingDireciton] is EmptyPiece & board[CastlingRow, source.col + (2 * CastlingDireciton)] is EmptyPiece)
                        {
                        board[CastlingRow, source.col] = board[CastlingRow, source.col + (2*CastlingDireciton)];
                        board[CastlingRow, source.col] = new EmptyPiece();
                        return true;
                        }
                }
            }
            return false;
        }

        public override string ToString()
        {
            return "K"+base.ToString();
        }
    }
}
