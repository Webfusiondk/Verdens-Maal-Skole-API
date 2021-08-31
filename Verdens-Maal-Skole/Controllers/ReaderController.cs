using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Verdens_Maal_Skole.Controllers
{
    public class ReaderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
