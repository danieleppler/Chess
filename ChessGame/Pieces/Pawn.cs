using System.Numerics;

namespace ChessGame.Pieces
{
    public class Pawn : Piece
    {
        int moveNumber;
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
                board[destination.row, destination.col] = board[source.row, source.col];
                board[source.row, source.col] = new EmptyPiece();
                this.moveNumber++;
                if (destination.row == 0 || destination.row == 7)
                    PromotePawn(source, destination,player,board);
                return true;
            }
            return false;
        }

        void PromotePawn(BoardLocation source, BoardLocation destination, string player, Piece[,] board)
        {

                string input;
                bool validInput;
                do
                {
                    validInput = true;
                    Console.WriteLine("Please chose a promotion for the pawn - Rook | Bishop | Queen | Knight");
                    input = Console.ReadLine();
                    switch (input)
                    {
                        case "Rook":
                            board[destination.row, destination.col] = new Rook(player);
                            break;

                        case "Bishop":
                            board[destination.row, destination.col] = new Bishop(player);
                            break;

                        case "Knight":
                            board[destination.row, destination.col] = new Knight(player);
                            break;

                        case "Queen":
                            board[destination.row, destination.col] = new Queen(player);
                            break;

                        default:
                            validInput = false;
                            Console.Write("Invalid Input ! ");
                            break;
                    }

                } while (!validInput);
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
            return (destination.col == source.col + 1) && 
                   (!(board[destination.row, destination.col] is EmptyPiece))  ||
                    (destination.col == source.col - 1) &&
                    (!(board[destination.row, destination.col] is EmptyPiece));
        }

        bool IsMovingStraight(BoardLocation source, BoardLocation destination, string player, Piece[,] board)
        {
            int direction;
            if (player == "white")
                direction = -1;
            else direction = 1;

            return
                //first move
                (this.moveNumber == 0 &&
                source.col == destination.col &&
                ((destination.row == (source.row + 2 * direction) && board[source.row + direction, source.col] is EmptyPiece) ||
                (destination.row == source.row + direction && board[destination.row, destination.col] is EmptyPiece))) ||
                //not first move
                (!(this.moveNumber == 0) && destination.col == source.col &&
                destination.row == source.row + direction && board[destination.row, destination.col] is EmptyPiece);
        }
       
        public override string ToString()
        {
            return "P" + base.ToString();
        }
    }
}
