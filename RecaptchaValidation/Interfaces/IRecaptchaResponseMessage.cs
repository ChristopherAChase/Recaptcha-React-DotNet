using System.Runtime.Serialization;

namespace RecaptchaValidation.Interfaces
{
    public interface IRecaptchaResponseMessage
    {
        bool success { get; set; }
        string challenge_ts { get; set; }
        string hostname { get; set; }
        string[] error_codes { get; set; }
    }
}
