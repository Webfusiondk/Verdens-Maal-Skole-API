using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Verdens_Maal_Skole.Attributes;

namespace Verdens_Maal_Skole.Controllers
{
    [Route("token")]
    [ApiController]
    [ApiKey]
    public class TokenController : ControllerBase
    {
        TokenManager tokenManager = new TokenManager();
        [HttpGet("GetToken")]
        public IActionResult GetToken()
        {
            return Ok(tokenManager.GenerateToken());
        }

        [HttpGet("Validate")]
        public IActionResult ValidateSession(string token)
        {
            SessionToken sessionToken = new SessionToken(token);
            return Ok(tokenManager.ValidateToken(sessionToken));
        }
            
        [HttpPost("Update")]
        public IActionResult UpdateSession(string token)
        {
            return Ok(tokenManager.UpdateToken(token));
        }
    }
}
