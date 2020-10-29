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

            var compositions = _context.Composition.Include(c => c.Account).Where(c => c.AccountId == accountId).
                Include(c => c.Reel).Include(c => c.Rod).
                Include(c => c.CompFish).ThenInclude(c => c.Fish).Include(c => c.CompLure).ThenInclude(c => c.Lure).
                Include(c => c.CompPlace).ThenInclude(c => c.Place);
            return View(await compositions.ToListAsync());
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

        public IActionResult CreateAll()
        {

            string session = HttpContext.Session.GetString("user");

            if (session != null)
            {
                int accountId = Convert.ToInt32(JsonConvert.DeserializeObject(session));
                var account = _context.Account.FindAsync(accountId);
                var name = account != null ? account.Result.Mail : "not logged IN!!!";
                ViewData["AccountId"] = name;
            }
            else
            {
                ViewData["AccountId"] = "Not logged in";
                return RedirectToAction("index", "Home");
            }

            ViewData["ReelId"] = new SelectList(_context.Reel, "ReelId", "Brand");
            ViewData["RodId"] = new SelectList(_context.Rod, "RodId", "Brand");
            ViewData["FishId"] = new SelectList(_context.Fish, "FishId", "Species");
            ViewData["LureId"] = new SelectList(_context.Lure, "LureId", "Brand");
            ViewData["Coordinates"] = new SelectList(_context.Place, "Coordinates", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAll(VMCompFishPlaceLure viewModel)
        {

            string session = HttpContext.Session.GetString("user");

            if (session != null)
            {
                int accountId = Convert.ToInt32(JsonConvert.DeserializeObject(session));
                var account = _context.Account.FindAsync(accountId);
                var name = account != null ? account.Result.Mail : "not logged IN!!!";

                viewModel.Composition.AccountId = accountId;
                ViewData["AccountId"] = name;
            }
            else
            {
                ViewData["AccountId"] = "Not logged in";
                return RedirectToAction("index", "Home");
            }


            
            if (ModelState.IsValid)
            {
                _context.Add(viewModel.Composition);
                _context.SaveChanges();

                if (viewModel.Fishes != null)
                {
                    foreach (var fish in viewModel.Fishes)
                    {
                        CompFish cf = new CompFish
                        {
                            Composition = viewModel.Composition,
                            FishId = fish
                        };

                        _context.Add(cf);
                    }
                }

                if (viewModel.Lures != null)
                {
                    foreach (var lure in viewModel.Lures)
                    {
                        CompLure cl = new CompLure
                        {
                            Composition = viewModel.Composition,
                            LureId = lure
                        };

                        _context.Add(cl);
                    }
                }

                if (viewModel.Places != null)
                {
                    foreach (var place in viewModel.Places)
                    {
                        CompPlace cp = new CompPlace
                        {
                            Composition = viewModel.Composition,
                            Coordinates = place
                        };

                        _context.Add(cp);
                    }
                }

                _context.SaveChanges();
                HttpContext.Session.SetString("added", "true");

                return RedirectToAction(nameof(Index));
            }

            ViewData["ReelId"] = new SelectList(_context.Reel, "ReelId", "Brand");
            ViewData["RodId"] = new SelectList(_context.Rod, "RodId", "Brand");
            ViewData["FishId"] = new SelectList(_context.Fish, "FishId", "Species");
            ViewData["LureId"] = new SelectList(_context.Lure, "LureId", "Brand");
            ViewData["Coordinates"] = new SelectList(_context.Place, "Coordinates", "Name");

            return View();
        }

        public async Task<IActionResult> EditAll(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VMCompFishPlaceLure viewModel = new VMCompFishPlaceLure();

            var composition = await _context.Composition.FindAsync(id);
            if (composition == null)
            {
                return NotFound();
            }

            viewModel.Composition = composition;

            ViewData["ReelId"] = new SelectList(_context.Reel, "ReelId", "Brand", composition.ReelId);
            ViewData["RodId"] = new SelectList(_context.Rod, "RodId", "Brand", composition.RodId);
            ViewData["FishId"] = new SelectList(_context.Fish, "FishId", "Species");
            ViewData["LureId"] = new SelectList(_context.Lure, "LureId", "Brand");
            ViewData["Coordinates"] = new SelectList(_context.Place, "Coordinates", "Name");

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAll(int id, VMCompFishPlaceLure viewModel)
        {
            if (id != viewModel.Composition.CompositionId)
            {
                return NotFound();
            }

            string session = HttpContext.Session.GetString("user");

            if (session != null)
            {
                ViewData["AccountId"] = session;
                int accountId = Convert.ToInt32(JsonConvert.DeserializeObject(session)); // denna får inte vara null
                viewModel.Composition.AccountId = accountId;
            }
            else
            {
                ViewData["AccountId"] = "Log in to edit compositions";
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewModel.Composition);
                    await _context.SaveChangesAsync();

                    if (viewModel.Fishes != null)
                    {
                        var compFishes = from matches in _context.CompFish
                                         where matches.CompositionId == id
                                         select matches;


                        _context.CompFish.RemoveRange(compFishes);
                        
                        foreach (var fish in viewModel.Fishes)
                        {
                            CompFish cf = new CompFish
                            {
                                Composition = viewModel.Composition,
                                FishId = fish
                            };

                            _context.Add(cf);

                        }
                    }

                    if (viewModel.Lures != null)
                    {

                        var compLures = from matches in _context.CompLure
                                        where matches.CompositionId == id
                                        select matches;

                        _context.CompLure.RemoveRange(compLures);

                        foreach (var lure in viewModel.Lures)
                        {
                            CompLure cl = new CompLure
                            {
                                Composition = viewModel.Composition,
                                LureId = lure
                            };

                            _context.Add(cl);
                        }
                    }

                    if (viewModel.Places != null)
                    {
                        var compPlaces = from matches in _context.CompPlace
                                         where matches.CompositionId == id
                                         select matches;

                        _context.CompPlace.RemoveRange(compPlaces);

                        foreach (var place in viewModel.Places)
                        {
                            CompPlace cp = new CompPlace
                            {
                                Composition = viewModel.Composition,
                                Coordinates = place
                            };

                            _context.Add(cp);
                        }
                    }

                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompositionExists(viewModel.Composition.CompositionId))
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

            ViewData["ReelId"] = new SelectList(_context.Reel, "ReelId", "Brand", viewModel.Composition.ReelId);
            ViewData["RodId"] = new SelectList(_context.Rod, "RodId", "Brand", viewModel.Composition.RodId);
            ViewData["FishId"] = new SelectList(_context.Fish, "FishId", "Species");
            ViewData["LureId"] = new SelectList(_context.Lure, "LureId", "Brand");
            ViewData["Coordinates"] = new SelectList(_context.Place, "Coordinates", "Name");
            return View(viewModel.Composition);
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
                int accountId = Convert.ToInt32(JsonConvert.DeserializeObject(session));
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
                //return RedirectToAction("Create", "CompFish");

                return RedirectToAction(nameof(Index));
            }

            ViewData["ReelId"] = new SelectList(_context.Reel, "ReelId", "Brand", composition.ReelId);
            ViewData["RodId"] = new SelectList(_context.Rod, "RodId", "Brand", composition.RodId);

            return View(composition);
            //return RedirectToAction("Create", "CompFish");
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
            ViewData["ReelId"] = new SelectList(_context.Reel, "ReelId", "Brand", composition.ReelId);
            ViewData["RodId"] = new SelectList(_context.Rod, "RodId", "Brand", composition.RodId);
            return View(composition);
        }

        // POST: Compositions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompositionId,RodId,ReelId,Name")] Composition composition)
        {
            if (id != composition.CompositionId)
            {
                return NotFound();
            }

            string session = HttpContext.Session.GetString("user");

            if (session != null)
            {
                ViewData["AccountId"] = session;
                int accountId = Convert.ToInt32(JsonConvert.DeserializeObject(session)); // denna får inte vara null
                composition.AccountId = accountId;
            }
            else
            {
                ViewData["AccountId"] = "Log in to edit compositions";
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
            //ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "Mail", composition.AccountId);
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
