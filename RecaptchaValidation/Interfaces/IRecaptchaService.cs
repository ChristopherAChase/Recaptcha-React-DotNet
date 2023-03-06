using System.Threading.Tasks;
using RecaptchaValidation.Models;

namespace RecaptchaValidation.Interfaces
{
    public interface IRecaptchaService
    {
        RecaptchaRequestMessage Request { get; set; }
        RecaptchaResponseMessage Response { get; set; }
        void InitializeRequest(RecaptchaRequestMessage request);
        Task<RecaptchaResponseMessage> Execute();
    }
}
