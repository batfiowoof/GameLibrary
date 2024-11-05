using GameLibrary.Data;
using GameLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GameLibrary.Controllers
{
    public class GamesController : Controller
    {
        // Контекст на базата данни
        private readonly GameLibraryContext _context;
        // Сортиране на игрите
        private readonly GameSorter _gameSorter;

        private readonly GameSubject _gameSubject;

        public GamesController(GameLibraryContext context,GameSubject gameSubject)
        {
            _context = context;
            _gameSorter = new GameSorter();
            _gameSubject = gameSubject;
        }

        //GET /Game/
        public async Task<IActionResult> Index(string sortOrder)
        {
            // Retrieve all games from the database
            var games = await _context.Game.AsNoTracking().ToListAsync();

            // Initialize GameSorter and choose the appropriate strategy
            var gameSorter = new GameSorter();
            switch (sortOrder)
            {
                case "name":
                    gameSorter.SetSortStrategy(new SortByName());
                    break;
                case "releaseDate":
                    gameSorter.SetSortStrategy(new SortByReleaseDate());
                    break;
                case "genre":
                    gameSorter.SetSortStrategy(new SortByGenre());
                    break;
                default:
                    gameSorter.SetSortStrategy(new SortByName()); // Default sorting by name
                    break;
            }

            // Apply the selected sorting strategy
            var sortedGames = gameSorter.Sort(games);

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
        // Атрибутите [Bind] са необходими, за да избегнем атаки от типа "over-posting"
        public async Task<IActionResult> Create([Bind("ID,Title,Publisher,Developer,ReleaseDate,Genre")] Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                _gameSubject.Notify(game); // Известяваме всички наблюдатели за добавянето на нова игра
                return RedirectToAction("Index");
            }
            return View(game);
        }

        // GET: /Game/Edit/
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            // Use AsNoTracking to retrieve the entity without tracking it
            var game = await _context.Game.AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);
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
                    // Откачат се всякакви вече проследени инстанции на този обект
                    var existingEntity = _context.Game.Local.FirstOrDefault(e => e.ID == game.ID);
                    if (existingEntity != null)
                    {
                        _context.Entry(existingEntity).State = EntityState.Detached;
                    }

                    // Променяме състоянието на обекта
                    _context.Attach(game).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
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
            if (game != null)
            {
                _context.Game.Remove(game);
                // Прилагаме промените в контекста на базата данни
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Game.Any(e => e.ID == id);
        }
    }
}
