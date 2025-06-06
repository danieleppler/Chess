using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Pieces
{
    public class Queen : Piece
    {
        public Queen(string _color) : base(_color)
        {
        }

        public override bool IsLegalMove(BoardLocation source, BoardLocation destination, string player, Piece[,] board)
        {
            if (!base.IsLegalMove(source, destination, player, board))
                return false;

           Rook queenAsARook = new Rook(player);
           Bishop queenAsABishop = new Bishop(player);
           if(queenAsARook.IsLegalMove(source,destination,player,board) || queenAsABishop.IsLegalMove(source, destination, player, board))
            {
                board[destination.row, destination.col] = board[source.row, source.col];
                board[source.row, source.col] = new EmptyPiece();
            }
            return true;
        }
        public override string ToString()
        {
            return "Q" + base.ToString();
        }
    }
}
