using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Verdens_Maal_Skole.Attributes;

namespace Verdens_Maal_Skole.Controllers
{
    [Route("arduino")]
    [ApiController]
    [ApiKey]
    public class ArduinoController : ControllerBase
    {
        ArduinoManager manager = new ArduinoManager();

        [HttpGet]
        public IActionResult ProvideReading(string data)
        {
            System.Console.WriteLine("HttpPost Data: " + data);
            return Ok(manager.SaveArduinoData(data));
        }

    }
}
