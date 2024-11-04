namespace GameLibrary.Models
{
    public class GameAddedObserver : IGameObserver
    {
        public void Update(Game game)
        {
            Console.WriteLine($"Game {game.Title} has been added to the library.");
        }
    }
}
