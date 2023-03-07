using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RecaptchaValidation.Interfaces;
using RecaptchaValidation.Models;
using System.Text.Json;

namespace RecaptchaValidation.Controllers;

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
        var secret = _config.Value.Keys.Secret;
        var verificationUrl = _config.Value.VerifyUrl;
        var remoteIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? throw new InvalidOperationException($"The Remote IP Address for the connection was null.");
        var request = new RecaptchaRequestMessage(recaptchaToken, remoteIpAddress, secret, verificationUrl);

        var response = await _recaptcha.ExecuteAsync(request);

        if(!response.Success)
        {
            var message = $"Successfully received a response from the recaptcha API, but payload indicated failure with errors: \n{response}";
            Console.WriteLine(message);
            return Problem(message, $"/api/recaptcha/verify?recaptchaToken={recaptchaToken}", 500, "Unsuccessful validation through the Recaptcha API");
        }
        
        return Ok($"Controller received token! \n\n{JsonSerializer.Serialize(response)}");
    }

}
