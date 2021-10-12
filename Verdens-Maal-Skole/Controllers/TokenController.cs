using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Verdens_Maal_Skole.Attributes;

namespace Verdens_Maal_Skole.Controllers
{
    [EnableCors("AllowSpecificOrigins")]
    [Route("token")]
    [ApiController]
    [ApiKey]
    public class TokenController : ControllerBase
    {
        TokenManager tokenManager = new TokenManager();

        /// <summary>
        /// Sends a freshly generated token to the user
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetToken")]
        public IActionResult GetToken()
        {
            return Ok(tokenManager.GenerateToken());
        }

        [HttpGet("Validate")]
        public IActionResult ValidateSession(string token)
        {
            return Ok(tokenManager.CheckForToken(token));
        }
            
         
        /// <summary>
        /// Updates a users session token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public IActionResult UpdateSession(string token)
        {
            return Ok(tokenManager.UpdateToken(token));
        }
    }
}
