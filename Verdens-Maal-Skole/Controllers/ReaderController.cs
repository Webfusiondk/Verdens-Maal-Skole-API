using Microsoft.AspNetCore.Mvc;
using Verdens_Maal_Skole.Attributes;

namespace Verdens_Maal_Skole.Controllers
{
    [Route("data")]
    [ApiController]
    [ApiKey]
    public class ReaderController : Controller
    {
        ArduinoManager manager = new ArduinoManager();

        /// <summary>
        /// Get all reader data
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public IActionResult GetAllData()
        {
            return Ok(manager.GetAllReaders());
        }


        /// <summary>
        /// Get all room numbers
        /// </summary>
        /// <returns></returns>
        [HttpGet("rooms")]
        public IActionResult GetAllRooms()
        {
            return Ok(manager.GetAllRoomNumbers());
        }


        /// <summary>
        /// Get all reader data by room nr
        /// </summary>
        /// <param name="roomNr"></param>
        /// <returns></returns>
        [HttpGet("room")]
        public IActionResult GetAllDataByRoomNr(string roomNr)
        {
            return Ok(manager.GetReaderDataByRoomNr(roomNr));
        }
    }
}
