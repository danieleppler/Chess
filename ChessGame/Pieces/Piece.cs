using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public class Piece 
    {
       protected string Color;
       protected int MoveNumber;
       

       public Piece(string _color) 
        {
            this.Color = _color;
            MoveNumber = 0;
        }


        public virtual bool Move(int currPieceRow, int currPieceColumn, int destRow, int destColumn, string player, Piece[,] board)
        {
            //prevent moving with the opponent piece
            if (board[currPieceRow, currPieceColumn].Color != player)
                return false;
            //if its the same place
            if (currPieceRow == destRow && currPieceColumn == destColumn)
                return false;
            //if the place is occupied by the same player piece
            if (board[destRow, destColumn] != null && board[destRow, destColumn].Color == player)
                return false;

            return true;
        }

        public string getColor()
        {
            return Color;
        }

        public bool setColor(string _color)
        {
            if(_color.ToLower() != "white" &&  _color.ToLower() !="black")
                return false;
            this.Color = _color.ToLower();
            return true;
        }

        public int GetMoveNumber()
        {
            return MoveNumber;
        }

        public override string ToString()
        {
            return (this.Color == "white"? "w":"b");
        }
    }
}
