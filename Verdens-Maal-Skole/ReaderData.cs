using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Verdens_Maal_Skole
{
    public abstract class ReaderData
    {
        public string RoomNumber { get; set; }
        public float Value { get; set; }
        public DateTime Time { get; set; }
            
    }
}
