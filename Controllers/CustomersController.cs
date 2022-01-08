using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Pop_Simona_SADE.Data;

namespace Pop_Simona_SADE.Controllers
{
    [Authorize(Policy = "SalesManager")]
    public class CustomersController : Controller
    {
        private readonly ExhibitionContext _context;
        private string _baseUrl = "http://localhost:58730/api/Customers";
        public CustomersController(ExhibitionContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
