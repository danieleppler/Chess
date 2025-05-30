using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Draws
{
    internal class FifhtyMoveRuleDraw : IDraw
    {
     
        public bool IsDraw(Piece[,] board, Game currGame)
        {
            if(currGame.GetLastMoveThereWasCapture() < currGame.GetMoveNum() - 50 &&
                currGame.GetLastMoveWherePawnmoved() < currGame.GetMoveNum() - 50)
                return true;
            return false;
        }
    }
}
