using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pop_Simona_SADE.Data;
using Pop_Simona_SADE.Models;
using Microsoft.AspNetCore.Authorization;

namespace Pop_Simona_SADE.Controllers
{
    [Authorize(Roles = "Employee")]
    public class PaintingsController : Controller
    {
        private readonly ExhibitionContext _context;

        public PaintingsController(ExhibitionContext context)
        {
            _context = context;
        }

        // GET: Paintings
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var paintings = from b in _context.Paintings
                            select b;
            if (!String.IsNullOrEmpty(searchString))
            {
            paintings = paintings.Where(s => s.Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "title_desc":
                    paintings = paintings.OrderByDescending(b => b.Title);
                    break;
                case "Price":
                    paintings = paintings.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    paintings = paintings.OrderByDescending(b => b.Price);
                    break;
                default:
                    paintings = paintings.OrderBy(b => b.Title);
                    break;
            }
            int pageSize = 2;
            return View(await PaginatedList<Painting>.CreateAsync(paintings.AsNoTracking(), pageNumber ?? 1, pageSize));
        }


        // GET: Paintings/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var painting = await _context.Paintings
                .Include(s => s.Orders)
                .ThenInclude(e => e.Customer)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (painting == null)
            {
                return NotFound();
            }

            return View(painting);
        }

        // GET: Paintings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Paintings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Painter,Price")] Painting painting)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(painting);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex*/)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists ");
            }
            return View(painting);
        }

        // GET: Paintings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var painting = await _context.Paintings.FindAsync(id);
            if (painting == null)
            {
                return NotFound();
            }
            return View(painting);
        }

        // POST: Paintings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var studentToUpdate = await _context.Paintings.FirstOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Painting>(studentToUpdate,"",s => s.Painter, s => s.Title, s => s.Price))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists");
                }
            }
            return View(studentToUpdate);
        }

        // GET: Paintings/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var painting = await _context.Paintings
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (painting == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                "Delete failed. Try again";
            }

            return View(painting);
        }

        // POST: Paintings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var painting = await _context.Paintings.FindAsync(id);
            if (painting == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Paintings.Remove(painting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
            return RedirectToAction(nameof(Delete), new {id = id, saveChangesError = true });
            }
        }

        private bool PaintingExists(int id)
        {
            return _context.Paintings.Any(e => e.ID == id);
        }
    }
}
