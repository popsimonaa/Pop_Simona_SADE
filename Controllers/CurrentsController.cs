using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pop_Simona_SADE.Data;
using Pop_Simona_SADE.Models;
using Pop_Simona_SADE.Models.ExhibitionViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Pop_Simona_SADE.Controllers
{
    [Authorize(Policy = "OnlySales")]
    public class CurrentsController : Controller
    {
        private readonly ExhibitionContext _context;

        public CurrentsController(ExhibitionContext context)
        {
            _context = context;
        }

        // GET: Currents
        public async Task<IActionResult> Index(int? id, int? paintingID)
        {
            var viewModel = new CurrentIndexData();
            viewModel.Currents = await _context.Currents
                    .Include(i => i.CurrentPaintings)
                    .ThenInclude(i => i.Painting)
                    .ThenInclude(i => i.Orders)
                    .ThenInclude(i => i.Customer)
                    .AsNoTracking()
                    .OrderBy(i => i.CurrentName)
                    .ToListAsync();
            if (id != null)
            {
                ViewData["CurrentID"] = id.Value;
                Current current= viewModel.Currents.Where(
                i => i.ID == id.Value).Single();
                viewModel.Paintings = current.CurrentPaintings.Select(s => s.Painting);
            }
            if (paintingID != null)
            {
                ViewData["PaintingID"] = paintingID.Value;
                viewModel.Orders = viewModel.Paintings.Where(
                x => x.ID == paintingID).Single().Orders;
            }
            return View(viewModel);
        }

        // GET: Currents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var current = await _context.Currents
                .FirstOrDefaultAsync(m => m.ID == id);
            if (current == null)
            {
                return NotFound();
            }

            return View(current);
        }

        // GET: Currents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Currents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CurrentName,Particularity")] Current current)
        {
            if (ModelState.IsValid)
            {
                _context.Add(current);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(current);
        }

        // GET: Currents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var current = await _context.Currents
                .Include(i => i.CurrentPaintings).ThenInclude(i => i.Painting)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (current == null)
            {
                return NotFound();
            }
            PopulateCurrentPaintingData(current);
            return View(current);
        }
        private void PopulateCurrentPaintingData(Current current)
        {
            var allPaintings = _context.Paintings;
            var currentPainting = new HashSet<int>(current.CurrentPaintings.Select(c => c.PaintingID));
            var viewModel = new List<CurrentPaintingData>();
            foreach (var painting in allPaintings)
            {
                viewModel.Add(new CurrentPaintingData
                {
                    PaintingID = painting.ID,
                    Title = painting.Title,
                    IsBelonged = currentPainting.Contains(painting.ID)
                });
            }
            ViewData["Paintings"] = viewModel;
        }

        // POST: Currents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedPaintings)
        {
            if (id == null)
            {
                return NotFound();
            }
            var currentToUpdate = await _context.Currents
                .Include(i => i.CurrentPaintings)
                .ThenInclude(i => i.Painting)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (await TryUpdateModelAsync<Current> (currentToUpdate, 
                "", 
                i => i.CurrentName, i => i.Particularity))
            {
                UpdateCurrentPaintings(selectedPaintings, currentToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists, ");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateCurrentPaintings(selectedPaintings, currentToUpdate);
            PopulateCurrentPaintingData(currentToUpdate);
            return View(currentToUpdate);
        }
        private void UpdateCurrentPaintings(string[] selectedPaintings, Current currentToUpdate)
        {
            if (selectedPaintings == null)
            {
                currentToUpdate.CurrentPaintings = new List<CurrentPainting>();
                return;
            }
            var selectedPaintingsHS = new HashSet<string>(selectedPaintings);
            var currentPaintings = new HashSet<int>
           (currentToUpdate.CurrentPaintings.Select(c => c.Painting.ID));
            foreach (var painting in _context.Paintings)
            {
                if (selectedPaintingsHS.Contains(painting.ID.ToString()))
                {
                    if (!currentPaintings.Contains(painting.ID))
                    {
                       currentToUpdate.CurrentPaintings.Add(new CurrentPainting
                       {
                            CurrentID = currentToUpdate.ID,
                            PaintingID = painting.ID
                       });
                    }
                }
                else
                {
                    if (currentPaintings.Contains(painting.ID))
                    {
                        CurrentPainting paintingToRemove = currentToUpdate.CurrentPaintings.FirstOrDefault(i => i.PaintingID == painting.ID);
                        _context.Remove(paintingToRemove);
                    }
                }
            }
        }


        // GET: Currents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var current = await _context.Currents
                .FirstOrDefaultAsync(m => m.ID == id);
            if (current == null)
            {
                return NotFound();
            }

            return View(current);
        }

        // POST: Currents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var current = await _context.Currents.FindAsync(id);
            _context.Currents.Remove(current);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CurrentExists(int id)
        {
            return _context.Currents.Any(e => e.ID == id);
        }
    }
}
