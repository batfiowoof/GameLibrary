namespace GameLibrary.Models
{
    public class SortByGenre : ISortStrategy
    {
        public IEnumerable<Game> Sort(IEnumerable<Game> games)
        {
            return games.OrderBy(g => g.Genre);
        }
    }
}
