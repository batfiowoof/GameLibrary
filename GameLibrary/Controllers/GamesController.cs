using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Controllers
{
    public class GamesController : Controller
    {
        private readonly GameLibraryContext _context;
        private readonly GameSubject _gameSubject;
        private readonly GameSorter _gameSorter;

        public GamesController(GameLibraryContext context, GameSubject gameSubject)
        {
            _context = context;
            _gameSubject = gameSubject;
            _gameSorter = new GameSorter();

            _gameSubject.Attach(new GameAddedObserver());
        }

        //GET /Game/
        public async Task<IActionResult> Index(string sortOrder)
        {
            var games = await _context.Game.ToListAsync();

            switch (sortOrder)
            {
                case "title":
                    _gameSorter.SetSortStrategy(new SortByName());
                    break;
                case "release_date":
                    _gameSorter.SetSortStrategy(new SortByReleaseDate());
                    break;
                default:
                    _gameSorter.SetSortStrategy(new SortByGenre());
                    break;
            }

            var sortedGames = _gameSorter.Sort(games);

            return View(sortedGames);
        }

        // GET: /Game/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var game = await _context.Game.FirstOrDefaultAsync(m => m.ID == id);
            if (game == null)
                return NotFound();

            return View(game);
        }

        // GET: /Game/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Game/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Publisher,Developer,ReleaseDate,Genre")] Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                _gameSubject.Notify(game); // Notify observers of the new game
                return RedirectToAction("Index");
            }
            return View(game);
        }

        // GET: /Game/Edit/
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var game = await _context.Game.FindAsync(id);
            if (game == null)
                return NotFound();

            return View(game);
        }

        // POST: /Game/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Game game)
        {
            if (id != game.ID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                    _gameSubject.Notify(game);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.ID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Index");
            }
            return View(game);
        }

        // GET: /Game/Delete/
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var game = await _context.Game.FirstOrDefaultAsync(m => m.ID == id);
            if (game == null)
                return NotFound();

            return View(game);
        }

        // POST: /Game/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Game.FindAsync(id);
            _context.Game.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool GameExists(int id)
        {
            return _context.Game.Any(e => e.ID == id);
        }
    }
}
