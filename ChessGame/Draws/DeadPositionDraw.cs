using ChessGame.Pieces;

namespace ChessGame.Draws
{
    internal class DeadPositionDraw : IDraw
    {
        public bool IsDraw(Piece[,] board, Game currGame)
        {
            //king vs king
            bool kingVsKing = true;
            for (int i = 0; i < 7 && kingVsKing; i++)
                for (int j = 0; j < 7; j++)
                    if (board[i, j] != null && !(board[i, j] is King) && kingVsKing)
                        kingVsKing = false;

            bool KingVsKingAndBishop = true;
            int WhitePlayerKings = 0;
            int BlackPlayerKings = 0;
            int WhitePlayerBishops = 0;
            int BlackPlayerBishops = 0;
            for (int i = 0; i < 7; i++)
                for (int j = 0; j < 7; j++)
                {
                    if (board[i, j] is King && board[i, j].getColor() == "black")
                        BlackPlayerKings++;
                    if (board[i, j] is King && board[i, j].getColor() == "white")
                        WhitePlayerKings++;
                    if (board[i, j] is Bishop && board[i, j].getColor() == "white")
                        WhitePlayerBishops++;
                    if (board[i, j] is Bishop && board[i, j].getColor() == "black")
                        BlackPlayerBishops++;
                    if (board[i, j] != null && !(board[i, j] is Bishop || board[i, j] is King))
                        KingVsKingAndBishop = false;
                }

            if (KingVsKingAndBishop)
            {
                if ((WhitePlayerBishops == 1 && WhitePlayerKings == 1 && BlackPlayerBishops == 0 && BlackPlayerKings == 1) ||
                             (WhitePlayerBishops == 0 && WhitePlayerKings == 1 && BlackPlayerBishops == 1 && BlackPlayerKings == 1))
                    KingVsKingAndBishop = true;
                else KingVsKingAndBishop = false;
            }
             
            if (KingVsKingAndBishop || kingVsKing)
            {
                Console.Write("Dead position - ");
                return true;
            }
                

            return false;
        }
    }
}
