using Microsoft.Extensions.Options;
using RecaptchaValidation.Interfaces;
using System.Web;

namespace RecaptchaValidation.Models
{
    public class RecaptchaRequestMessage
    {
        public string path { get; private set; }
        public string secret { get; private set; }
        public string response { get; set; }
        public string remoteip { get; set; }

        public RecaptchaRequestMessage(string _response, string _remoteIp, IOptions<RecaptchaOptions> _options)
        {
            path = _options.Value.VerifyUrl;
            secret = _options.Value.Keys.Secret;
            response = _response;
            remoteip = _remoteIp;
        }

        public override string ToString()
        {
            return $"{path}?{HttpUtility.UrlPathEncode($"secret={secret}&response={response}&remoteip={remoteip}")}";
        }
    }
}
