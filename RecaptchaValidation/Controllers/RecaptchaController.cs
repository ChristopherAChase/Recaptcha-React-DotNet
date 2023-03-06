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

        [HttpPost("Verify")]
        public async Task<IActionResult> Verify([FromQuery] string RecaptchaToken)
        {
            IRecaptchaRequestMessage request = new RecaptchaRequestMessage(RecaptchaToken, HttpContext.Connection.RemoteIpAddress.ToString(), _config);

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
