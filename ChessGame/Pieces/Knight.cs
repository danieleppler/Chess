using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Pieces
{
    public class Knight : Piece
    {
        public Knight(string _color) : base(_color)
        {
        }

        public override bool IsLegalMove(BoardLocation source, BoardLocation destination, string player, Piece[,] board)
        {
            if (!base.IsLegalMove(source, destination, player, board))
                return false;
            if(IsMovingUpperRightStraightL(source,destination) ||
                IsMovingUpperLeftStraightL(source,destination) ||
                IsMovingUpperRightLayingL(source,destination) ||
                IsMovingUpperLeftLayingL(source,destination)||
                IsMovingDownLeftLayingL(source,destination)||
                IsMovingDownRightLayingL(source,destination)||
                IsMovingDownLeftStraightL(source,destination) ||
                IsMovingDownRightStraightL(source, destination))
            {
                return true;
            }
                return false;                 
        }

        bool IsMovingUpperRightStraightL(BoardLocation source,BoardLocation destination)
        {
            if (destination.row == source.row - 2 && destination.col == source.col + 1) return true;
            return false;
        }

        bool IsMovingUpperLeftStraightL(BoardLocation source, BoardLocation destination)
        {
            if (destination.row == source.row - 2 && destination.col == source.col - 1) return true;
            return false;
        }
        bool IsMovingUpperRightLayingL(BoardLocation source, BoardLocation destination)
        {
            if (destination.row == source.row - 1 && destination.col == source.col + 2) return true;
            return false;
        }
        bool IsMovingUpperLeftLayingL(BoardLocation source, BoardLocation destination)
        {
            if (destination.row == source.row - 1 && destination.col == source.col - 2) return true;
            return false;
        }
        bool IsMovingDownRightStraightL(BoardLocation source, BoardLocation destination)
        {
            if (destination.row == source.row + 2 && destination.col == source.col + 1) return true;
            return false;
        }
        bool IsMovingDownLeftStraightL(BoardLocation source, BoardLocation destination)
        {
            if (destination.row == source.row + 2 && destination.col == source.col - 1) return true;
            return false;
        }
        bool IsMovingDownRightLayingL(BoardLocation source, BoardLocation destination)
        {
            if (destination.row == source.row + 1 && destination.col == source.col + 2) return true;
            return false;
        }
        bool IsMovingDownLeftLayingL(BoardLocation source, BoardLocation destination)
        {
            if (destination.row == source.row + 1 && destination.col == source.col - 2) return true;
            return false;
        }

        public override string ToString()
        {
            return "N" + base.ToString();
        }
    }
}
