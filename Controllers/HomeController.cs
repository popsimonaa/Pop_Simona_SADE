using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pop_Simona_SADE.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Pop_Simona_SADE.Data;
using Pop_Simona_SADE.Models.ExhibitionViewModels;
using Microsoft.EntityFrameworkCore;

namespace Pop_Simona_SADE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ExhibitionContext _context;
        public HomeController(ExhibitionContext context)
        {
            _context = context;
        }
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Chat()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //o metoda Statistics care utilizeaza o interogare LINQ care grupeaza entitatile carti
        //dupa data comenzii, calculand numarul de carti comandate pentru fiecare data calendaristica
        //si salveaza rezultatul intr-o colectie OrderGroup
        public async Task<ActionResult> Statistics()
        {
            IQueryable<OrderGroup> data =
            from order in _context.Orders
            group order by order.OrderDate into dateGroup
            select new OrderGroup()
            {
                OrderDate = dateGroup.Key,
                PaintingCount = dateGroup.Count()
            };
            return View(await data.AsNoTracking().ToListAsync());
        }
    }
}
