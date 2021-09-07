using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Verdens_Maal_Skole
{
    public class Temperature : ReaderData
    {
        public Temperature(float value, DateTime time, string roomNumber)
        {
            RoomNumber = roomNumber;
            Value = value;
            Time = time;
        }
    }
}
