namespace GameLibrary.Models
{
    public class GameSorter
    {
        private ISortStrategy _sortStrategy;

        public void SetSortStrategy(ISortStrategy sortStrategy)
        {
            _sortStrategy = sortStrategy;
        }

        public IEnumerable<Game> Sort(IEnumerable<Game> games)
        {
            return _sortStrategy.Sort(games);
        }
    }
}
