using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Verdens_Maal_Skole
{
    public class Celsius : ReaderData
    {
        public Celsius(float value, DateTime time)
        {
            Value = value;
            Time = time;
        }
    }
}
