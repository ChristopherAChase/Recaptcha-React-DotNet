using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RecaptchaValidation.Interfaces;
using RecaptchaValidation.Models;
using System;
using System.Text.Json;
using System.Threading.Tasks;

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

        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromQuery] string recaptchaToken)
        {
            var request = new RecaptchaRequestMessage(recaptchaToken, HttpContext.Connection.RemoteIpAddress.ToString(), _config.Value.Keys.Secret, _config.Value.VerifyUrl);

            RecaptchaResponseMessage response = await _recaptcha.ExecuteAsync(request);

            if(!response.Success)
            {
                Console.WriteLine(response);
                return Ok(response);
            }
            
            return Ok($"Controller received token! { JsonSerializer.Serialize(response)}");
        }

    }
}
