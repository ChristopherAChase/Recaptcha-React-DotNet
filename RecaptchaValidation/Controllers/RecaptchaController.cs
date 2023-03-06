using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RecaptchaValidation.Interfaces;
using RecaptchaValidation.Models;
using System.Text.Json;

namespace RecaptchaValidation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecaptchaController : ControllerBase
    {
        private IRecaptchaService _recaptcha { get; set; }
        private IOptions<RecaptchaOptions> _config { get; set; }
        
        public RecaptchaController(IRecaptchaService recaptcha, IOptions<RecaptchaOptions> recaptchaConfig) 
        { 
            _config = recaptchaConfig;
            _recaptcha = recaptcha;
        }

        // [FromQuery] is redundant, you don't need it here (but some people like it for explicitness).
        //   The way Action parameters work (by ASP.NET Core default) is:
        //    - Complex objects are taken from the request Body
        //    - Any named parameter parsed from the route ([HttpPost("/verify/{id}")]) is taken from the route
        //    - Anything else is taken from the request Query
        // Depending on your team standards, PascalCasing for method parameters is whack as fk. -z
        [HttpPost("Verify")]
        public async Task<IActionResult> Verify([FromQuery] string RecaptchaToken)
        {
            // I didn't realize that the request was being used in this way. Don't pass in the _config, just pass in the strings.
            // Also lol:
            //  IRecaptchaRequestMessage iRecaptchaRequestMessage = new RecaptchaRequestMessage().
            // Thank god we had fully qualified the type instead of using `var` or I would have never had any idea what this object was! -z
            IRecaptchaRequestMessage request = new RecaptchaRequestMessage(RecaptchaToken, HttpContext.Connection.RemoteIpAddress.ToString(), _config);

            // You are not using the `HttpClientFactory` here. You are using the transient service dependency. You can remove that other in the composition root. -z
            _recaptcha.InitializeRequest(request);

            IRecaptchaResponseMessage response = await _recaptcha.Execute();

            if(!response.success)
            {
                Console.WriteLine(_recaptcha.Response);
                return Ok(_recaptcha.Response);
            }
            
            return Ok($"Controller received token! { JsonSerializer.Serialize(_recaptcha.Response)}");
        }

    }
}
