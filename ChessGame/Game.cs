using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using ChessGame.Draws;
using ChessGame.Pieces;

namespace ChessGame
{
    public class Game
    {
        int MoveNumbr;
        int LastMoveThereWasCapture;
        int LastMoveWherePawnMoved;
        bool WhitePlayerAskedForDraw;
        bool BlackPlayerAskedForDraw;

        string[] boardsMemory;

        public bool IsWhitePlayerAskedForDraw()
        {
            return WhitePlayerAskedForDraw;
        }

        public bool IsBlackPlayerAskedForDraw()
        {
            return BlackPlayerAskedForDraw;
        }

        Piece[,] board;
        public Game() {
            boardsMemory = new string[100];
            this.WhitePlayerAskedForDraw = false;
            this.BlackPlayerAskedForDraw=false;
            InitalizeBoard();
        }

        string ConvertBoardToString(Piece[,] board)
        {
            string currBoardRepresentation ="";
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    currBoardRepresentation += board[i, j] == null ? "Emp" : board[i, j].ToString();
                    currBoardRepresentation += (i == 7 && j == 7) ? "" : ",";
                }
            return currBoardRepresentation;
        }
        void IncreaseMemorySize()
        {
            string[] newMemory = new string[this.boardsMemory.Length + 100];
            for (int i = 0; i < this.boardsMemory.Length; i++) {
                newMemory[i] = this.boardsMemory[i];
            }
            this.boardsMemory = newMemory;
        }

        public string[] GetBoardMemory()
        {
            return this.boardsMemory;
        }

        public int GetMoveNum()
        {
            return this.MoveNumbr;
        }

        public int GetLastMoveThereWasCapture()
        {
            return this.LastMoveThereWasCapture;
        }

        public int GetLastMoveWherePawnmoved()
        {
            return this.LastMoveWherePawnMoved;
        }

        public bool PlayMove(string move, string player)
        {
            //convert verified string input to board indexes
            int column_source = 7 - (72 % ((char)move[0]));
            int column_dest = 7 - (72 % ((char)move[2]));
            int row_source = 7 - (int.Parse(move[1].ToString()) - 1);
            int row_dest = 7 - (int.Parse(move[3].ToString()) - 1);

            if (this.board[row_source, column_source] == null)
                return false;

            return board[row_source, column_source].Move(
                row_source,column_source,row_dest,column_dest, player, board);
        }

        void InitalizeBoard()
        {
            board = new Piece[8, 8];

            //place Rooks
            board[7, 0] = new Rook("white");
            board[7, 7] = new Rook("white");
            board[0, 0] = new Rook("black");
            board[0, 7] = new Rook("black");

            //place Knights
            board[7, 1] = new Knight("white");
            board[7, 6] = new Knight("white");
            board[0, 1] = new Knight("black");
            board[0, 6] = new Knight("black");

            //place Bishops
            board[7, 2] = new Bishop("white");
            board[7, 5] = new Bishop("white");
            board[0, 2] = new Bishop("black");
            board[0, 5] = new Bishop("black");

            //place Queens
            board[7, 3] = new Queen("white");
            board[0, 3] = new Queen("black");

            //place Kings
            board[7, 4] = new King("white");
            board[0, 4] = new King("black");

            //place pawns
            for (int i = 0; i < 8; i++)
            {
                board[1, i] = new Pawn("black");
                board[6, i] = new Pawn("white");
            }

            //place empty tiles
            for (int i = 0; i < 8; i++)
            {
                board[2, i] = null;
                board[3, i] = null;
                board[4, i] = null;
                board[5, i] = null;
            }
        }

