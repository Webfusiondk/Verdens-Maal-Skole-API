using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Verdens_Maal_Skole.Controllers
{
    public class ReaderController : Controller
    {
        private readonly ILogger<ReaderController> _logger;

        public ReaderController(ILogger<ReaderController> logger)
        {
            _logger = logger;
        }

        //[HttpGet]
        //public string[] Get()
        //{

        //}
    }
}
