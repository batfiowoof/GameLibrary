namespace GameLibrary.Models
{
    public interface ISortStrategy
    {
        IEnumerable<Game> Sort(IEnumerable<Game> games);
    }
}
