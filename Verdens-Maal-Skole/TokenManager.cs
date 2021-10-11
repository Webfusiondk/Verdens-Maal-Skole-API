using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Verdens_Maal_Skole
{
    public class TokenManager
    {

        public SessionToken GenerateToken()
        {
            string token = "";
            for (int i = 0; i < 5; i++)
            {
                token += Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            }
            token = Regex.Replace(token, @"[^0-9a-zA-Z]+", "");
            DataAccess.PostToken(token);
            return new SessionToken(token);
        }

        public int UpdateToken(string token)
        {
            return DataAccess.UpdateSessionToken(token);
        }
    }
}
