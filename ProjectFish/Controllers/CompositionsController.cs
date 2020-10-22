using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectFish.Models;

namespace ProjectFish.Controllers
{
    public class CompositionsController : Controller
    {
        private readonly ProjectFishContext _context;

        public CompositionsController(ProjectFishContext context)
        {
            _context = context;
        }

        // GET: Compositions
        public async Task<IActionResult> Index()
        {
            string session = HttpContext.Session.GetString("user");
            int accountId = 0;
            if (session != null)
            {
                accountId = Convert.ToInt32(JsonConvert.DeserializeObject(session));
            }

            var projectFishContext = _context.Composition.Include(c => c.Account).Where(c => c.AccountId == accountId).Include(c => c.Reel).Include(c => c.Rod).
                Include(c => c.CompFish).ThenInclude(c => c.Fish).Include(c => c.CompLure).ThenInclude(c => c.Lure).
                Include(c => c.CompPlace).ThenInclude(c => c.Place); // Sista THENINCLUDE var det som funkade för att få åtkomst till elementen!
            return View(await projectFishContext.ToListAsync());
        }

        // GET: Compositions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var composition = await _context.Composition
                .Include(c => c.Account)
                .Include(c => c.Reel)
                .Include(c => c.Rod)
                .Include(c => c.CompFish).ThenInclude(c => c.Fish)
                .Include(c => c.CompLure).ThenInclude(c => c.Lure)
                .Include(c => c.CompPlace).ThenInclude(c => c.Place)
                .FirstOrDefaultAsync(m => m.CompositionId == id);
            if (composition == null)
            {
                return NotFound();
            }

            return View(composition);
        }

        // GET: Compositions/Create
        public IActionResult Create()
        {
            string session = HttpContext.Session.GetString("user");
            
            if (session != null)
            {
                int accountId = Convert.ToInt32(JsonConvert.DeserializeObject(session));
                //var account = _context.Account.SingleOrDefault(c => c.AccountId == accountId); // Hämta en specifik rad från en tabell
                var account = _context.Account.FindAsync(accountId);
                var name = account != null ? account.Result.Mail : "not logged IN!!!"; // Hämta värdet från en specifik kolumn
                ViewData["AccountId"] = name;
            } 
            else
            {
                ViewData["AccountId"] = "Not logged in";
            }

            ViewData["ReelId"] = new SelectList(_context.Reel, "ReelId", "Brand");
            ViewData["RodId"] = new SelectList(_context.Rod, "RodId", "Brand");
            ViewData["FishId"] = new SelectList(_context.Fish, "FishId", "Species");

            return View();
        }

        // POST: Compositions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompositionId,RodId,ReelId,Name")] Composition composition)
        {
            string session = HttpContext.Session.GetString("user");

            if (session != null)
            {
                ViewData["AccountId"] = session;
                int accountId = Convert.ToInt32(JsonConvert.DeserializeObject(session)); // denna får inte vara null
                composition.AccountId = accountId;
            }
            else
            {
                ViewData["AccountId"] = "Log in to add compositions";
            }

            if (ModelState.IsValid)
            {
                _context.Add(composition);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("added", "true");
                return RedirectToAction(nameof(Index));
            }

            //ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "Mail", composition.AccountId);
            ViewData["ReelId"] = new SelectList(_context.Reel, "ReelId", "Brand", composition.ReelId);
            ViewData["RodId"] = new SelectList(_context.Rod, "RodId", "Brand", composition.RodId);
            //ViewData["FishId"] = new SelectList(_context.Fish, "FishId", "Species", compFish.FishId);

            return View(composition);
        }

        // GET: Compositions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var composition = await _context.Composition.FindAsync(id);
            if (composition == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "Mail", composition.AccountId);
            ViewData["ReelId"] = new SelectList(_context.Reel, "ReelId", "Brand", composition.ReelId);
            ViewData["RodId"] = new SelectList(_context.Rod, "RodId", "Brand", composition.RodId);
            return View(composition);
        }

        // POST: Compositions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompositionId,AccountId,RodId,ReelId,Name")] Composition composition)
        {
            if (id != composition.CompositionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(composition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompositionExists(composition.CompositionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "Mail", composition.AccountId);
            ViewData["ReelId"] = new SelectList(_context.Reel, "ReelId", "Brand", composition.ReelId);
            ViewData["RodId"] = new SelectList(_context.Rod, "RodId", "Brand", composition.RodId);
            return View(composition);
        }

        // GET: Compositions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var composition = await _context.Composition
                .Include(c => c.Account)
                .Include(c => c.Reel)
                .Include(c => c.Rod)
                .FirstOrDefaultAsync(m => m.CompositionId == id);
            if (composition == null)
            {
                return NotFound();
            }

            return View(composition);
        }

        // POST: Compositions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compFishes = from matches in _context.CompFish
                             where matches.CompositionId == id
                             select matches;

            var compLures = from matches in _context.CompLure
                            where matches.CompositionId == id
                            select matches;

            var compPlaces = from matches in _context.CompPlace
                             where matches.CompositionId == id
                             select matches;

            _context.CompFish.RemoveRange(compFishes);
            _context.CompLure.RemoveRange(compLures);
            _context.CompPlace.RemoveRange(compPlaces);

            var composition = await _context.Composition.FindAsync(id);
            _context.Composition.Remove(composition);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompositionExists(int id)
        {
            return _context.Composition.Any(e => e.CompositionId == id);
        }
    }
}
