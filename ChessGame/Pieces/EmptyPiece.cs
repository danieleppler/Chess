using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Pieces
{
    internal class EmptyPiece : Piece
    {
       public EmptyPiece() : base()
        {
        }

        public override bool IsLegalMove(BoardLocation source, BoardLocation destination, string player, Piece[,] board)
        {
            return false;
        }

        public override string ToString()
        {
            return "- ";
        }
    }
}
