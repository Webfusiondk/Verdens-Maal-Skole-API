using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Verdens_Maal_Skole.Controllers
{
    [Route("data")]
    [ApiController]
    public class ReaderController : Controller
    {
        ArduinoManager manger = new ArduinoManager();

        [HttpGet("all")]
        public IActionResult GetAllData()
        {
            return Ok(manger.GetAllReades());
        }
    }
}
