namespace ChessGame.Pieces
{
    public class Pawn : Piece
    {
 
        public Pawn(string _color) : base(_color)
        {

        }

        public int GetMoveNumber()
        {
            return this.MoveNumber;
        }

        public override bool Move(int currPawnRow, int currPawnColumn, int destRow, int destColumn, string player, Piece[,] board)
        {

            if (!base.Move(currPawnRow, currPawnColumn, destRow, destColumn, player, board))
                return false;

            int direction;
            if (player == "white")
                direction = -1;
            else direction = 1;


            //conditions
            bool CaptureChance = destColumn == currPawnColumn + 1 && board[destRow, destColumn] != null && board[destRow, destColumn].getColor() != player ||
                    destColumn == currPawnColumn - 1 && board[destRow, destColumn] != null && board[destRow, destColumn].getColor() != player;
            bool MoveWithoutCapureFirstMove = this.MoveNumber == 0 && currPawnColumn == destColumn &&
                (destRow == (currPawnRow + 2 * direction) && board[currPawnRow + direction, currPawnColumn] == null ||
                destRow == currPawnRow + direction && board[destRow, destColumn] == null);
            bool MoveWithoutCaptureNotFirstMove = !(this.MoveNumber == 0) && destColumn == currPawnColumn &&
                destRow == currPawnRow + direction && board[destRow, destColumn] == null;
            bool enPassant = false;

            //if no valid move found - check if its en passant
            if(destColumn == currPawnColumn + 1 && !MoveWithoutCaptureNotFirstMove && !CaptureChance && !MoveWithoutCapureFirstMove)
            {
                if (board[currPawnRow,currPawnColumn + 1] is Pawn && ((Pawn)board[currPawnRow, currPawnColumn +1]).MoveNumber == 1 && board[currPawnRow + (1 * direction), destColumn] == null)
                {
                    enPassant = true;
                    board[currPawnRow , currPawnColumn + 1] = null;
                }
            }

            if (destColumn == currPawnColumn - 1 && !MoveWithoutCaptureNotFirstMove && !CaptureChance && !MoveWithoutCapureFirstMove)
            {
                if (board[currPawnRow, currPawnColumn - 1] is Pawn && ((Pawn)board[currPawnRow, currPawnColumn-1]).MoveNumber == 1 && board[currPawnRow + (1 * direction), destColumn] == null)
                {
                    enPassant = true;
                    board[currPawnRow , currPawnColumn -1 ] = null;
                }
            }


                if (CaptureChance || MoveWithoutCaptureNotFirstMove || MoveWithoutCapureFirstMove || enPassant)
            {
                                
                board[destRow, destColumn] = board[currPawnRow, currPawnColumn];
                board[currPawnRow, currPawnColumn] = null;

                //check if promotion
                if (destRow == 0 || destRow == 7)
                {
                    string input;
                    bool validInput ;
                    do
                    {
                        validInput = true ;
                        Console.WriteLine("Please chose a promotion for the pawn - Rook | Bishop | Queen | Knight");
                        input = Console.ReadLine();
                        switch (input)
                        {
                            case "Rook":
                                board[destRow, destColumn] = new Rook(player);
                                break;

                            case "Bishop":
                                board[destRow, destColumn] = new Bishop(player);
                                break;

                            case "Knight":
                                board[destRow, destColumn] = new Knight(player);
                                break;

                            case "Queen":
                                board[destRow, destColumn] = new Queen(player);
                                break;

                            default:
                                validInput = false;
                                Console.Write("Invalid Input ! ");
                                break;
                        }

                    } while (!validInput);
                }

                this.MoveNumber++;
                return true;
            }
            return false;

        }

       
        public override string ToString()
        {
            return "P" + base.ToString();
        }
    }
}
