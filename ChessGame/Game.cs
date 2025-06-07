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
        bool WhitePlayerAskedForDraw;
        bool BlackPlayerAskedForDraw;
        bool whiteTurn = true;
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
        public Game()
        {
            boardsMemory = new string[100];
            this.WhitePlayerAskedForDraw = false;
            this.BlackPlayerAskedForDraw = false;
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

        public void startGame()
        {
            //for debug purposes
            bool debugMode = true;
            string[] InputForDebug = "E2E4;C7C6;D2D4;D7D5;B1C3;D5E4;C3E4;C8F5;E4G3;F5G6;H2H4;H7H6;G1F3;B8D7;H4H5;G6H7;F1D3;H7D3;D1D3;E7E6;C3F4;C1F4;F8B4;C2C3;B4E7;E1B1;E1C1;G8F6;C1B1;E8G8;F3E5;C6C5;D3F3;D8B6;E5D7;F6D7;D4D5;E6D5;G3F5;E7F6;D1D5;B6E6;F4H6;F7E5;D7E5;F3E4;E5C6;E4F3;C6E5;F3E4;E5C6;E4G4;E6D5;H6G7;D5D3;B1A1;C6E5;F5E7;F8E8;G8H8;G8H7;G4G6;F7G6;H5G6;H7G7;H1H7\r\n".ToUpper().Split(';');
            int indexForDebug = 0;
            bool gameForfited = false;
            string userInput;

            bool IsDraw = false;
            bool validGameMove = false;
            while (!gameForfited)
            {
                PrintBoard();
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
                            IsDraw = true;
                        }
                    }
                    if (!IsDraw)
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
                            else validGameMove = true;
                        }
                    }
                }
                while (!validGameMove);
                if ((userInput[3].ToString() == "1"|| userInput[3].ToString() == "8") && board[7 - (int.Parse(userInput[3].ToString()) - 1), 7 - (72 % ((char)userInput[2]))] is Pawn)
                    PromotePawn(new BoardLocation(7 - (int.Parse(userInput[3].ToString()) - 1), 7 - (72 % ((char)userInput[2]))), 
                        whiteTurn?"whIte":"black");
                if (IsCheckMate(whiteTurn ? "white" : "black", whiteTurn ? "black" : "white", getKingLocation(whiteTurn ? "black" : "white")))
                {
                    PrintBoard();
                    gameForfited = true;
                    Console.WriteLine("Check Mate for "+ (whiteTurn?"white":"black")+" player");
                }
                else if (checkForDraw(whiteTurn ? "white" : "black"))
                {
                    PrintBoard();
                    Console.WriteLine("Thats a Draw!");
                    gameForfited = true;                  
                }
                whiteTurn = !whiteTurn;
            }
        }

        private bool IsBothPlayersAgreedOnDraw(bool whiteTurn)
        {
            bool validInput = false;
            string userInput;
            do
            {
                Console.WriteLine((this.BlackPlayerAskedForDraw ? "White" : "Black") + " player , Opponent asked for draw. do you accept ? (Yes/No)");
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
            if (board[destination.row, destination.col] is King)
            {
                King currKing = board[destination.row, destination.col] as King;
                currKing.moveNumber++;
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
            ////The idea of making the draws implement an interface and putting them in another files, is that if we want to add another draw 
            ////option we can just add the file , make hime implement the interface and add this item to the array. I am aware that there is some
            ////major logic that had been implemened in the Game class that had been needed in ThreeFoldRepetition.cs and FifhyMoveRuleDraw.cs and 
            ////it might miss the point a little , but i deicded to keep the implentation like this.
            //IDraw[] draws = new IDraw[5];
            //draws[0] = new ThreeFoldRepetitionDraw();
            //draws[1] = new StalemateDraw();
            //draws[2] = new PlayerRequestDraw();
            //draws[3] = new DeadPositionDraw();
            //draws[4] = new FifhtyMoveRuleDraw();
            //foreach (var draw in draws)
            //    if (draw.IsDraw(this.board, this, player))
            //        return true;
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
    }
}
