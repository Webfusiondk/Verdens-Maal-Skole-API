using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Verdens_Maal_Skole
{
    public class TokenManager
    {

        /// <summary>
        /// Generates a new token
        /// </summary>
        /// <returns></returns>
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
        public int ValidateToken(SessionToken token)
        {
        }

        /// <summary>
        /// Updates a valid token's expiration time
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public int UpdateToken(string token)
        {
            return DataAccess.UpdateSessionToken(token);
        }

        /// <summary>
        /// Checks if a token already exists in the database
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool CheckForToken(string token)
        {
            if(DataAccess.SelectToken(token) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
