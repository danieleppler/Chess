using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Draws
{
    public interface IDraw
    {
        bool IsDraw(Piece[,] board,string playerTurn,Game currGame);
    }
}
