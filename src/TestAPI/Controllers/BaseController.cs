using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAPI.Controllers
{
    public class MyBaseController : ControllerBase
    {
        public IActionResult CommunicateToEventGrid()
        {
            return Ok();
        }

        public IActionResult CommunicateToCosmosDB()
        {
            return Ok();
        }

        public IActionResult SendToAzureQueue(string message)
        {
            return Ok();
        }

        public IActionResult CommunicateToAzureFunction()
        {
            return Ok();
        }
    }
}
