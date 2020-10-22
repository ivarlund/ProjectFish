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
    public class CompLuresController : Controller
    {
        private readonly ProjectFishContext _context;

        public CompLuresController(ProjectFishContext context)
        {
            _context = context;
        }

        // GET: CompLures
        public async Task<IActionResult> Index()
        {
            string session = HttpContext.Session.GetString("user");
            int accountId = 0;
            if (session != null)
            {
                accountId = Convert.ToInt32(JsonConvert.DeserializeObject(session));
            }

            var projectFishContext = _context.CompLure.Include(c => c.Composition).Where(c => c.Composition.AccountId == accountId).Include(c => c.Lure);
            return View(await projectFishContext.ToListAsync());
        }

        // GET: CompLures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compLure = await _context.CompLure
                .Include(c => c.Composition)
                .Include(c => c.Lure)
                .FirstOrDefaultAsync(m => m.CompLureId == id);
            if (compLure == null)
            {
                return NotFound();
            }

            return View(compLure);
        }

        // GET: CompLures/Create
        public IActionResult Create()
        {
            string session = HttpContext.Session.GetString("user");
            int accountId = 0;
            if (session != null)
            {
                accountId = Convert.ToInt32(JsonConvert.DeserializeObject(session));
            }

            ViewData["CompositionId"] = new SelectList(_context.Composition.Where(c => c.AccountId == accountId), "CompositionId", "Name");
            ViewData["LureId"] = new SelectList(_context.Lure, "LureId", "Brand");
            return View();
        }

        // POST: CompLures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompositionId,LureId,CompLureId")] CompLure compLure)
        {
            string session = HttpContext.Session.GetString("user");
            int accountId = 0;
            if (session != null)
            {
                accountId = Convert.ToInt32(JsonConvert.DeserializeObject(session));
            }

            if (ModelState.IsValid)
            {
                _context.Add(compLure);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompositionId"] = new SelectList(_context.Composition.Where(c => c.AccountId == accountId), "CompositionId", "Name", compLure.CompositionId);
            ViewData["LureId"] = new SelectList(_context.Lure, "LureId", "Brand", compLure.LureId);
            return View(compLure);
        }

        // GET: CompLures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compLure = await _context.CompLure.FindAsync(id);
            if (compLure == null)
            {
                return NotFound();
            }
            ViewData["CompositionId"] = new SelectList(_context.Composition, "CompositionId", "Name", compLure.CompositionId);
            ViewData["LureId"] = new SelectList(_context.Lure, "LureId", "Brand", compLure.LureId);
            return View(compLure);
        }

        // POST: CompLures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompositionId,LureId,CompLureId")] CompLure compLure)
        {
            if (id != compLure.CompLureId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compLure);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompLureExists(compLure.CompLureId))
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
            ViewData["CompositionId"] = new SelectList(_context.Composition, "CompositionId", "Name", compLure.CompositionId);
            ViewData["LureId"] = new SelectList(_context.Lure, "LureId", "Brand", compLure.LureId);
            return View(compLure);
        }

        // GET: CompLures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compLure = await _context.CompLure
                .Include(c => c.Composition)
                .Include(c => c.Lure)
                .FirstOrDefaultAsync(m => m.CompLureId == id);
            if (compLure == null)
            {
                return NotFound();
            }

            return View(compLure);
        }

        // POST: CompLures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compLure = await _context.CompLure.FindAsync(id);
            _context.CompLure.Remove(compLure);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompLureExists(int id)
        {
            return _context.CompLure.Any(e => e.CompLureId == id);
        }
    }
}
