using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Pieces
{
    public class Rook : Piece
    {

        public Rook(string _color) : base(_color)
        {
          
        }

        public override bool Move(int currPieceRow, int currPieceColumn, int destRow, int destColumn, string player, Piece[,] board)
        {

            if (!base.Move(currPieceRow, currPieceColumn, destRow, destColumn, player, board))
                return false;

            bool validMove = false;

            //up
            if (currPieceRow > destRow && currPieceColumn == destColumn)
                for (int i = currPieceRow - 1; i >= destRow ; i--)
                {
                    if (i == destRow ) validMove = true;
                    if (board[i, currPieceColumn] != null) break;
                }

            //down
            if (!validMove && currPieceRow < destRow && currPieceColumn == destColumn)
                for (int i = currPieceRow + 1; i <= destRow; i++)
                {
                    if (i == destRow) validMove = true;
                    if (board[i, currPieceColumn] != null) break;
                }

            //left
            if (!validMove && currPieceRow == destRow && currPieceColumn > destColumn)
                for (int i = currPieceColumn - 1; i >= destColumn; i--)
                {
                    if (i == destColumn) validMove = true;
                    if (board[currPieceRow, i] != null) break;
                }

            //right
            if (!validMove && currPieceRow == destRow && currPieceColumn < destColumn)
                for (int i = currPieceColumn + 1; i <= destColumn; i++)
                {
                    if (i == destColumn) validMove = true;
                    if (board[currPieceRow, i] != null) break;
                }

            if (!validMove)
                return false;

            board[destRow, destColumn] = board[currPieceRow, currPieceColumn];
            board[currPieceRow, currPieceColumn] = null;
            return true;

        }

        public override string ToString()
        {
            return "R"+ base.ToString();
        }
    }
}
