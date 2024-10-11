namespace CheckersBackend.Controllers
{
	
    [ApiController]
	[Route("[controller]")]
	public class CheckersController : ControllerBase
	{
		private readonly IGameService _gameService;

		public CheckersController(IGameService gameService)
		{
			_gameService = gameService;
		}

		[HttpGet("state")]
		public IActionResult GetGameState()
		{
			return Ok(_gameService.GetGameState());
		}
	}
}

