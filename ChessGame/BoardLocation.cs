using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    public class BoardLocation
    {
        public int row;
        public int col;
        public BoardLocation(int _row,int _column) {
            this.row = _row;
            this.col = _column;
        }
        public override bool Equals(object? obj)
        {
            if(!(obj is  BoardLocation)) return false;
            BoardLocation other = (BoardLocation)obj;
            if(this.row == other.row && this.col == other.col) return true;
            return false;
        }
    }
}
