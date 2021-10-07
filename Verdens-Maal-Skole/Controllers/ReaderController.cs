using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Verdens_Maal_Skole.Attributes;

namespace Verdens_Maal_Skole.Controllers
{
    [Route("data")]
    [ApiController]
    [ApiKey]
    public class ReaderController : Controller
    {
        ArduinoManager manager = new ArduinoManager();

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
