using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Verdens_Maal_Skole
{
    public class Light : ReaderData
    {
        public Light(int value, DateTime time)
        {
            Time = time;
            Value = value;
            IsLightOn();
        }

        public bool LightIsOn { get; set; }

        void IsLightOn()
        {
            if (Value < 500)
            {
                LightIsOn = false;
            }
            else
                LightIsOn = true;
        }
    }
}
