using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Verdens_Maal_Skole.Controllers
{
    [Route("arduino")]
    [ApiController]
    public class ArduinoController : ControllerBase
    {
        ArduinoManager manager = new ArduinoManager();

        [HttpPost]
        public IActionResult ProvideReading(string data)
        {
            System.Console.WriteLine("HttpPost Data: " + data);
            return Ok(manager.SaveArduinoData(data));
        }

    }
}
