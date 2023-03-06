using System.Threading.Tasks;
using RecaptchaValidation.Models;

namespace RecaptchaValidation.Interfaces
{
    public interface IRecaptchaService
    {
        RecaptchaResponseMessage Response { get; set; }
        Task<RecaptchaResponseMessage> Execute(RecaptchaRequestMessage requestMessage);
    }
}
