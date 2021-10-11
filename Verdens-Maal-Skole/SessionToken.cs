using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Verdens_Maal_Skole
{
    public class SessionToken
    {
        public string Token { get; set; }

        public SessionToken(string token)
        {
            Token = token;
        }
    }
}
