using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Management.Automation;

namespace PowershellWebService.Controllers
{
    public class CommandByScriptQueryObject
    {
        public string script { get; set; }
        public string arguments { get; set; }
    }

    [Route("[controller]")]
    [ApiController]
    public class PowershellCommandController : ControllerBase
    {

        private readonly ILogger<PowershellCommandController> _logger;

        public PowershellCommandController(ILogger<PowershellCommandController> logger)
        {
            _logger = logger;
        }

        [HttpGet("byscript")]
        public IActionResult Get([FromQuery] CommandByScriptQueryObject request)
        {
            return Ok($"script = {request.script}, arguments = {request.arguments}");
        }

        public async Task<Array> RunScript(string scriptContents, Dictionary<string, object> scriptParameters)
        {
            using (PowerShell ps = PowerShell.Create())
            {
                ps.AddScript(scriptContents);
                ps.AddParameters(scriptParameters);
                var pipelineObjects = await ps.InvokeAsync().ConfigureAwait(false);

                return pipelineObjects.ToArray();
            }
        }
    }
}
