namespace CheckersBackend.Interfaces
{
	public interface IGameService
	{
		(bool IsSecondPlayer, string Color, string Message) AddPlayer(string clientId);
		void RemovePlayer(string clientId);
		(bool IsValid, string Message) MakeMove(int fromRow, int fromCol, int toRow, int toCol);
		void ResetGame();
		GameState GetGameState();
	}
}
