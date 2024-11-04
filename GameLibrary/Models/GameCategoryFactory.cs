namespace GameLibrary.Models
{
    public static class GameCategoryFactory
    {
        public static IGameCategory GetGameCategory(string category)
        {
            switch (category)
            {
                case "Action":
                    return new ActionGame();
                case "RPG":
                    return new RPGGame();
                case "Puzzle":
                    return new PuzzleGame();
                default:
                    throw new ArgumentException("Invalid category type!");
            }
        }
    }
}
