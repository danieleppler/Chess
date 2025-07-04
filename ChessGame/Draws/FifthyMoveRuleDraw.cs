﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Draws
{
    internal class FifhtyMoveRuleDraw : IDraw
    {
     
        public bool IsDraw(Piece[,] board, Game currGame, string player)
        {
            if(currGame.GetLastMoveThereWasCapture() <= currGame.GetMoveNum() - 100 &&
                currGame.GetLastMoveWherePawnmoved() <= currGame.GetMoveNum() - 100)
            {
                Console.Write("Fifthy move rule - ");
                return true;
            }
                
            return false;
        }
    }
}
