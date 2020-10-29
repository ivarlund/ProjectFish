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
    public class CompPlacesController : Controller
    {
        private readonly ProjectFishContext _context;

        public CompPlacesController(ProjectFishContext context)
        {
            _context = context;
        }

        // GET: CompPlaces
        public async Task<IActionResult> Index()
        {
            string session = HttpContext.Session.GetString("user");
            int accountId = 0;
            if (session != null)
            {
                accountId = Convert.ToInt32(JsonConvert.DeserializeObject(session));
            }

            var projectFishContext = _context.CompPlace.Include(c => c.Composition).Where(c => c.Composition.AccountId == accountId).Include(c => c.Place);
            return View(await projectFishContext.ToListAsync());
        }

        // GET: CompPlaces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compPlace = await _context.CompPlace
                .Include(c => c.Composition)
                .Include(c => c.Place)
                .FirstOrDefaultAsync(m => m.CompPlaceId == id);
            if (compPlace == null)
            {
                return NotFound();
            }

            return View(compPlace);
        }

        // GET: CompPlaces/Create
        public IActionResult Create()
        {
            string session = HttpContext.Session.GetString("user");
            int accountId = 0;
            if (session != null)
            {
                accountId = Convert.ToInt32(JsonConvert.DeserializeObject(session));
            }

            ViewData["CompositionId"] = new SelectList(_context.Composition.Where(c => c.AccountId == accountId), "CompositionId", "Name");
            ViewData["Coordinates"] = new SelectList(_context.Place, "Coordinates", "Name");
            return View();
        }

        // POST: CompPlaces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompositionId,Coordinates,CompPlaceId")] CompPlace compPlace)
        {
            string session = HttpContext.Session.GetString("user");
            int accountId = 0;
            if (session != null)
            {
                accountId = Convert.ToInt32(JsonConvert.DeserializeObject(session));
            }

            if (ModelState.IsValid)
            {
                _context.Add(compPlace);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompositionId"] = new SelectList(_context.Composition.Where(c => c.AccountId == accountId), "CompositionId", "Name", compPlace.CompositionId);
            ViewData["Coordinates"] = new SelectList(_context.Place, "Coordinates", "Coordinates", compPlace.Coordinates);
            return View(compPlace);
        }

        // GET: CompPlaces/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string session = HttpContext.Session.GetString("user");
            int accountId = 0;
            if (session != null)
            {
                accountId = Convert.ToInt32(JsonConvert.DeserializeObject(session));
            }

            var compPlace = await _context.CompPlace.FindAsync(id);
            if (compPlace == null)
            {
                return NotFound();
            }
            ViewData["CompositionId"] = new SelectList(_context.Composition.Where(c => c.AccountId == accountId), "CompositionId", "Name", compPlace.CompositionId);
            ViewData["Coordinates"] = new SelectList(_context.Place, "Coordinates", "Name", compPlace.Coordinates);
            return View(compPlace);
        }

        // POST: CompPlaces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompositionId,Coordinates,CompPlaceId")] CompPlace compPlace)
        {
            if (id != compPlace.CompPlaceId)
            {
                return NotFound();
            }

            string session = HttpContext.Session.GetString("user");
            int accountId = 0;
            if (session != null)
            {
                accountId = Convert.ToInt32(JsonConvert.DeserializeObject(session));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compPlace);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompPlaceExists(compPlace.CompPlaceId))
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
            ViewData["CompositionId"] = new SelectList(_context.Composition.Where(c => c.AccountId == accountId), "CompositionId", "Name", compPlace.CompositionId);
            ViewData["Coordinates"] = new SelectList(_context.Place, "Coordinates", "Name", compPlace.Coordinates);
            return View(compPlace);
        }

        // GET: CompPlaces/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compPlace = await _context.CompPlace
                .Include(c => c.Composition)
                .Include(c => c.Place)
                .FirstOrDefaultAsync(m => m.CompPlaceId == id);
            if (compPlace == null)
            {
                return NotFound();
            }

            return View(compPlace);
        }

        // POST: CompPlaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compPlace = await _context.CompPlace.FindAsync(id);
            _context.CompPlace.Remove(compPlace);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompPlaceExists(int id)
        {
            return _context.CompPlace.Any(e => e.CompPlaceId == id);
        }
    }
}
