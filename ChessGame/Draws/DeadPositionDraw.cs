using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Draws
{
    internal class DeadPositionDraw : IDraw
    {
        public bool IsDraw(Piece[,] board, string playerTurn, Game currGame)
        {
            return false;
        }
    }
}
