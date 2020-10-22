using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFish.Models;

namespace ProjectFish.Controllers
{
    public class ReelsController : Controller
    {
        private readonly ProjectFishContext _context;

        public ReelsController(ProjectFishContext context)
        {
            _context = context;
        }

        // GET: Reels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reel.ToListAsync());
        }

        // GET: Reels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reel = await _context.Reel
                .FirstOrDefaultAsync(m => m.ReelId == id);
            if (reel == null)
            {
                return NotFound();
            }

            return View(reel);
        }

        // GET: Reels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReelId,Brand,Type,Line")] Reel reel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reel);
        }

        // GET: Reels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reel = await _context.Reel.FindAsync(id);
            if (reel == null)
            {
                return NotFound();
            }
            return View(reel);
        }

        // POST: Reels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReelId,Brand,Type,Line")] Reel reel)
        {
            if (id != reel.ReelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReelExists(reel.ReelId))
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
            return View(reel);
        }

        // GET: Reels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reel = await _context.Reel
                .FirstOrDefaultAsync(m => m.ReelId == id);
            if (reel == null)
            {
                return NotFound();
            }

            return View(reel);
        }

        // POST: Reels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reel = await _context.Reel.FindAsync(id);
            _context.Reel.Remove(reel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReelExists(int id)
        {
            return _context.Reel.Any(e => e.ReelId == id);
        }
    }
}
