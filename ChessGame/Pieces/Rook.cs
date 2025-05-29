using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Pieces
{
    public class Rook : Piece
    {
        bool firstMove;
        public Rook(string _color) : base(_color)
        {
            firstMove = true;
        }

        public override bool Move(int currPawnRow, int currPawnColumn, int destRow, int destColumn, string player, Piece[,] board)
        {

            if (!base.Move(currPawnRow, currPawnColumn, destRow, destColumn, player, board))
                return false;

            bool validMove = false;

            //up
            if (currPawnRow > destRow && currPawnColumn == destColumn)
                for (int i = currPawnRow - 1; i >= destRow ; i--)
                {
                    if (i == destRow ) validMove = true;
                    if (board[i, currPawnColumn] != null) break;
                }

            //down
            if (!validMove && currPawnRow < destRow && currPawnColumn == destColumn)
                for (int i = currPawnRow + 1; i <= destRow; i++)
                {
                    if (i == destRow) validMove = true;
                    if (board[i, currPawnColumn] != null) break;
                }

            //left
            if (!validMove && currPawnRow == destRow && currPawnColumn > destColumn)
                for (int i = currPawnColumn - 1; i >= destColumn; i--)
                {
                    if (i == destColumn) validMove = true;
                    if (board[currPawnRow, i] != null) break;
                }

            //right
            if (!validMove && currPawnRow == destRow && currPawnColumn < destColumn)
                for (int i = currPawnColumn + 1; i <= destColumn; i++)
                {
                    if (i == destColumn) validMove = true;
                    if (board[currPawnRow, i] != null) break;
                }

            if (!validMove)
                return false;

            if(firstMove) firstMove = false;
            board[destRow, destColumn] = board[currPawnRow, currPawnColumn];
            board[currPawnRow, currPawnColumn] = null;
            return true;

        }

        public bool getFirstMove()
        {
            return firstMove;
        }

        public override string ToString()
        {
            return "R"+ base.ToString();
        }
    }
}
