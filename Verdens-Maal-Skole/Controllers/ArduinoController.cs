using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Verdens_Maal_Skole.Controllers
{
    public class ArduinoController : ApiController
    {

        [HttpPost]
        public string ProvideReading(string data)
        {

            System.Console.WriteLine(data);
            System.Console.WriteLine("Got a ProvideReading Post");
            return data;
        }

    }
}
