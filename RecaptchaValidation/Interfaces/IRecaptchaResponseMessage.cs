using System.Runtime.Serialization;

namespace RecaptchaValidation.Interfaces
{
    // You don't need an interface for a data transfer object (DTO). -z
    public interface IRecaptchaResponseMessage
    {
        bool success { get; set; }
        string challenge_ts { get; set; }
        string hostname { get; set; }
        string[] error_codes { get; set; }
    }
}
