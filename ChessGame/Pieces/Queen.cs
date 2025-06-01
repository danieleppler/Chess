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

        public override bool Move(int currPieceRow, int currPieceColumn, int destRow, int destColumn, string player, Piece[,] board)
        {
            if (!base.Move(currPieceRow, currPieceColumn, destRow, destColumn, player, board))
                return false;

            bool validMove = false;

            //up
            if (currPieceRow > destRow && currPieceColumn == destColumn)
                for (int i = currPieceRow - 1; i >= destRow; i--)
                {
                    if (i == destRow) validMove = true;
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

            //beneath right 
            if (currPieceRow < destRow && currPieceColumn < destColumn)
                for (int i = currPieceRow + 1, j = currPieceColumn + 1; i <= destRow && j <= destColumn; i++, j++)
                {
                    if (i == destRow && j == destColumn) validMove = true;
                    if (board[i, j] != null) break;
                }

            //beneath left
            if (!validMove && currPieceRow < destRow && currPieceColumn > destColumn)
                for (int i = currPieceRow + 1, j = currPieceColumn - 1; i <= destRow && j >= destColumn; i++, j--)
                {
                    if (i == destRow && j == destColumn) validMove = true;
                    if (board[i, j] != null) break;
                }


            //upper left
            if (!validMove && currPieceRow > destRow && currPieceColumn > destColumn)
                for (int i = currPieceRow - 1, j = currPieceColumn - 1; i >= destRow && j >= destColumn; i--, j--)
                {
                    if (i == destRow && j == destColumn) validMove = true;
                    if (board[i, j] != null) break;
                }


            //upper right 
            if (!validMove && currPieceRow > destRow && currPieceColumn < destColumn)
                for (int i = currPieceRow - 1, j = currPieceColumn + 1; i >= destRow && j <= destColumn; i--, j++)
                {
                    if (i == destRow && j == destColumn) validMove = true;
                    if (board[i, j] != null) break;
                }

            if (!validMove)
                return false;

            board[destRow, destColumn] = board[currPieceRow, currPieceColumn];
            board[currPieceRow, currPieceColumn] = null;
            return true;
        }
        public override string ToString()
        {
            return "Q" + base.ToString();
        }
    }
}
