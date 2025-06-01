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

        public override bool Move(int currPieceRow, int currPieceColumn, int destRow, int destColumn, string player, Piece[,] board)
        {
            if (!base.Move(currPieceRow, currPieceColumn, destRow, destColumn, player, board))
                return false;

            bool validMove = false;

            //upper right "straight L"
            if (destRow == currPieceRow - 2 && destColumn == currPieceColumn + 1)
                validMove = true;

            //upper left "straight L"
            if (destRow == currPieceRow - 2 && destColumn == currPieceColumn - 1)
                validMove = true;

            //upper right "laying L"
            if (destRow == currPieceRow -1 && destColumn == currPieceColumn + 2)
                validMove = true;

            //upper left "laying L"
            if (destRow == currPieceRow - 1 && destColumn == currPieceColumn - 2)
                validMove = true;

            //beneath right "straight L"
            if (destRow == currPieceRow + 2 && destColumn == currPieceColumn + 1)
                validMove = true;

            //beneath left "straight L"
            if (destRow == currPieceRow + 2 && destColumn == currPieceColumn - 1)
                validMove = true;

            //beneath right "laying L"
            if (destRow == currPieceRow + 1 && destColumn == currPieceColumn + 2)
                validMove = true;

            //beneath left "laying L"
            if (destRow == currPieceRow + 1 && destColumn == currPieceColumn - 2)
                validMove = true;

            if (!validMove)
                return false;


            board[destRow, destColumn] = board[currPieceRow, currPieceColumn];
            board[currPieceRow, currPieceColumn] = null;
            return true;
        }

        public override string ToString()
        {
            return "N" + base.ToString();
        }
    }
}
