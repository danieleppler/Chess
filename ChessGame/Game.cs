using System.ComponentModel.Design;
using System.Numerics;
using System.Runtime.CompilerServices;

using ChessGame.Pieces;

namespace ChessGame
{
    public class Game
    {
        int MoveNumbr;
        int LastMoveThereWasCapture;
        int LastMoveWherePawnMoved;
        bool whiteTurn = true;
        string[] boardsMemory;
        bool gameForfited = false;


        Piece[,] board;
        public Game()
        {
            boardsMemory = new string[100];
            InitalizeBoard();
        }

        string ConvertBoardToString(Piece[,] board)
        {
            string currBoardRepresentation = "";
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
            for (int i = 0; i < this.boardsMemory.Length; i++)
            {
                newMemory[i] = this.boardsMemory[i];
            }
            this.boardsMemory = newMemory;
        }

 
     
        public bool IsLegalPieceMove(string move, string player)
        {
            //convert verified string input to board indexes
            int column_source = 7 - (72 % ((char)move[0]));
            int column_dest = 7 - (72 % ((char)move[2]));
            int row_source = 7 - (int.Parse(move[1].ToString()) - 1);
            int row_dest = 7 - (int.Parse(move[3].ToString()) - 1);
            return board[row_source, column_source].IsLegalMove(
                new BoardLocation(row_source, column_source), new BoardLocation(row_dest, column_dest), player, board);
        }

        void InitalizeBoard()
        {
            board = new Piece[8, 8];
            ////place Rooks
            board[7, 0] = new Rook("white");
            board[7, 7] = new Rook("white");
            board[0, 0] = new Rook("black");
            board[0, 7] = new Rook("black");
            ////place Knights
            board[7, 1] = new Knight("white");
            board[7, 6] = new Knight("white");
            board[0, 1] = new Knight("black");
            board[0, 6] = new Knight("black");
            ////place Bishops
            board[7, 2] = new Bishop("white");
            board[7, 5] = new Bishop("white");
            board[0, 2] = new Bishop("black");
            board[0, 5] = new Bishop("black");
            ////place Queens
            board[7, 3] = new Queen("white");
            board[0, 3] = new Queen("black");
            //place Kings
            board[7, 4] = new King("white");
            board[0, 4] = new King("black");
            ////place pawns and empty pieces
            for (int i = 0; i < 8; i++)
            {
                board[1, i] = new Pawn("black");
                board[2, i] = new EmptyPiece();
                board[3, i] = new EmptyPiece();
                board[4, i] = new EmptyPiece();
                board[5, i] = new EmptyPiece();
                board[6, i] = new Pawn("white");
            }
        }