        public void PrintBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                Console.Write(8 - i + "  ");
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j]== null)
                        Console.Write("-  ");
                    else Console.Write(board[i, j].ToString() + " ");
                }
                Console.WriteLine();
            }
            Console.Write(" ");
            for (int i = 0; i < 8; i++)
                Console.Write("  " + (char)(65 + i));
            Console.WriteLine();

        }
        
        public void startGame()
        {
            //for debug purposes
            bool debugMode = true;
            string[] InputForDebug = "e2e4;c7c5;g1f3;d7d6;d2d4;c5d4;f3d4;g8f6;b1c3;a7a6;c1g5;e7e6;f2f4;H7H6;G5H4;F8E7;D1F3;B8C6;E1C1;C8B6;D8B6;H8F2;H4F2;B6C7;G2G4;C6D4;F2D4;E6E5;F4E5;D6E5;F3G3;E7D6;D4F2;F6G4;F1B5;A6B5;C3B5;C7C6;B5D6;F7F6;E8E7;G3B3;C8E6;B3B4;E7F6;D6F5;H8C8;C2C3;G4F2;B4E7;F6G6;D1D6;C6E4;D6E6;G6H7;E6H6;F2H3;H7G8;E7F8;C8F8;G5E7;F5E7\r\n".ToUpper().Split(';');
            int indexForDebug = 0;

            bool gameForfited = false;
            string userInput;
            bool whiteTurn = true;
            bool validPieceMove = true;
            bool validGameMove = true;
            bool KingInCheck = false;
            int[] KingPlace;

            PrintBoard();

            while (!gameForfited)
            {
                //check if the player previously played had won the game
                if (checkForWin(whiteTurn ? "black" : "white"))
                {
                    Console.WriteLine((whiteTurn ? "black " : "white ") + "player won!");
                    gameForfited = true;
                }
                if (!gameForfited) //if the player previously played hasnt won the game
                {
                    //we make a copy of the board - so if the player move is not valid we can return to the original state
                    Piece[,] PrevBoard = MakenewCopyOfBoard(board);
                    do
                    {
                        validGameMove = true;
                        if (!debugMode || indexForDebug >= InputForDebug.Length)
                        {   
                                userInput = getUserInput(whiteTurn);
                                if (userInput == "DRAW")
                                {
                                    if (whiteTurn)
                                        this.WhitePlayerAskedForDraw = true;
                                    else this.BlackPlayerAskedForDraw = true;

                                }
                            
                        }      
                        else {
                            Console.WriteLine(InputForDebug[indexForDebug]);
                            userInput = InputForDebug[indexForDebug++];
                        }
                        //if no player asked for draw play the player move
                        if(!this.WhitePlayerAskedForDraw && !this.BlackPlayerAskedForDraw)
                        {
                            //need to check if the king is in check right now. if so,we have to play the next move to protect him. either by blocking or capturing.That mean we need to check if there
                            //a possible to do so . If not the game is in checkmate. if so , we pass check boolean var after move function and check if the new situation is still check
                            KingPlace = getKingPlace(whiteTurn ? "white" : "black");
                            KingInCheck = IsPieceInCaptureDanger(whiteTurn ? "white" : "black", KingPlace[0], KingPlace[1], this.board);
                            validPieceMove = PlayMove(userInput, whiteTurn ? "white" : "black");
                            if (!validPieceMove)
                                Console.WriteLine("invalid move. pleaes try again");
                            else
                            {
                                if (KingInCheck) // king was in check before the player move 
                                {
                                    KingPlace = getKingPlace(whiteTurn ? "white" : "black");
                                    KingInCheck = IsPieceInCaptureDanger(whiteTurn ? "white" : "black", KingPlace[0], KingPlace[1], this.board);
                                    if (isKingCanBeSaved(whiteTurn ? "white" : "black") && KingInCheck) //if the king is still in check and can be saved
                                    {
                                        validGameMove = false;
                                        Console.WriteLine("invalid move.You can save your king and you have to do that. pleaes try again");
                                        board = MakenewCopyOfBoard(PrevBoard);
                                    }
                                }
                                else
                                {
                                    KingPlace = getKingPlace(whiteTurn ? "white" : "black");
                                    KingInCheck = IsPieceInCaptureDanger(whiteTurn ? "white" : "black", KingPlace[0], KingPlace[1], this.board); //king is in check and wasnt in check before the player move
                                    if (KingInCheck)
                                    {
                                        validGameMove = false;
                                        Console.WriteLine("invalid move.You expose your kings to check. pleaes try again");
                                        board = MakenewCopyOfBoard(PrevBoard);
                                    }
                                }
                            }
                        }

                    } while ((!validPieceMove || !validGameMove));

                    //of no player asked for draw check this stats on the board
                    if (!this.BlackPlayerAskedForDraw && !this.WhitePlayerAskedForDraw)
                    {
                        //check if there was capturing or pawn movement
                        int column_source = 7 - (72 % ((char)userInput[0]));
                        int column_dest = 7 - (72 % ((char)userInput[2]));
                        int row_source = 7 - (int.Parse(userInput[1].ToString()) - 1);
                        int row_dest = 7 - (int.Parse(userInput[3].ToString()) - 1);
                        if (PrevBoard[row_source, row_dest] is Pawn) this.LastMoveWherePawnMoved = this.MoveNumbr;
                        if (PrevBoard[row_dest, column_dest] != null && PrevBoard[row_dest, column_dest].getColor() != (whiteTurn ? "white" : "black")) this.LastMoveThereWasCapture = this.MoveNumbr;

                        //save board in memory
                        if (this.boardsMemory[this.boardsMemory.Length - 1] != null) this.IncreaseMemorySize();
                        this.boardsMemory[MoveNumbr] = this.ConvertBoardToString(board);
                        this.MoveNumbr++;
                        PrintBoard();
                    }

                    if (this.WhitePlayerAskedForDraw || this.BlackPlayerAskedForDraw)
                    {
                        bool validInput = false;
                        do
                        {
                            Console.WriteLine("Opponent asked for draw. do you accept ? (Yes/No)");
                            userInput = Console.ReadLine();
                            if (userInput == "Yes")
                            {
                                this.WhitePlayerAskedForDraw = true;
                                this.BlackPlayerAskedForDraw = true;
                                validInput = true;
                            }
                            else if (userInput == "No")
                            {
                                this.WhitePlayerAskedForDraw = false;
                                this.BlackPlayerAskedForDraw = false;
                                validInput = true;
                            }
                            if (!validInput)
                                Console.Write("invalid input.");
                        } while (!validInput);
                    }
                    else
                    {
                        whiteTurn = !whiteTurn;
                    }
                    //check win or draw
                    if (checkForDraw(whiteTurn ? "white" : "black"))
                    {
                        Console.WriteLine("Thats a Draw!");
                        gameForfited = true;
                    }
 
                }
             }      
        }
        
        public int[] getKingPlace(string player)
        {
            //need to check if in the new board scenario , the king is in check 
            int PieceColumn = 0, PieceRow = 0;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (board[i, j] is King && board[i, j].getColor() == player)
                    {
                        PieceColumn = j;
                        PieceRow = i;
                    }
            return [PieceRow, PieceColumn];

        }
        private bool isKingCanBeSaved(string player)
        {
            string opponent = player == "white" ? "black" : "white";
            int[] KingsPlace = getKingPlace(player);
            int KingRow = KingsPlace[0];
            int KingColumn = KingsPlace[1];
            
            //Rooks/Queens
            for (int i = KingRow + 1; i < 8; i++) //check for Rooks/Queens threats in the same column down
            {
                if (board[i, KingColumn] is Piece && board[i, KingColumn].getColor() == player) break;
                if (board[i, KingColumn] is Piece && board[i, KingColumn].getColor() != player && (!(board[i, KingColumn] is Queen || board[i, KingColumn] is Rook))) break;
                if (board[i, KingColumn] is Queen || board[i, KingColumn] is Rook)
                {
                    //if king can move to other tile
                    if ((KingColumn != 0 && board[KingRow, KingColumn - 1] == null && !IsPieceInCaptureDanger(player,KingRow,KingColumn - 1, this.board)) || 
                        (KingColumn != 0 && board[KingRow + 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn - 1, this.board)) || 
                        (KingColumn != 0 && KingRow != 0 && board[KingRow - 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn - 1, this.board))
                        || (KingColumn != 7 && board[KingRow, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow, KingColumn + 1, this.board)) || 
                        (KingColumn != 7 && board[KingRow + 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn + 1, this.board)) || 
                        (KingColumn != 7 && KingRow != 0 && board[KingRow - 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn + 1, this.board)))
                        break;
                    //if this opponent piece can be captured
                    if (IsPieceInCaptureDanger(player, i, KingColumn, this.board)) break;
                    //if current player piece can block the route
                    for (int j = i  - 1 ; j > KingRow; j--)
                    {
                        if (IsPieceInCaptureDanger(opponent, j, KingColumn, this.board))
                            break;
                    }
                    return false;
                }
            }
            for (int i = KingRow - 1; i >= 0; i--) //check for Rooks/Queens threats in the same column up
            {
                if (board[i, KingColumn] is Piece && board[i, KingColumn].getColor() == player) break;
                if (board[i, KingColumn] is Piece && board[i, KingColumn].getColor() != player && (!(board[i, KingColumn] is Queen || board[i, KingColumn] is Rook))) break;
                if (board[i, KingColumn] is Queen || board[i, KingColumn] is Rook)
                {
                    //if king can move to other tile
                    if ((KingColumn != 0 && board[KingRow, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow, KingColumn - 1, this.board)) ||
                        (KingColumn != 0 && board[KingRow + 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn - 1, this.board)) ||
                        (KingColumn != 0 && KingRow != 0 && board[KingRow - 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn - 1, this.board))
                        || (KingColumn != 7 && board[KingRow, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow, KingColumn + 1, this.board)) ||
                        (KingColumn != 7 && board[KingRow + 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn + 1, this.board)) ||
                        (KingColumn != 7 && KingRow != 0 && board[KingRow - 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn + 1, this.board)))
                        break;
                    //if this piece can be captured
                    if (IsPieceInCaptureDanger(opponent, i, KingColumn, this.board)) break;
                    //if other piece can block the route
                    for (int j = i + 1; j < KingRow; j++)
                    {
                        if (IsPieceInCaptureDanger(opponent == "white" ? "black" : "white", j, KingColumn, this.board))
                            break;
                    }
                    return false;
                }
            }
            for (int i = KingColumn + 1; i < 8; i++) //check for Rooks/Queens threats in the same row right
            {
                if (board[KingRow, i] is Piece && board[KingRow, i].getColor() == player) break;
                if (board[KingRow, i] is Piece && board[KingRow, i].getColor() != player && (!(board[KingRow, i] is Queen || board[KingRow, i] is Rook))) break;
                if (board[KingRow, i] is Queen || board[KingRow, i] is Rook)
                {
                    //if king can move to other tile
                    if ((KingColumn != 0 && KingRow != 0 &&  board[KingRow -1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player,KingRow - 1,KingColumn - 1, this.board)) || 
                        (KingColumn != 0 && KingRow !=7 && board[KingRow + 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn - 1, this.board)) || 
                        (KingColumn != 7 && KingRow != 0 && board[KingRow - 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn + 1, this.board))
                        || (KingColumn != 7 && KingRow!=7 && board[KingRow +1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn + 1, this.board)) || 
                        (KingRow != 7 && board[KingRow + 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn, this.board)) || 
                        (KingRow != 0 && board[KingRow - 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn, this.board)))
                        break;
                    //if this piece can be captured
                    if (IsPieceInCaptureDanger(opponent, KingRow, i, this.board)) break;
                    //if other piece can block the route
                    for (int j = i -1 ; j > KingColumn; j--)
                    {
                        if (IsPieceInCaptureDanger(opponent, KingRow, j, this.board))
                            break;
                    }
                    return false;
                }
            }
            for (int i = KingColumn - 1; i >= 0; i--) //check for Rooks/Queens threats in the same row left
            {
                if (board[KingRow, i] is Piece && board[KingRow, i].getColor() == player) break;
                if (board[KingRow, i] is Piece && board[KingRow, i].getColor() != player && (!(board[KingRow, i] is Queen || board[KingRow, i] is Rook))) break;
                if (board[KingRow, i] is Queen || board[KingRow, i] is Rook)
                {
                    //if king can move to other tile
                    if ((KingColumn != 0 && KingRow != 0 && board[KingRow - 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn - 1, this.board)) ||
                        (KingColumn != 0 && KingRow != 7 && board[KingRow + 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn - 1, this.board)) ||
                        (KingColumn != 7 && KingRow != 0 && board[KingRow - 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn + 1, this.board))
                        || (KingColumn != 7 && KingRow != 7 && board[KingRow + 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn + 1, this.board)) ||
                        (KingRow != 7 && board[KingRow + 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn, this.board)) ||
                        (KingRow != 0 && board[KingRow - 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn, this.board)))
                        break;
                    //if this piece can be captured
                    if (IsPieceInCaptureDanger(opponent, KingRow, i, this.board)) break;
                    //if other piece can block the route
                    for (int j = i + 1; j < KingColumn; j++)
                    {
                        if (IsPieceInCaptureDanger(player == "white" ? "black" : "white", KingRow, j, this.board))
                            break;
                    }
                    return false;
                }
            }

            //Queens/Bishops/Pawns

            for (int i = KingRow + 1, j = KingColumn + 1; i < 8 && j < 8; i++, j++) //check for Queens/Bishops threats diagonly right down
            {
                if (board[i, j] is Piece && board[i, j].getColor() == player) break;
                if (board[i, j] is Piece && board[i, j].getColor() == player && (!(board[i, j] is Queen || board[i, j] is Bishop))) break;
                if (board[i, j] is Queen || board[i, j] is Bishop)
                {
                    //if king can move to other tile
                    if ((KingRow !=7 && board[KingRow + 1, KingColumn ] == null && !IsPieceInCaptureDanger(player,KingRow + 1,KingColumn, this.board)) || //down
                        (KingColumn != 0 && KingRow != 7 && board[KingRow + 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn - 1, this.board)) || //down left
                        (KingColumn != 0 && board[KingRow, KingColumn -1] == null && !IsPieceInCaptureDanger(player, KingRow , KingColumn - 1, this.board)) //left
                        || (KingRow != 0 && board[KingRow - 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow -1, KingColumn, this.board)) || //up
                        (KingColumn != 7 && KingRow !=0 && board[KingRow - 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn + 1, this.board)) || // up right
                        (KingColumn != 7 && board[KingRow , KingColumn + 1] == null) && !IsPieceInCaptureDanger(player, KingRow , KingColumn + 1, this.board)) //right
                        break;
                    //if this piece can be captured NOT BY THE KING
                    if (IsPieceInCaptureDanger(opponent, i, j, this.board)) break;
                    //if other piece can block the route
                    for (int m = i -1, n = j - 1 ; m > KingRow && n > KingColumn; m-- , n--)
                    {
                        if (IsPieceInCaptureDanger(opponent, m, n, this.board))
                            break;
                    }
                    return false;
                }
            }
            for (int i = KingRow + 1, j = KingColumn - 1; i < 8 && j >= 0; i++, j--) //check for Bishops threats diagonly left down
            {
                if (board[i, j] is Piece && board[i, j].getColor() == player) break;
                if (board[i, j] is Piece && board[i, j].getColor() == player && (!(board[i, j] is Queen || board[i, j] is Bishop))) break;
                if (board[i, j] is Queen || board[i, j] is Bishop)
                {
                    //if king can move to other tile
                    if ((KingRow != 7 && board[KingRow + 1, KingColumn] == null && !IsPieceInCaptureDanger(player,KingRow + 1,KingColumn, this.board)) || //down
                        (KingColumn != 7 && KingRow != 7 && board[KingRow + 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn + 1, this.board)) || //down right
                        (KingColumn != 0 && board[KingRow, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow , KingColumn - 1, this.board)) //left
                        || (KingRow != 0 && board[KingRow - 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn, this.board)) || //up
                        (KingColumn != 0 && KingRow != 0 && board[KingRow - 1, KingColumn -1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn - 1, this.board)) || // up left
                        (KingColumn != 7 && board[KingRow, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow , KingColumn + 1, this.board))) //right
                        break;
                    //if this piece can be captured
                    if (IsPieceInCaptureDanger(opponent, i, j, this.board)) break;
                    //if other piece can block the route
                    for (int m = i - 1, n = j + 1; m > KingRow && n < KingColumn; m--, n++)
                    {
                        if (IsPieceInCaptureDanger(opponent, m, n, this.board))
                            break;
                    }
                    return false;
                }
            }
            for (int i = KingRow - 1, j = KingColumn + 1; i >= 0 && j < 8; i--, j++) //check for Bishops threats diagonly right up
            {
                if (board[i, j] is Piece && board[i, j].getColor() == player) break;
                if (board[i, j] is Piece && board[i, j].getColor() == player && (!(board[i, j] is Queen || board[i, j] is Bishop))) break;
                if (board[i, j] is Queen || board[i, j] is Bishop)
                {
                    //if king can move to other tile
                    if ((KingRow != 7 && board[KingRow + 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn, this.board)) || //down
                          (KingColumn != 0 && KingRow != 7 && board[KingRow + 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn - 1, this.board)) || //down left
                          (KingColumn != 0 && board[KingRow, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow, KingColumn - 1, this.board)) //left
                          || (KingRow != 0 && board[KingRow - 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn, this.board)) || //up
                          (KingColumn != 7 && KingRow != 0 && board[KingRow - 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn + 1, this.board)) || // up right
                          (KingColumn != 7 && board[KingRow, KingColumn + 1] == null) && !IsPieceInCaptureDanger(player, KingRow, KingColumn + 1, this.board)) //right
                        break;
                    //if this piece can be captured
                    if (IsPieceInCaptureDanger(opponent, i, j, this.board)) break;
                    //if other piece can block the route
                    for (int m = i + 1, n = j - 1; m < KingRow && n > KingColumn; m++, n--)
                    {
                        if (IsPieceInCaptureDanger(opponent , m, n, this.board))
                            break;
                    }
                    return false;
                }
            }
            for (int i = KingRow - 1, j = KingColumn - 1; i >= 0 && j >= 0; i--, j--) //check for Bishops threats diagonly left up
            {
                if (board[i, j] is Piece && board[i, j].getColor() == player) break;
                if (board[i, j] is Piece && board[i, j].getColor() == player && (!(board[i, j] is Queen || board[i, j] is Bishop))) break;
                if (board[i, j] is Queen || board[i, j] is Bishop)
                {
                    //if king can move to other tile
                    if ((KingRow != 7 && board[KingRow + 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn, this.board)) || //down
                         (KingColumn != 7 && KingRow != 7 && board[KingRow + 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn + 1, this.board)) || //down right
                         (KingColumn != 0 && board[KingRow, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow, KingColumn - 1, this.board)) //left
                         || (KingRow != 0 && board[KingRow - 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn, this.board)) || //up
                         (KingColumn != 0 && KingRow != 0 && board[KingRow - 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn - 1, this.board)) || // up left
                         (KingColumn != 7 && board[KingRow, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow, KingColumn + 1, this.board))) //right
                        break;
                    //if this piece can be captured
                    if (IsPieceInCaptureDanger(opponent, i, j, this.board)) break;
                    //if other piece can block the route
                    for (int m = i + 1, n = j + 1; m < KingRow && n < KingColumn; m++, n++)
                    {
                        if (IsPieceInCaptureDanger(opponent, m, n, this.board))
                            break;
                    }
                    return false;
                }
            }

            //no need to check kings threats because king canot capture another king

            //knights threats
            if ((KingRow >= 2 && KingColumn < 7) && board[KingRow - 2, KingColumn + 1] is Knight && (board[KingRow - 2, KingColumn + 1].getColor() != player))
            {
                //if the king canot move one tile
                if (!(((KingRow != 7 && board[KingRow + 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn, this.board)) || //down
                       (KingColumn != 7 && KingRow != 7 && board[KingRow + 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn + 1, this.board)) || //down right
                       (KingColumn != 0 && KingRow != 7 && board[KingRow + 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn - 1, this.board)) || //down left
                       (KingColumn != 0 && board[KingRow, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow, KingColumn - 1, this.board)) //left
                       || (KingRow != 0 && board[KingRow - 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn, this.board)) || //up
                       (KingColumn != 0 && KingRow != 0 && board[KingRow - 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn - 1, this.board)) || // up left
                       (KingColumn != 7 && KingRow != 0 && board[KingRow - 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn + 1, this.board)) || // up right
                       (KingColumn != 7 && board[KingRow, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow, KingColumn + 1, this.board))) //right 
                    || IsPieceInCaptureDanger(opponent, KingRow - 2, KingColumn + 1, this.board))) return false;  //if the knight can be captured
            }
            if ((KingRow >= 2 && KingColumn > 0) && board[KingRow- 2, KingColumn- 1] is Knight && (board[KingRow- 2, KingColumn- 1].getColor() != player))
            {
                //if the king canot move one tile
                if (!(((KingRow != 7 && board[KingRow + 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn, this.board)) || //down
                       (KingColumn != 7 && KingRow != 7 && board[KingRow + 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn + 1, this.board)) || //down right
                       (KingColumn != 0 && KingRow != 7 && board[KingRow + 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn - 1, this.board)) || //down left
                       (KingColumn != 0 && board[KingRow, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow, KingColumn - 1, this.board)) //left
                       || (KingRow != 0 && board[KingRow - 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn, this.board)) || //up
                       (KingColumn != 0 && KingRow != 0 && board[KingRow - 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn - 1, this.board)) || // up left
                       (KingColumn != 7 && KingRow != 0 && board[KingRow - 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn + 1, this.board)) || // up right
                       (KingColumn != 7 && board[KingRow, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow, KingColumn + 1, this.board))) //right 
                    || IsPieceInCaptureDanger(opponent, KingRow - 2, KingColumn - 1, this.board))) return false;  //if the knight can be captured
            }
            if ((KingRow >= 1 && KingColumn < 6) && board[KingRow - 1, KingColumn + 2] is Knight && (board[KingRow - 1, KingColumn + 2].getColor() != player))
            {
                //if the king canot move one tile
                if (!(((KingRow != 7 && board[KingRow + 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn, this.board)) || //down
                       (KingColumn != 7 && KingRow != 7 && board[KingRow + 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn + 1, this.board)) || //down right
                       (KingColumn != 0 && KingRow != 7 && board[KingRow + 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn - 1, this.board)) || //down left
                       (KingColumn != 0 && board[KingRow, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow, KingColumn - 1, this.board)) //left
                       || (KingRow != 0 && board[KingRow - 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn, this.board)) || //up
                       (KingColumn != 0 && KingRow != 0 && board[KingRow - 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn - 1, this.board)) || // up left
                       (KingColumn != 7 && KingRow != 0 && board[KingRow - 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn + 1, this.board)) || // up right
                       (KingColumn != 7 && board[KingRow, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow, KingColumn + 1, this.board))) //right 
                    || IsPieceInCaptureDanger(opponent, KingRow - 1, KingColumn + 2, this.board))) return false;  //if the knight can be captured
            }
            if ((KingRow >= 1 && KingColumn > 1) && board[KingRow - 1, KingColumn - 2] is Knight && (board[KingRow - 1, KingColumn - 2].getColor() != player))
            {
                //if the king canot move one tile
                if (!(((KingRow != 7 && board[KingRow + 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn, this.board)) || //down
                       (KingColumn != 7 && KingRow != 7 && board[KingRow + 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn + 1, this.board)) || //down right
                       (KingColumn != 0 && KingRow != 7 && board[KingRow + 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn - 1, this.board)) || //down left
                       (KingColumn != 0 && board[KingRow, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow, KingColumn - 1, this.board)) //left
                       || (KingRow != 0 && board[KingRow - 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn, this.board)) || //up
                       (KingColumn != 0 && KingRow != 0 && board[KingRow - 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn - 1, this.board)) || // up left
                       (KingColumn != 7 && KingRow != 0 && board[KingRow - 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn + 1, this.board)) || // up right
                       (KingColumn != 7 && board[KingRow, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow, KingColumn + 1, this.board))) //right 
                    || IsPieceInCaptureDanger(opponent, KingRow - 1, KingColumn - 2, this.board))) return false ;  //if the knight can be captured
            }
            if ((KingRow < 7 && KingColumn < 6) && board[KingRow + 1, KingColumn + 2] is Knight && (board[KingRow + 1, KingColumn + 2].getColor() != player))
            {
                //if the king canot move one tile
                if (!(((KingRow != 7 && board[KingRow + 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn, this.board)) || //down
                       (KingColumn != 7 && KingRow != 7 && board[KingRow + 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn + 1, this.board)) || //down right
                       (KingColumn != 0 && KingRow != 7 && board[KingRow + 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn - 1, this.board)) || //down left
                       (KingColumn != 0 && board[KingRow, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow, KingColumn - 1, this.board)) //left
                       || (KingRow != 0 && board[KingRow - 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn, this.board)) || //up
                       (KingColumn != 0 && KingRow != 0 && board[KingRow - 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn - 1, this.board)) || // up left
                       (KingColumn != 7 && KingRow != 0 && board[KingRow - 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn + 1, this.board)) || // up right
                       (KingColumn != 7 && board[KingRow, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow, KingColumn + 1, this.board))) //right 
                    || IsPieceInCaptureDanger(opponent, KingRow + 1, KingColumn + 2, this.board))) return false ;  //if the knight can be captured
            }
            if ((KingRow < 7 && KingColumn > 1) && board[KingRow + 1, KingColumn - 2] is Knight && (board[KingRow + 1, KingColumn - 2].getColor() != player))
            {
                //if the king canot move one tile
                if (!(((KingRow != 7 && board[KingRow + 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn, this.board)) || //down
                       (KingColumn != 7 && KingRow != 7 && board[KingRow + 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn + 1, this.board)) || //down right
                       (KingColumn != 0 && KingRow != 7 && board[KingRow + 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn - 1, this.board)) || //down left
                       (KingColumn != 0 && board[KingRow, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow, KingColumn - 1, this.board)) //left
                       || (KingRow != 0 && board[KingRow - 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn, this.board)) || //up
                       (KingColumn != 0 && KingRow != 0 && board[KingRow - 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn - 1, this.board)) || // up left
                       (KingColumn != 7 && KingRow != 0 && board[KingRow - 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn + 1, this.board)) || // up right
                       (KingColumn != 7 && board[KingRow, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow, KingColumn + 1, this.board))) //right 
                    || IsPieceInCaptureDanger(opponent, KingRow + 1, KingColumn - 2, this.board))) return false;  //if the knight can be captured
            }
            if ((KingRow < 6 && KingColumn < 7) && board[KingRow + 2, KingColumn + 1] is Knight && (board[KingRow + 2, KingColumn + 1].getColor() != player))
            {
                //if the king canot move one tile
                if (!(((KingRow != 7 && board[KingRow + 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn, this.board)) || //down
                       (KingColumn != 7 && KingRow != 7 && board[KingRow + 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn + 1, this.board)) || //down right
                       (KingColumn != 0 && KingRow != 7 && board[KingRow + 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn - 1, this.board)) || //down left
                       (KingColumn != 0 && board[KingRow, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow, KingColumn - 1, this.board)) //left
                       || (KingRow != 0 && board[KingRow - 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn, this.board)) || //up
                       (KingColumn != 0 && KingRow != 0 && board[KingRow - 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn - 1, this.board)) || // up left
                       (KingColumn != 7 && KingRow != 0 && board[KingRow - 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn + 1, this.board)) || // up right
                       (KingColumn != 7 && board[KingRow, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow, KingColumn + 1, this.board))) //right 
                    || IsPieceInCaptureDanger(opponent, KingRow + 2, KingColumn + 1, this.board))) return false;  //if the knight can be captured
            }
            if ((KingRow < 6 && KingColumn > 0) && board[KingRow + 2, KingColumn - 1] is Knight && (board[KingRow + 2, KingColumn - 1].getColor() != player))
            {
                //if the king canot move one tile
                if (!((
                       (KingRow != 7 && board[KingRow + 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn, this.board)) || //down
                       (KingColumn != 7 && KingRow != 7 && board[KingRow + 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn + 1, this.board)) || //down right
                       (KingColumn != 0 && KingRow != 7 && board[KingRow + 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow + 1, KingColumn - 1, this.board)) || //down left
                       (KingColumn != 0 && board[KingRow, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow, KingColumn - 1, this.board)) //left
                       || (KingRow != 0 && board[KingRow - 1, KingColumn] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn, this.board)) || //up
                       (KingColumn != 0 && KingRow != 0 && board[KingRow - 1, KingColumn - 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn - 1, this.board)) || // up left
                       (KingColumn != 7 && KingRow != 0 && board[KingRow - 1, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow - 1, KingColumn + 1, this.board)) || // up right
                       (KingColumn != 7 && board[KingRow, KingColumn + 1] == null && !IsPieceInCaptureDanger(player, KingRow, KingColumn + 1, this.board))) //right 
                    || IsPieceInCaptureDanger(opponent, KingRow + 2, KingColumn - 1, this.board))) return false;  //if the knight can be captured
            }

            return true;
        }

        public Piece[,] MakenewCopyOfBoard(Piece[,] board)
        {
            //make copy of the board by values and not by reference 
            Piece[,] newBoard = new Piece[8, 8];
            for(int i = 0; i < 8; i++)
                for(int j=0;j<8; j++)
                    newBoard[i,j] = board[i,j];
            return newBoard;
        }

        public bool IsPieceInCaptureDanger(string player, int PieceRow,int PieceColumn, Piece[,] board)
        {          
            for (int i = PieceRow +1 ; i < 8; i++) //check for Rooks/Queens threats in the same column down
            {
                if (board[i, PieceColumn] is Piece && board[i, PieceColumn].getColor() == player ) break;
                if (board[i, PieceColumn] is Queen || board[i, PieceColumn] is Rook ) return true;
                if (board[i, PieceColumn] != null) break;
            }
            for (int i = PieceRow - 1; i >= 0; i--) //check for Rooks/Queens threats in the same column up
            {
                if (board[i, PieceColumn] is Piece && board[i, PieceColumn].getColor() == player ) break;
                if (board[i, PieceColumn] is Queen || board[i, PieceColumn] is Rook ) return true;
                if (board[i, PieceColumn] != null) break;
            }
            for (int i = PieceColumn + 1; i < 8; i++) //check for Rooks/Queens threats in the same row to the right
            {
                if (board[PieceRow, i] is Piece && board[PieceRow, i].getColor() == player) break;
                if (board[PieceRow, i] is Queen || board[PieceRow, i] is Rook ) return true;
                if (board[PieceRow,i] != null) break;
            }

            for (int i = PieceColumn - 1; i >= 0; i--) //check for Rooks/Queens threats in the same row to the left
            {
                if (board[PieceRow, i] is Piece && board[PieceRow, i].getColor() == player) break;
                if (board[PieceRow, i] is Queen || board[PieceRow, i] is Rook ) return true;
                if (board[PieceRow, i] != null) break;
            }

            for (int i = PieceRow + 1 , j =PieceColumn + 1; i < 8 && j < 8; i++,j++) //check for Queens/Bishops threats diagonly right down
            {
                if (board[i, j] is Piece && board[i, j].getColor() == player ) break;
                if (board[i, j] is Queen || board[i, j] is Bishop ) return true;
                if (board[i, j] != null) break;
            }

            for (int i = PieceRow + 1, j = PieceColumn - 1; i < 8 && j > 0; i++, j--) //check for Bishops threats diagonly left down
            {
                if (board[i, j] is Piece && board[i, j].getColor() == player ) break;
                if (board[i, j] is Queen || board[i, j] is Bishop ) return true;
                if (board[i, j] != null) break;
            }

            for (int i = PieceRow-1 , j = PieceColumn + 1; i >= 0  && j < 8; i--, j++) //check for Bishops threats diagonly right up
            {
                if (board[i, j] is Piece && board[i, j].getColor() == player ) break;
                if (board[i, j] is Queen || board[i, j] is Bishop  ) return true;
                if (board[i, j] != null) break;
            }

            for (int i = PieceRow - 1, j = PieceColumn - 1; i >= 0 && j > 0; i--, j--) //check for Bishops threats diagonly left up
            {
                if (board[i, j] is Piece && board[i, j].getColor() == player ) break;
                if (board[i, j] is Queen || board[i, j] is Bishop  ) return true;
                if (board[i, j] != null) break;
            }

            //Pawns threats
            if(player == "black")
            {
                if (PieceRow != 7 && PieceColumn != 0 && board[PieceRow + 1, PieceColumn - 1] is Pawn && board[PieceRow + 1, PieceColumn - 1].getColor() != player) return true;
                if (PieceRow != 7 && PieceColumn != 7 && board[PieceRow + 1, PieceColumn + 1] is Pawn && board[PieceRow + 1, PieceColumn + 1].getColor() != player) return true;
            }
            else
            {
                if (PieceRow != 0 && PieceColumn != 0 && board[PieceRow - 1, PieceColumn - 1] is Pawn && board[PieceRow - 1, PieceColumn - 1].getColor() != player) return true;
                if (PieceRow != 0 && PieceColumn != 7 && board[PieceRow -1 , PieceColumn + 1] is Pawn && board[PieceRow - 1, PieceColumn + 1].getColor() != player) return true;
            }

       
            ////Kings threats
            //int[] KingPlace = getKingPlace(player);
            //if(PieceRow != KingPlace[0] || PieceColumn != KingPlace[1]) //if the piece isnt a king
            //{
            //    if (PieceRow != 0 && PieceColumn != 0 && board[PieceRow - 1, PieceColumn - 1] is King && board[PieceRow - 1, PieceColumn - 1].getColor() != player) return true; // up left
            //    if (PieceRow != 0 && board[PieceRow - 1, PieceColumn ] is King && board[PieceRow - 1, PieceColumn].getColor() != player) return true; // up left
            //    if (PieceRow != 0 && PieceColumn != 7 && board[PieceRow - 1, PieceColumn + 1] is King && board[PieceRow - 1, PieceColumn + 1].getColor() != player) return true; // up right
            //    if (PieceColumn != 7 && board[PieceRow , PieceColumn + 1] is King && board[PieceRow, PieceColumn + 1].getColor() != player) return true; // right
            //    if (PieceRow != 7 && PieceColumn != 7 && board[PieceRow + 1, PieceColumn + 1] is King && board[PieceRow + 1, PieceColumn + 1].getColor() != player) return true; // down right
            //    if (PieceRow != 7 && board[PieceRow + 1, PieceColumn] is King && board[PieceRow + 1, PieceColumn].getColor() != player) return true; // down
            //    if (PieceRow != 7 && PieceColumn != 0 && board[PieceRow + 1, PieceColumn - 1] is King && board[PieceRow + 1, PieceColumn - 1].getColor() != player) return true; // down left
            //    if (PieceColumn != 0 && board[PieceRow, PieceColumn - 1] is King && board[PieceRow, PieceColumn - 1].getColor() != player) return true; // left
            //}

            //knights threats
            if ((PieceRow >= 2 && PieceColumn < 7) && board[PieceRow - 2, PieceColumn + 1] is Knight && (board[PieceRow - 2, PieceColumn + 1].getColor() != player)) return true;
            if ((PieceRow >= 2 && PieceColumn > 0) && board[PieceRow - 2, PieceColumn - 1] is Knight && (board[PieceRow - 2, PieceColumn - 1].getColor() != player)) return true;
            if ((PieceRow >= 1 && PieceColumn < 6) && board[PieceRow - 1, PieceColumn + 2] is Knight && (board[PieceRow - 1, PieceColumn + 2].getColor() != player)) return true;
            if ((PieceRow >= 1 && PieceColumn > 1) && board[PieceRow - 1, PieceColumn - 2] is Knight && (board[PieceRow - 1, PieceColumn - 2].getColor() != player)) return true;
            if ((PieceRow < 7 && PieceColumn <6) && board[PieceRow + 1, PieceColumn + 2] is Knight && (board[PieceRow + 1, PieceColumn + 2].getColor() != player)) return true;
            if ((PieceRow < 7 && PieceColumn  >1) && board[PieceRow + 1, PieceColumn -2] is Knight && (board[PieceRow + 1, PieceColumn - 2].getColor() != player)) return true;
            if ((PieceRow < 6 && PieceColumn < 7) && board[PieceRow  +2, PieceColumn + 1] is Knight && (board[PieceRow + 2, PieceColumn + 1].getColor() != player)) return true;
            if ((PieceRow <6 && PieceColumn > 0) && board[PieceRow + 2, PieceColumn - 1] is Knight && (board[PieceRow + 2, PieceColumn - 1].getColor() != player)) return true;

            return false;
        }

        bool checkForWin(string player)
        {
            string opponent = player == "white" ? "black" : "white";
            int[] KingPlace = getKingPlace(opponent);
            //if the opponent king is in check and the king canot be save - its a checkmate 
            if (IsPieceInCaptureDanger(opponent, KingPlace[0], KingPlace[1], this.board) && !isKingCanBeSaved(opponent)) return true;
            return false;
        }

        bool checkForDraw(string player)
        {
            IDraw[] draws = new IDraw[5];
            draws[0] = new ThreeFoldRepetitionDraw();
            draws[1] = new StalemateDraw();
            draws[2] = new PlayerRequestDraw();
            draws[3] = new DeadPositionDraw();
            draws[4] = new FifhtyMoveRuleDraw();
            foreach (var draw in draws)
                if (draw.IsDraw(this.board, this,player))
                    return true;
            return false;
        }

        string getUserInput(bool Whiteturn)
        {
            bool validInput = false;
            string input = "";
            while (!validInput)
            {
                Console.WriteLine((Whiteturn ? "White " : "Black ") + "Turn , enter your move , Or Ask for a draw (Type DRAW)");
                input = Console.ReadLine().Trim().ToUpper();
                if (input == "DRAW") return "DRAW";
                if (input.Length == 4 && ((int)input[0] >= 65 && (int)input[0] <= 72) && (int.Parse(input[1].ToString()) <= 8 && int.Parse(input[1].ToString()) >= 1) &&
                    ((int)input[2] >= 65 && (int)input[2] <= 72) && (int.Parse(input[3].ToString()) <= 8 && int.Parse(input[3].ToString()) >= 1))
                    validInput = true;
                else Console.Write("Invalid input. ");
            }
            return input;
        }
    }
}
