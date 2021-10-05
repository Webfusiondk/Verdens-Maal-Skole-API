using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Verdens_Maal_Skole.Controllers
{
    [Route("data")]
    [ApiController]
    public class ReaderController : Controller
    {
        ArduinoManager manager = new ArduinoManager();

        [HttpPost("providereading")]
        public async Task<ActionResult> ProvideReading(string data)
        {



            return StatusCode(200);
        }


        //Get all reader data
        [HttpGet("all")]
        public IActionResult GetAllData()
        {
            return Ok(manager.GetAllReaders());
        }


        //Get all room numbers
        [HttpGet("rooms")]
        public IActionResult GetAllRooms()
        {
            return Ok(manager.GetAllRoomNumbers());
        }


        //Get all reader data by room nr
        [HttpGet("room")]
        public IActionResult GetAllDataByRoomNr(string roomNr)
        {
            return Ok(manager.GetReaderDataByRoomNr(roomNr));
        }
    }
}