        public void PrintBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                Console.Write(8 - i + "  ");
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] == null)
                        Console.Write("-  ");
                    else Console.Write(board[i, j].ToString() + " ");
                }
                Console.WriteLine();
            }
            Console.Write(" ");
            for (int i = 0; i < 8; i++)
                Console.Write("  " + (char)(65 + i));
            Console.WriteLine();
            Console.WriteLine("===========================");
        }

        public void startAndPlayGame()
        {
            PrintBoard();
            while (!gameForfited)
            {
                PlayTurn();
                if (IsCheckMate(whiteTurn ? "white" : "black", whiteTurn ? "black" : "white", getKingLocation(whiteTurn ? "black" : "white")))
                {
                    gameForfited = true;
                    Console.WriteLine("Check Mate for "+ (whiteTurn?"white":"black")+" player");
                }
                else if (checkForDraw(whiteTurn ? "white" : "black"))
                {
                    Console.WriteLine("Thats a Draw!");
                    gameForfited = true;                  
                }
                whiteTurn = !whiteTurn;
            }
        }

        void PlayTurn()
        {
            bool IsDrawRequested = false;
            bool debugMode = true;
            string[] InputForDebug = "E2E3;A7A5;D1H5;A8A6;H5A5;H7H5;A5C7;A6H6;H2H4;F7F6;C7D7;E8F7;D7B7;D8D3;B7B8;D3H7;B8C8;F7G6;C8E6".ToUpper().Split(';');
            int indexForDebug = 0;
            bool validGameMove = false;
            //for debug purposes
            string userInput;
            do
            {
                validGameMove = false;
                if (debugMode && indexForDebug < InputForDebug.Length)
                {
                    Console.WriteLine(InputForDebug[indexForDebug]);
                    userInput = InputForDebug[indexForDebug++];
                }
                else
                {
                    userInput = getUserInput(whiteTurn);
                    if (userInput == "DRAW" && IsBothPlayersAgreedOnDraw(whiteTurn))
                    {
                        gameForfited = true;
                        Console.WriteLine("Both players agreed on draw!");
                        IsDrawRequested = true;
                    }
                }
                if (!IsDrawRequested)
                {
                    Piece[,] PrevBoard = GetCopyOfBoard(board);
                    if (!IsLegalPieceMove(userInput, whiteTurn ? "white" : "black"))
                        Console.WriteLine("invalid move. pleaes try again");
                    else
                    {
                        MovePiece(new BoardLocation(7 - (int.Parse(userInput[1].ToString()) - 1), 7 - (72 % ((char)userInput[0]))),
                            new BoardLocation(7 - (int.Parse(userInput[3].ToString()) - 1), 7 - (72 % ((char)userInput[2]))));
                        if (IsCheck(whiteTurn ? "black" : "white", getKingLocation(whiteTurn ? "white" : "black")))
                        {
                            Console.WriteLine("invalid move.You leave your king in check. pleaes try again");
                            board = GetCopyOfBoard(PrevBoard);
                        }
                        else
                        {
                            ExecuteCommandsAfterMoveSucceded(PrevBoard, new BoardLocation(7 - (int.Parse(userInput[1].ToString()) - 1), 7 - (72 % ((char)userInput[0]))),
                                new BoardLocation(7 - (int.Parse(userInput[3].ToString()) - 1), 7 - (72 % ((char)userInput[2]))));
                            validGameMove = true;
                        }
                    }
                }
            }
            while (!validGameMove);
        }

        void ExecuteCommandsAfterMoveSucceded(Piece[,] PrevBoard,BoardLocation source, BoardLocation dest)
        {
            if (PrevBoard[dest.row, source.col] is Pawn) this.LastMoveWherePawnMoved = this.MoveNumbr;
            if (!(PrevBoard[dest.row, dest.col] is EmptyPiece) &&
                PrevBoard[dest.row, dest.col].getColor() != (whiteTurn ? "white" : "black")) this.LastMoveThereWasCapture = this.MoveNumbr;
            if ((dest.row == 1 || dest.row == 8) && board[source.row, source.col] is Pawn)
                PromotePawn(new BoardLocation(dest.row, dest.col),
                    whiteTurn ? "whIte" : "black");

            //save board in memory
            if (this.boardsMemory[this.boardsMemory.Length - 1] != null) this.IncreaseMemorySize();
            this.boardsMemory[MoveNumbr] = this.ConvertBoardToString(board);
            this.MoveNumbr++;
            PrintBoard();
        }
        bool IsBothPlayersAgreedOnDraw(bool whiteTurn)
        {
            bool validInput = false;
            string userInput;
            do
            {
                Console.WriteLine((whiteTurn ? "White" : "Black") + " player , Opponent asked for draw. do you accept ? (Yes/No)");
                userInput = Console.ReadLine();
                if (userInput == "Yes")
                    return true;
                else if (userInput == "No")
                    return false;
                if (!validInput)
                    Console.Write("invalid input.");
            } while (!validInput);
            return false;
        }

        public BoardLocation getKingLocation(string player)
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
            return new BoardLocation(PieceRow, PieceColumn);

        }
        bool IsCheckMate(string ThreateningPlayer, string KingColor, BoardLocation ThreatenedKingLocation)
        {
            BoardLocation currKingLocation;
            bool isMate = false;
            if (!IsCheck(ThreateningPlayer, ThreatenedKingLocation))
                return false;
            for (int row = 0; row < 8; row++)
                for (int col = 0; col < 8; col++)
                    for (int toRow = 0; toRow < 8; toRow++)
                        for (int toColumn = 0; toColumn < 8; toColumn++)
                            if (board[row, col].IsLegalMove(new BoardLocation(row, col), new BoardLocation(toRow, toColumn), KingColor, board))
                            {
                                Piece[,] prevBoard = GetCopyOfBoard(board);
                                if (board[row, col] is King)
                                    currKingLocation = new BoardLocation(toRow, toColumn);
                                else currKingLocation = ThreatenedKingLocation;
                                MovePiece(new BoardLocation(row, col), new BoardLocation(toRow, toColumn));
                                isMate = IsCheck(ThreateningPlayer, currKingLocation);
                                board = GetCopyOfBoard(prevBoard); //undo move
                                if (!isMate) 
                                    return false;
                            }
            return true;
        }

        void MovePiece(BoardLocation source,BoardLocation destination)
        {
            if (board[source.row, source.col] is King)
            {
                King currKing = board[source.row, source.col] as King;
                if (currKing.isMovingIsCastling(source, destination, whiteTurn ? "white" : "black", board))
                {
                    if (source.col > destination.col)
                    {
                        board[source.row, source.col - 1] = new Rook(whiteTurn ? "white" : "black");
                        board[source.row, 0] = new EmptyPiece();
                    }
                    else
                    {
                        board[source.row, source.col + 1] = new Rook(whiteTurn ? "white" : "black");
                        board[source.row, 7] = new EmptyPiece();
                    }
                }
                currKing.moveNumber++;
            }
              
            board[destination.row, destination.col] = board[source.row, source.col];
            board[source.row, source.col] = new EmptyPiece();
            if (board[destination.row, destination.col] is Pawn)
                ((Pawn)board[destination.row, destination.col]).moveNumber++;
            if (board[destination.row, destination.col] is Rook)
                ((Rook)board[destination.row, destination.col]).moveNumber++;
           
        }

        void PromotePawn(BoardLocation PawnLocation,string player)
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
                        board[PawnLocation.row, PawnLocation.col] = new Rook(player);
                        break;

                    case "Bishop":
                        board[PawnLocation.row, PawnLocation.col] = new Bishop(player);
                        break;

                    case "Knight":
                        board[PawnLocation.row, PawnLocation.col] = new Knight(player);
                        break;

                    case "Queen":
                        board[PawnLocation.row, PawnLocation.col] = new Queen(player);
                        break;

                    default:
                        validInput = false;
                        Console.Write("Invalid Input ! ");
                        break;
                }

            } while (!validInput);
        }
        public Piece[,] GetCopyOfBoard(Piece[,] board)
        {
            //make copy of the board by values and not by reference 
            Piece[,] newBoard = new Piece[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    newBoard[i, j] = board[i, j];
            return newBoard;
        }

        public bool IsCheck(string ThreateningPlayer, BoardLocation ThreatenedKingLocation)
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (board[i, j].IsLegalMove(new BoardLocation(i, j), ThreatenedKingLocation, ThreateningPlayer, board))
                        return true;
            return false;
        }


        bool checkForDraw(string player)
        {
            if (IsStaleMate(player) || IsThreeFoldRepetitionDraw() || IsFifhtyMoveRuleDraw() ||IsDeadPositionDrawKingVsKing() || IsDeadPositionDrawKingVsKingAndBishop())
                return true;
            return false;
        }

        private bool IsStaleMate(string ThreateningPlayer)
        {
            BoardLocation currKingLocation = getKingLocation(ThreateningPlayer == "white" ? "black":"white");
            bool IsStaleMate = false;
            if (IsCheck(ThreateningPlayer, currKingLocation))
                return false;
            for (int row = 0; row < 8; row++)
                for (int col = 0; col < 8; col++)
                    for (int toRow = 0; toRow < 8; toRow++)
                        for (int toColumn = 0; toColumn < 8; toColumn++)
                            if (board[row, col].IsLegalMove(new BoardLocation(row, col), new BoardLocation(toRow, toColumn), ThreateningPlayer == "white" ? "black" : "white", board))
                            {
                                Piece[,] prevBoard = GetCopyOfBoard(board);
                                if (board[row, col] is King)
                                    currKingLocation = new BoardLocation(toRow, toColumn);
                                else currKingLocation = currKingLocation;
                                MovePiece(new BoardLocation(row, col), new BoardLocation(toRow, toColumn));
                                IsStaleMate = IsCheck(ThreateningPlayer, currKingLocation);
                                board = GetCopyOfBoard(prevBoard); //undo move
                                if (!IsStaleMate)
                                    return false;
                            }
            Console.WriteLine("Stalemate - ");
            return true;
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
                if (input.Length == 4 && ((int)input[1] >= 65 && (int)input[1] <= 89 || (int)input[3] >= 65 && (int)input[3] <= 89))
                {
                    Console.WriteLine("invalid input.");
                }
                else
                {
                    if (input.Length == 4 && ((int)input[0] >= 65 && (int)input[0] <= 72) && (int.Parse(input[1].ToString()) <= 8 && int.Parse(input[1].ToString()) >= 1) &&
                                       ((int)input[2] >= 65 && (int)input[2] <= 72) && (int.Parse(input[3].ToString()) <= 8 && int.Parse(input[3].ToString()) >= 1))
                        validInput = true;
                    else Console.Write("Invalid input. ");
                }

            }
            return input;
        }
        public bool IsThreeFoldRepetitionDraw()
        {
            for (int i = 0; i < boardsMemory.Length && boardsMemory[i] != null; i++)
            {
                int BoardCount = 1;
                for (int j = i + 1; j < boardsMemory.Length && boardsMemory[j] != null; j++)
                {
                    if (boardsMemory[j] == boardsMemory[i])
                        BoardCount++;
                    if (BoardCount == 3)
                    {
                        Console.Write("Three fold repetition - ");
                        return true;
                    }

                }
            }
            return false;
        }

        public bool IsFifhtyMoveRuleDraw()
        {
            if (LastMoveThereWasCapture <= MoveNumbr - 100 &&
                LastMoveThereWasCapture <= MoveNumbr - 100)
            {
                Console.Write("Fifthy move rule - ");
                return true;
            }
            return false;
        }
        public bool IsDeadPositionDrawKingVsKing()
        {
            //king vs king
            bool kingVsKing = true;
            for (int i = 0; i < 7 && kingVsKing; i++)
                for (int j = 0; j < 7; j++)
                    if (board[i, j] != null && !(board[i, j] is King) && kingVsKing)
                        return false;
            Console.Write("Dead Position - ");
            return true;
         }
        public bool IsDeadPositionDrawKingVsKingAndBishop()
        {
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
            if (KingVsKingAndBishop)
            {
                Console.Write("Dead position - ");
                return true;
            }
            return false;
        }
    }
}
