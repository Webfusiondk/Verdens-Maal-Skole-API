using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Verdens_Maal_Skole
{
    public class Light : ReaderData
    {
        public Light(string description, int value, DateTime time)
        {
            Time = time;
            Value = value;
            Description = description;
        }

        public string Description { get; set; }

    }
}
