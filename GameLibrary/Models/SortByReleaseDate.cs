namespace GameLibrary.Models
{
    public class SortByReleaseDate : ISortStrategy
    {
        public IEnumerable<Game> Sort(IEnumerable<Game> games)
        {
            return games.OrderBy(g => g.ReleaseDate);
        }
    }
}
