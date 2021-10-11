using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Verdens_Maal_Skole.Controllers
{
    [Route("token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        TokenManager tokenManager = new TokenManager();
        [HttpGet("GetToken")]
        public IActionResult GetToken()
        {
            return Ok(tokenManager.GenerateToken());
        }

        public IActionResult UpdateSession()
        {
            return Ok();
        }
    }
}
