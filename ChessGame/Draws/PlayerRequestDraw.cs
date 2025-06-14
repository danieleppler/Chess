﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Draws
{
    internal class PlayerRequestDraw : IDraw
    {
        public bool IsDraw(Piece[,] board, Game currGame,string playerTurn)
        {
            if(currGame.IsWhitePlayerAskedForDraw() && currGame.IsBlackPlayerAskedForDraw())
            {
                Console.Write("Both players agreed - ");
                return true;
            }
            return false;
        }
    }
}
