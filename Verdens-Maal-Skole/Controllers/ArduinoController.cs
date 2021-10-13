using Microsoft.AspNetCore.Mvc;
using Verdens_Maal_Skole.Attributes;

namespace Verdens_Maal_Skole.Controllers
{
    [Route("arduino")]
    [ApiController]
    [ApiKey]
    public class ArduinoController : ControllerBase
    {
        ArduinoManager manager = new ArduinoManager();

        /// <summary>
        /// Receives http data from arduino
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ProvideReading(string data)
        {
            System.Console.WriteLine("HttpPost Data: " + data);
            return Ok(manager.SaveArduinoData(data));
        }

    }
}
