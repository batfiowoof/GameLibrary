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
            // Set up sorting options
            ViewData["TitleSortParm"] = sortOrder == "title" ? "title_desc" : "title";
            ViewData["ReleaseDateSortParm"] = sortOrder == "releaseDate" ? "releaseDate_desc" : "releaseDate";
            ViewData["GenreSortParm"] = sortOrder == "genre" ? "genre_desc" : "genre";

            // Retrieve all games from the database
            var games = from g in _context.Game select g;

            // Apply sorting based on sortOrder parameter
            switch (sortOrder)
            {
                case "title":
                    games = games.OrderBy(g => g.Title);
                    break;
                case "title_desc":
                    games = games.OrderByDescending(g => g.Title);
                    break;
                case "releaseDate":
                    games = games.OrderBy(g => g.ReleaseDate);
                    break;
                case "releaseDate_desc":
                    games = games.OrderByDescending(g => g.ReleaseDate);
                    break;
                case "genre":
                    games = games.OrderBy(g => g.Genre);
                    break;
                case "genre_desc":
                    games = games.OrderByDescending(g => g.Genre);
                    break;
                default:
                    games = games.OrderBy(g => g.Title); // Default sort by title
                    break;
            }

            return View(await games.AsNoTracking().ToListAsync());
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Publisher,Developer,ReleaseDate,Genre")] Game game)
        {
            if (id != game.ID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();

                    _gameSubject.Notify(game); // Notify observers of the updated game
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.ID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
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
