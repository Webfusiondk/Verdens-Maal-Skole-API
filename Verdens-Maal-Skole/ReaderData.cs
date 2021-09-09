using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Verdens_Maal_Skole
{
    public class ReaderData
    {
        public string RoomNumber { get; set; }
        public DateTime Time { get; set; }
        public Temperature Temperature { get; set; }
        public Humidity Humidity { get; set; }
        public Light Light { get; set; }
        public ReaderData(string roomNumber,DateTime time ,Temperature temperature, Humidity humidity, Light light)
        {
            RoomNumber = roomNumber;
            Time = time;
            Temperature = temperature;
            Humidity = humidity;
            Light = light;

        }
            
    }
}
