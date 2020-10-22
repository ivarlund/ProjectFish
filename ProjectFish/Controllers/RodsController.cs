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
    public class RodsController : Controller
    {
        private readonly ProjectFishContext _context;

        public RodsController(ProjectFishContext context)
        {
            _context = context;
        }

        // GET: Rods
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rod.ToListAsync());
        }

        // GET: Rods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rod = await _context.Rod
                .FirstOrDefaultAsync(m => m.RodId == id);
            if (rod == null)
            {
                return NotFound();
            }

            return View(rod);
        }

        // GET: Rods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RodId,Brand,Length,CastWeight")] Rod rod)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rod);
        }

        // GET: Rods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rod = await _context.Rod.FindAsync(id);
            if (rod == null)
            {
                return NotFound();
            }
            return View(rod);
        }

        // POST: Rods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RodId,Brand,Length,CastWeight")] Rod rod)
        {
            if (id != rod.RodId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RodExists(rod.RodId))
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
            return View(rod);
        }

        // GET: Rods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rod = await _context.Rod
                .FirstOrDefaultAsync(m => m.RodId == id);
            if (rod == null)
            {
                return NotFound();
            }

            return View(rod);
        }

        // POST: Rods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rod = await _context.Rod.FindAsync(id);
            _context.Rod.Remove(rod);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RodExists(int id)
        {
            return _context.Rod.Any(e => e.RodId == id);
        }
    }
}
