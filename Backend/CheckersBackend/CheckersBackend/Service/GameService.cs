namespace CheckersBackend.Service
{
	public class GameService : IGameService
	{
		private ConcurrentDictionary<string, string> _players = new ConcurrentDictionary<string, string>();
		private GameState _gameState = new GameState();

		public (bool IsSecondPlayer, string Color, string Message) AddPlayer(string clientId)
		{
			if (_players.Count < 2)
			{
				string color = _players.Count == 0 ? "red" : "black";
				_players.TryAdd(clientId, color);
				if (_players.Count == 2) InitializeBoard();
				return (_players.Count == 2, color, $"You are the {color} player");
			}
			return (false, "", "Game is full, you are a spectator");
		}
		public void RemovePlayer(string clientId)
		{
			_players.TryRemove(clientId, out _);
		}

		public (bool IsValid, string Message) MakeMove(int fromRow, int fromCol, int toRow, int toCol)
		{
			if (IsValidMove(fromRow, fromCol, toRow, toCol))
			{
				_gameState.Board[toRow][toCol] = _gameState.Board[fromRow][fromCol];
				_gameState.Board[fromRow][fromCol] = null;

				if (Math.Abs(fromRow - toRow) == 2) //Jump of 2 not allowed.!
				{
					int midRow = (fromRow + toRow) / 2;
					int midCol = (fromCol + toCol) / 2;
					_gameState.Board[midRow][midCol] = null;
				}

				_gameState.CurrentPlayer = _gameState.CurrentPlayer == "red" ? "black" : "red";
				return (true, "Move successful");
			}
			return (false, "Invalid move");
		}

		public void ResetGame()
		{
			InitializeBoard();
			_gameState.CurrentPlayer = "red";
		}
		public GameState GetGameState()
		{
			return _gameState;
		}
		private void InitializeBoard()
		{
			for (int row = 0; row < 8; row++)
			{
				for (int col = 0; col < 8; col++)
				{
					if ((row + col) % 2 == 1)
					{
						if (row < 3)
							_gameState.Board[row][col] = "black";
						else if (row > 4)
							_gameState.Board[row][col] = "red";
						else
							_gameState.Board[row][col] = null;
					}
					else
					{
						_gameState.Board[row][col] = null;
					}
				}
			}
		}

		
		private bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol)
		{
			Console.WriteLine($"Current Player: {_gameState.CurrentPlayer}");
			Console.WriteLine($"Piece at from position: {_gameState.Board[fromRow][fromCol]}");
			Console.WriteLine($"Attempting move from ({fromRow},{fromCol}) to ({toRow},{toCol})");

			// Check if the piece belongs to the current player (validtion check)
			if (_gameState.Board[fromRow][fromCol] != _gameState.CurrentPlayer)
			{
				Console.WriteLine("Invalid move: piece does not belong to current player.");
				return false;
			}

			int rowDiff = toRow - fromRow;
			int colDiff = Math.Abs(toCol - fromCol);

			// Check the direction of the player ! 
			if (_gameState.CurrentPlayer == "red" && rowDiff > 0)
			{
				Console.WriteLine("Invalid move: red pieces cannot move downwards.");
				return false;
			}
			if (_gameState.CurrentPlayer == "black" && rowDiff < 0)
			{
				Console.WriteLine("Invalid move: black pieces cannot move upwards.");
				return false;
			}

			
			if (Math.Abs(rowDiff) == 1 && (colDiff == 1 || colDiff == 0) && _gameState.Board[toRow][toCol] == null)
			{
				Console.WriteLine("Valid move: single step.");
				return true;
			}

			if (Math.Abs(rowDiff) == 2 && colDiff == 2)
			{
				int midRow = (fromRow + toRow) / 2;
				int midCol = (fromCol + toCol) / 2;
				if (_gameState.Board[midRow][midCol] != null &&
					_gameState.Board[midRow][midCol] != _gameState.CurrentPlayer &&
					_gameState.Board[toRow][toCol] == null)
				{
					Console.WriteLine("Valid move: jump.");
					return true;
				}
			}
			Console.WriteLine("Invalid move: does not meet any move criteria.");
			return false;
		}

	}
}


