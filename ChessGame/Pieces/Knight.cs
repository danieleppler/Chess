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

        public override bool Move(int currPawnRow, int currPawnColumn, int destRow, int destColumn, string player, Piece[,] board)
        {
            if (!base.Move(currPawnRow, currPawnColumn, destRow, destColumn, player, board))
                return false;

            bool validMove = false;

            //upper right "straight L"
            if (destRow == currPawnRow - 2 && destColumn == currPawnColumn + 1)
                validMove = true;

            //upper left "straight L"
            if (destRow == currPawnRow - 2 && destColumn == currPawnColumn - 1)
                validMove = true;

            //upper right "laying L"
            if (destRow == currPawnRow -1 && destColumn == currPawnColumn + 2)
                validMove = true;

            //upper left "laying L"
            if (destRow == currPawnRow - 1 && destColumn == currPawnColumn - 2)
                validMove = true;

            //beneath right "straight L"
            if (destRow == currPawnRow + 2 && destColumn == currPawnColumn + 1)
                validMove = true;

            //beneath left "straight L"
            if (destRow == currPawnRow + 2 && destColumn == currPawnColumn - 1)
                validMove = true;

            //beneath right "laying L"
            if (destRow == currPawnRow + 1 && destColumn == currPawnColumn + 2)
                validMove = true;

            //beneath left "laying L"
            if (destRow == currPawnRow + 1 && destColumn == currPawnColumn - 2)
                validMove = true;

            if (!validMove)
                return false;


            board[destRow, destColumn] = board[currPawnRow, currPawnColumn];
            board[currPawnRow, currPawnColumn] = null;
            return true;
        }

        public override string ToString()
        {
            return "N" + base.ToString();
        }
    }
}
