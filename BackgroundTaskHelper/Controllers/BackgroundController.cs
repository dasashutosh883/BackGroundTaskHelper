using BackgroundTaskHelper.Configuration;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundTaskHelper.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BackgroundController : ControllerBase
    {
        private readonly PeriodicHostedService _service;
		private readonly IAntiforgery _antiforgery;

		public BackgroundController(PeriodicHostedService service, IAntiforgery antiforgery)
        {
            _service = service;
            _antiforgery=antiforgery;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            return Ok("Hello World!");
        }

        [HttpGet("GetServiceStatus")]
        public IActionResult GetBackground()
        {
            var state = new PeriodicHostedServiceState(_service.IsEnabled,_service.TimePeriod);
            return Ok(state);
        }
		[HttpGet("GetToken")]
		public IActionResult GetAntiforgeryToken()
		{
			var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
			string Token= tokens.RequestToken!;
			return Ok(Token);
		}
		[HttpPatch("SetServiceStatus")]
        public IActionResult UpdateBackground([FromBody] PeriodicHostedServiceState state)
        {
            _service.IsEnabled=state.IsEnabled;
            _service.TimePeriod = state.TimePeriod;
            string response = string.Empty;
            if (state.IsEnabled)
            {
                response = $"Service is running with interval of {state.TimePeriod} Minutes";
            }
            else
            {
                response = $"Service is stopped on {DateTime.Now}";
            }
            return Ok(response);
        }
       
    }
}
