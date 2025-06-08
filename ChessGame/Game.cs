using System.ComponentModel.Design;
using System.Numerics;
using System.Runtime.CompilerServices;

using ChessGame.Pieces;

namespace ChessGame
{
    public class Game
    {
        int MoveNumbr ;
        int LastMoveThereWasCapture;
        int LastMoveWherePawnMoved;
        bool IswhiteTurn = true;
        string[] boardsMemory;
        bool IsGameForfited = false;
        string[] InputForDebug = "E2E4;E7E5;D2D4;E5D4;D1D4;D7D6;F1C4;C8E6;C4E6;F7E6;D4G7;H8F8;G7F8".ToUpper().Split(';');
        int indexForDebug = 0;
        bool IsDebugMode = false;

        Piece[,] board;
        public Game()
        {
            boardsMemory = new string[100];
            InitalizeBoard();
        }

        string ConvertBoardToString(Piece[,] board)
        {//convert the board to string representation so we can store it in the boards memory array.
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
        {//if we need to increase the memory size, increase it by more 100 places
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
            board[7, 0] = new Rook("white");////place Rooks
            board[7, 7] = new Rook("white");
            board[0, 0] = new Rook("black");
            board[0, 7] = new Rook("black");
            board[7, 1] = new Knight("white");////place Knights
            board[7, 6] = new Knight("white");
            board[0, 1] = new Knight("black");
            board[0, 6] = new Knight("black");
            board[7, 2] = new Bishop("white");////place Bishops
            board[7, 5] = new Bishop("white");
            board[0, 2] = new Bishop("black");
            board[0, 5] = new Bishop("black");
            board[7, 3] = new Queen("white");////place Queens
            board[0, 3] = new Queen("black");
            board[7, 4] = new King("white");//place Kings
            board[0, 4] = new King("black");
            for (int i = 0; i < 8; i++)////place pawns and empty pieces
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
            PrintBoard(); //initial printing
            while (!IsGameForfited)
            {
                PlayTurn(); //get user input, and execute the move if its legal
                if (IsCheckMate(IswhiteTurn ? "white" : "black", IswhiteTurn ? "black" : "white", getKingLocation(IswhiteTurn ? "black" : "white"))) //check for checkmate
                {
                    IsGameForfited = true;
                    Console.WriteLine("Check Mate for "+ (IswhiteTurn?"white":"black")+" player");
                }
                else if (checkForDraw(IswhiteTurn ? "white" : "black")) //check for draw
                {
                    Console.WriteLine("Thats a Draw!");
                    IsGameForfited = true;                  
                }
                IswhiteTurn = !IswhiteTurn; // change player turn
            }
        }
        void PlayTurn()
        {
            bool IsDrawRequested;        
            bool validGameMove;
            string userInput;
            do
            {
                IsDrawRequested = false;
                validGameMove = false;
                if (IsDebugMode && indexForDebug < InputForDebug.Length) // for debug purposes
                {
                    Console.WriteLine(InputForDebug[indexForDebug]);
                    userInput = InputForDebug[indexForDebug++];
                }
                else
                {
                    userInput = getUserInput(IswhiteTurn);
                    if(userInput == "DRAW")
                    {
                        IsDrawRequested = true;
                        if (IsBothPlayersAgreedOnDraw(IswhiteTurn)) //check if both players agreed for draw
                        {
                            IsGameForfited = true;
                            Console.WriteLine("Both players agreed on draw!");
                        }
                    }
                }
                if (!IsDrawRequested) 
                {
                    Piece[,] PrevBoard = GetCopyOfBoard(board); // in case we need to undo a invalid move
                    if (!IsLegalPieceMove(userInput, IswhiteTurn ? "white" : "black")) //if its a legal piece move. each piece have its own login
                        Console.WriteLine("invalid move. pleaes try again");
                    else
                    {
                        MovePiece(new BoardLocation(7 - (int.Parse(userInput[1].ToString()) - 1), 7 - (72 % ((char)userInput[0]))),
                            new BoardLocation(7 - (int.Parse(userInput[3].ToString()) - 1), 7 - (72 % ((char)userInput[2]))));
                        if (IsCheck(IswhiteTurn ? "black" : "white", getKingLocation(IswhiteTurn ? "white" : "black"))) //if the move made the piece be in check , its invalid
                        {
                            Console.WriteLine("invalid move.You leave your king in check. pleaes try again");
                            board = GetCopyOfBoard(PrevBoard);
                        }
                        else
                        {
                            ExecuteCommandsAfterMoveSucceded(PrevBoard, new BoardLocation(7 - (int.Parse(userInput[1].ToString()) - 1), 7 - (72 % ((char)userInput[0]))),
                                new BoardLocation(7 - (int.Parse(userInput[3].ToString()) - 1), 7 - (72 % ((char)userInput[2]))));
                            validGameMove = true; //valid move
                        }
                    }
                }
            }
            while (!validGameMove && !IsGameForfited);
        }
        void ExecuteCommandsAfterMoveSucceded(Piece[,] PrevBoard,BoardLocation source, BoardLocation dest)
        {
            if (PrevBoard[dest.row, source.col] is Pawn) this.LastMoveWherePawnMoved = this.MoveNumbr; // keep track of what turn was the last pawn movemant, for checking fifhy move rule draw purpose
            if (!(PrevBoard[dest.row, dest.col] is EmptyPiece) &&
                PrevBoard[dest.row, dest.col].getColor() != (IswhiteTurn ? "white" : "black")) this.LastMoveThereWasCapture = this.MoveNumbr; // keep track of what turn was the last capture, for checking fifhy move rule draw purpose
            if ((dest.row == 1 || dest.row == 8) && board[source.row, source.col] is Pawn) // check if a pawn moved to the last/first row this turn, to promote it
                PromotePawn(new BoardLocation(dest.row, dest.col),
                    IswhiteTurn ? "whIte" : "black");
            if (this.boardsMemory[this.boardsMemory.Length - 1] != null) this.IncreaseMemorySize(); //increase board memory if needed
            this.boardsMemory[MoveNumbr] = this.ConvertBoardToString(board);//save board in memory
            this.MoveNumbr++;
            PrintBoard(); //print the board each turn
        }
        bool IsBothPlayersAgreedOnDraw(bool IswhiteTurn)
        {
            bool validInput = false;
            string userInput;
            do
            {
                Console.WriteLine((IswhiteTurn ? "Black" : "White") + " player , Opponent asked for draw. do you accept ? (Yes/No)");
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
        {//if there is one move on the board (from each cell to the other) that make the threathend king not be in check, its not checkmate
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
                MoveKing(source, destination); //need to check if we need to move the rook if there was castling
            if (board[source.row, source.col] is Pawn)
                MovePawn(source, destination);//need to check if we need to capture the adjecent opponent pawn if there was en passant
            if (board[destination.row, destination.col] is Rook)
                ((Rook)board[destination.row, destination.col]).moveNumber++;
            board[destination.row, destination.col] = board[source.row, source.col];
            board[source.row, source.col] = new EmptyPiece();
        }
        void MovePawn(BoardLocation source,BoardLocation destination)
        {
            Pawn currPawn = board[source.row, source.col] as Pawn;
            currPawn.moveNumber++;
            if (currPawn.IsEnPassant(source,destination,IswhiteTurn?"white":"black",this.board)) {
                if(source.col == destination.col - 1)
                    board[source.row, source.col+ 1] = new EmptyPiece();
                if (source.col == destination.col + 1)
                    board[source.row, source.col - 1] = new EmptyPiece();
            }
            
        }
        void MoveKing(BoardLocation source,BoardLocation destination)
        {
            King currKing = board[source.row, source.col] as King;
            if (currKing.isMovingIsCastling(source, destination, IswhiteTurn ? "white" : "black", board))
            {
                if (source.col > destination.col)
                {
                    board[source.row, source.col - 1] = new Rook(IswhiteTurn ? "white" : "black");
                    board[source.row, 0] = new EmptyPiece();
                }
                else
                {
                    board[source.row, source.col + 1] = new Rook(IswhiteTurn ? "white" : "black");
                    board[source.row, 7] = new EmptyPiece();
                }
            }
            currKing.moveNumber++;
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
        {//make copy of the board by values and not by reference (deep copy)
            Piece[,] newBoard = new Piece[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    newBoard[i, j] = board[i, j];
            return newBoard;
        }
        public bool IsCheck(string ThreateningPlayer, BoardLocation ThreatenedKingLocation)
        {//if there is one move of the opponent piece that can capture the king , its check
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
        {//if there is one move that can make the king not be in check , its not stalemate
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
            Console.Write("Stalemate - ");
            return true;
        }
        string getUserInput(bool IswhiteTurn)
        {//get and validate user input
            bool validInput = false;
            string input = "";
            while (!validInput)
            {
                Console.WriteLine((IswhiteTurn ? "White " : "Black ") + "Turn , enter your move , Or Ask for a draw (Type DRAW)");
                input = Console.ReadLine().Trim().ToUpper();
                if (input == "DRAW") return "DRAW";
                if (input.Length == 4 && ((int)input[1] >= 65 && (int)input[1] <= 89 || (int)input[3] >= 65 && (int)input[3] <= 89))
                    Console.WriteLine("invalid input.");
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
        {//check boards memory
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
        {//check our boolean variables. the value is 100 because each move is considered to be only one player move.
            if (LastMoveThereWasCapture <= MoveNumbr - 100 &&
                LastMoveWherePawnMoved <= MoveNumbr - 100)
            {
                Console.Write("Fifthy move rule - ");
                return true;
            }
            return false;
        }
        public bool IsDeadPositionDrawKingVsKing()
        {   //king vs king
            bool kingVsKing = true;
            for (int i = 0; i < 7 && kingVsKing; i++)
                for (int j = 0; j < 7; j++)
                    if (board[i, j] != null && !(board[i, j] is King) && kingVsKing)
                        return false;
            Console.Write("Dead Position - ");
            return true;
         }
        public bool IsDeadPositionDrawKingVsKingAndBishop()
        {//king vs king and bishop
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
