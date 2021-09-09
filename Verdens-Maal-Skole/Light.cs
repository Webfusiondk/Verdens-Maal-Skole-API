using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Verdens_Maal_Skole
{
    public class Light
    {
        public float Value { get; set; }
        public Light(int value)
        {
            Value = value;
            IsLightOn();
        }
        public Light(int value, bool lightIsOn)
        {
            Value = value;
            LightIsOn = lightIsOn;
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
