
namespace CheckersBackend.Model
{
	public class GameState
	{
		public string[][] Board { get; set; } = new string[8][];
		public string CurrentPlayer { get; set; } = "red";

		public GameState()
		{
			for (int i = 0; i < 8; i++)
			{
				Board[i] = new string[8];
			}
		}
	}
}
