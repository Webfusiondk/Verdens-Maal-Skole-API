using Microsoft.AspNetCore.Mvc;
using Verdens_Maal_Skole.Attributes;

namespace Verdens_Maal_Skole.Controllers
{
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


        /// <summary>
        /// Gets a token from client to validate active session
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
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
        [HttpGet("Update")]
        public IActionResult UpdateSession(string token)
        {
            return Ok(tokenManager.UpdateToken(token));
        }
    }
}
