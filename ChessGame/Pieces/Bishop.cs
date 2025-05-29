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

        public override bool Move(int currPawnRow, int currPawnColumn, int destRow, int destColumn, string player, Piece[,] board)
        {
            if (!base.Move(currPawnRow, currPawnColumn, destRow, destColumn, player, board))
                return false;


            bool validMove =false;

           //beneath right 
           if(currPawnRow < destRow && currPawnColumn < destColumn)
                for(int i = currPawnRow + 1,j = currPawnColumn + 1; i <= destRow && j <= destColumn; i++, j++)
                {
                    if (i == destRow && j == destColumn) validMove = true;
                    if (board[i, j] != null) break;
                }

            //beneath left
            if (!validMove && currPawnRow < destRow && currPawnColumn > destColumn)
                for (int i = currPawnRow + 1, j = currPawnColumn - 1; i <= destRow && j >= destColumn; i++, j--)
                {
                    if (i == destRow && j == destColumn) validMove = true;
                    if (board[i, j] != null) break;
                }
                    

            //upper left
            if (!validMove && currPawnRow > destRow && currPawnColumn > destColumn)
                for (int i = currPawnRow - 1, j = currPawnColumn - 1; i >= destRow && j >= destColumn; i--, j--)
                {
                    if (i == destRow && j == destColumn) validMove = true;
                    if (board[i, j] != null) break;
                }


            //upper right 
            if (!validMove && currPawnRow > destRow && currPawnColumn < destColumn)
                for (int i = currPawnRow - 1, j = currPawnColumn + 1; i >= destRow && j <= destColumn; i--, j++)
                {
                    if (i == destRow && j == destColumn) validMove = true;
                    if (board[i, j] != null) break;
                }


            if (!validMove)
                return false;

            board[destRow, destColumn] = board[currPawnRow, currPawnColumn];
            board[currPawnRow, currPawnColumn] = null;
            return true;
        }

        public override string ToString()
        {
            return "B" + base.ToString();
        }
    }
}
