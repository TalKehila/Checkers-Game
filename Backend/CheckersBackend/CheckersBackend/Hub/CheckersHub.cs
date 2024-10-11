namespace CheckersBackend.Hubs
{
	public class CheckersHub :Hub
	{
		private readonly IGameService _gameService;

        public CheckersHub(IGameService gameService)
        {
            _gameService = gameService;            
        }

        public override async Task OnConnectedAsync()
        {
            string clientId = Context.ConnectionId;
            var res = _gameService.AddPlayer(clientId);
            await Clients.Caller.SendAsync("message", res.Message);

            if (res.IsSecondPlayer)
            {
                await Clients.Others.SendAsync("message", $"A new player has joined as {res.Color}");
                var gameState = _gameService.GetGameState();
                await Clients.All.SendAsync("updateBoard", gameState.Board);
				await Clients.All.SendAsync("switchPlayer", gameState.CurrentPlayer);

			}
            await base.OnConnectedAsync();
		}

		public override async Task OnDisconnectedAsync(Exception exception)
		{
			string clientId = Context.ConnectionId;
			_gameService.RemovePlayer(clientId);
			await Clients.All.SendAsync("message", "A player has disconnected");
			await base.OnDisconnectedAsync(exception);
		}


		public async Task Move(int fromRow, int fromCol, int toRow, int toCol)
		{
			var result = _gameService.MakeMove(fromRow, fromCol, toRow, toCol);		
			if (result.IsValid)
			{
				var gameState = _gameService.GetGameState();
				await Clients.All.SendAsync("updateBoard", gameState.Board);
				await Clients.All.SendAsync("switchPlayer", gameState.CurrentPlayer);
			}
			else
			{
				await Clients.Caller.SendAsync("message", "Invalid move. Try again.");
			}
		}

		public async Task ResetBoard()
		{
			_gameService.ResetGame();
			var gameState = _gameService.GetGameState();
			await Clients.All.SendAsync("updateBoard", gameState.Board);
			await Clients.All.SendAsync("switchPlayer", gameState.CurrentPlayer);
			await Clients.All.SendAsync("message", "The board has been reset");
		}



	}
}
