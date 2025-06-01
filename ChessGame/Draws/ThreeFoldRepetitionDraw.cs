using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Draws
{
    internal class ThreeFoldRepetitionDraw : IDraw
    {
        
        public bool IsDraw(Piece[,] board, Game currGame, string playerTurn)
        {
            string[] boardsMemory = currGame.GetBoardMemory();
            for (int i = 0; i < boardsMemory.Length && boardsMemory[i] != null; i++) { 
                int BoardCount = 1;
                for (int j = i + 1; j < boardsMemory.Length && boardsMemory[j] != null; j++)
                {
                    if (boardsMemory[j] == boardsMemory[i])
                        BoardCount++;
                    if(BoardCount == 3)
                    {
                        Console.Write("Three fold repetition - ");
                        return true;
                    }
                        
                }
            }
            return false;
        }
    }
}
