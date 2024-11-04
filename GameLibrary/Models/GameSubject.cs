namespace GameLibrary.Models
{
    public class GameSubject
    {
        private readonly List<IGameObserver> _observers = new List<IGameObserver>();

        public void Attach(IGameObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IGameObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(Game game)
        {
            foreach (var observer in _observers)
            {
                observer.Update(game);
            }
        }
    }
}
