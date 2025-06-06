using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessGame.Pieces;

namespace ChessGame
{
    public class Piece 
    {
       protected string Color;
       
       public Piece() { }
       public Piece(string _color) 
        {
            this.Color = _color;
        }


        public virtual bool IsLegalMove(BoardLocation source, BoardLocation destination, string player, Piece[,] board)
        {
            //some restrictions that common to all pieces
            //prevent moving with the opponent piece
            if (board[source.row, source.col].Color != player)
                return false;
            //if its the same place
            if (source.Equals(destination))
                return false;
            //if the place is occupied by the same player piece
            if (!(board[destination.row,destination.col] is EmptyPiece) && board[destination.row, destination.col].Color == player)
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

   

        public override string ToString()
        {
            return (this.Color == "white"? "w":"b");
        }
    }
}
