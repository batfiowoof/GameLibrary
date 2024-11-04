namespace GameLibrary.Models
{
    public class SortByName : ISortStrategy
    {
        public IEnumerable<Game> Sort(IEnumerable<Game> games)
        {
            return games.OrderBy(g => g.Title);
        }
    }
}
