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

        //Get all reader data
        [HttpGet("all")]
        public IActionResult GetAllData()
        {
            return Ok(manger.GetAllReaders());
        }

        //Get all room numbers
        [HttpGet("rooms")]
        public IActionResult GetAllRooms()
        {
            return Ok(manger.GetRoomNumbers());
        }


        //Get all reader data by room nr
        [HttpGet("room")]
        public IActionResult GetAllDataByRoomNr(int roomNr)
        {
            return Ok();
        }
    }
}
