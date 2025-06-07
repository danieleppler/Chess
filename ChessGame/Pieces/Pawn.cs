using System.Numerics;

namespace ChessGame.Pieces
{
    public class Pawn : Piece
    {
        public int moveNumber;
        public Pawn(string _color) : base(_color)
        {
            this.moveNumber = 0;
        }

        public override bool IsLegalMove(BoardLocation source, BoardLocation destination, string player, Piece[,] board)
        {
            if (!base.IsLegalMove(source,destination, player, board))
                return false;

            if(IsCapturing(source,destination,player, board) || IsMovingStraight(source, destination, player, board) || IsEnPassant(source, destination, player, board))
            {
                return true;
            }
            return false;
        }

 
        bool IsEnPassant(BoardLocation source, BoardLocation destination, string player, Piece[,] board)
        {
            int direction;
            if (player == "white")
                direction = -1;
            else direction = 1;
            if (destination.col == source.col + 1)
            {
                if (board[source.row, source.col + 1] is Pawn && ((Pawn)board[source.row, source.col + 1]).moveNumber == 1 && board[source.row + (1 * direction), destination.col] == null)
                {
                    board[source.row, source.col + 1] = new EmptyPiece();
                    return true;
                }
            }

            if (destination.col == source.col - 1)
            {
                if (board[source.row, source.col - 1] is Pawn && ((Pawn)board[source.row, source.col - 1]).moveNumber == 1 && board[source.row + (1 * direction), destination.col] == null)
                {
                    board[source.row, source.col - 1] = new EmptyPiece();
                    return true;
                }
            }

            return false;
        }
        bool IsCapturing(BoardLocation source,BoardLocation destination ,string player, Piece[,] board)
        {
            int direction;
            if (player == "white")
                direction = -1;
            else direction = 1;
            bool valid = (destination.row == source.row + direction) &&  ((destination.col == source.col + 1 &&  !(board[destination.row, destination.col] is EmptyPiece) &&
                board[destination.row, destination.col].getColor() != player) ||
                    (destination.col == source.col - 1 && !(board[destination.row, destination.col] is EmptyPiece) &&
                    board[destination.row, destination.col].getColor() != player));

            return valid;
        }

        bool IsMovingStraight(BoardLocation source, BoardLocation destination, string player, Piece[,] board)
        {
            int direction;
            if (player == "white")
                direction = -1;
            else direction = 1;
            bool IstwoSteps = this.moveNumber == 0 && source.col == destination.col &&
                destination.row == (source.row + 2 * direction) && board[source.row + direction, source.col] is EmptyPiece && board[destination.row,destination.col] is EmptyPiece;
            bool IsOneStep =
                destination.col == source.col &&
                destination.row == source.row + direction && board[destination.row, destination.col] is EmptyPiece;


            return IsOneStep || IstwoSteps;
        }
       
        public override string ToString()
        {
            return "P" + base.ToString();
        }
    }
}
