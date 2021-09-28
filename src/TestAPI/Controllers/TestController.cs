using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : MyBaseController
    {
        [HttpPost("send")]
        public IActionResult SendMessage([FromBody] string message)
        {
            return SendMessage(message);            
        }
    }
}
