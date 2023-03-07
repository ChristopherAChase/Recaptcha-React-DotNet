using RecaptchaValidation.Models;

namespace RecaptchaValidation.Interfaces;

public interface IRecaptchaService
{
    Task<RecaptchaResponseMessage> ExecuteAsync(RecaptchaRequestMessage requestMessage);
}
