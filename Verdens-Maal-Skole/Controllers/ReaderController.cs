using Microsoft.AspNetCore.Mvc;

namespace Verdens_Maal_Skole.Controllers
{
    [Route("data")]
    [ApiController]
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
            return Ok(manager.GetRoomNumbers());
        }


        //Get all reader data by room nr
        [HttpGet("room")]
        public IActionResult GetAllDataByRoomNr(string roomNr)
        {
            return Ok(manager.GetDataFromRoom(roomNr));
        }
    }
}
