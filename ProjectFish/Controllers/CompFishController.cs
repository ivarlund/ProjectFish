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
    public class CompFishController : Controller
    {
        private readonly ProjectFishContext _context;

        public CompFishController(ProjectFishContext context)
        {
            _context = context;
        }

        // GET: CompFish
        public async Task<IActionResult> Index()
        {

            string session = HttpContext.Session.GetString("user");
            int accountId = 0;
            if (session != null)
            {
                accountId = Convert.ToInt32(JsonConvert.DeserializeObject(session));
            }
            var projectFishContext = _context.CompFish.Include(c => c.Composition).Where(c => c.Composition.AccountId == accountId).Include(c => c.Fish);
            return View(await projectFishContext.ToListAsync());
        }

        // GET: CompFish/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compFish = await _context.CompFish
                .Include(c => c.Composition)
                .Include(c => c.Fish)
                .FirstOrDefaultAsync(m => m.CompFishId == id);
            if (compFish == null)
            {
                return NotFound();
            }

            return View(compFish);
        }

        // GET: CompFish/Create
        public IActionResult Create()
        {
            string session = HttpContext.Session.GetString("user");
            int accountId = 0;
            if (session != null)
            {
                accountId = Convert.ToInt32(JsonConvert.DeserializeObject(session));
            }

            ViewData["CompositionId"] = new SelectList(_context.Composition.Where(c => c.AccountId == accountId), "CompositionId", "Name");
            ViewData["FishId"] = new SelectList(_context.Fish, "FishId", "Species");
            return View();
        }

        // POST: CompFish/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompositionId,FishId,CompFishId")] CompFish compFish)
        {
            string session = HttpContext.Session.GetString("user");
            int accountId = 0;
            if (session != null)
            {
                accountId = Convert.ToInt32(JsonConvert.DeserializeObject(session));
            }

            if (ModelState.IsValid)
            {
                _context.Add(compFish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompositionId"] = new SelectList(_context.Composition.Where(c => c.AccountId == accountId), "CompositionId", "Name", compFish.CompositionId);
            ViewData["FishId"] = new SelectList(_context.Fish, "FishId", "Species", compFish.FishId);
            return View(compFish);
        }

        // GET: CompFish/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compFish = await _context.CompFish.FindAsync(id);
            if (compFish == null)
            {
                return NotFound();
            }
            ViewData["CompositionId"] = new SelectList(_context.Composition, "CompositionId", "Name", compFish.CompositionId);
            ViewData["FishId"] = new SelectList(_context.Fish, "FishId", "Species", compFish.FishId);
            return View(compFish);
        }

        // POST: CompFish/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompositionId,FishId,CompFishId")] CompFish compFish)
        {
            if (id != compFish.CompFishId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compFish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompFishExists(compFish.CompFishId))
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
            ViewData["CompositionId"] = new SelectList(_context.Composition, "CompositionId", "Name", compFish.CompositionId);
            ViewData["FishId"] = new SelectList(_context.Fish, "FishId", "Species", compFish.FishId);
            return View(compFish);
        }

        // GET: CompFish/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compFish = await _context.CompFish
                .Include(c => c.Composition)
                .Include(c => c.Fish)
                .FirstOrDefaultAsync(m => m.CompFishId == id);
            if (compFish == null)
            {
                return NotFound();
            }

            return View(compFish);
        }

        // POST: CompFish/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compFish = await _context.CompFish.FindAsync(id);
            _context.CompFish.Remove(compFish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompFishExists(int id)
        {
            return _context.CompFish.Any(e => e.CompFishId == id);
        }
    }
}
