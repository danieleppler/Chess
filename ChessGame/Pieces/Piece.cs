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
        {//some restrictions that common to all pieces
            if (board[source.row, source.col].Color != player)//prevent moving with the opponent piece
                return false;
            if (source.Equals(destination))//if its the same place
                return false;
            if (!(board[destination.row,destination.col] is EmptyPiece) && board[destination.row, destination.col].Color == player)//if the place is occupied by the same player piece
                return false;
            return true;
        }
        public string getColor()
        {
            return Color;
        }
        public override string ToString()
        {
            return (this.Color == "white"? "w":"b");
        }
    }
}
