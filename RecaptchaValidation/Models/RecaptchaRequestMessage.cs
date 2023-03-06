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

        public RecaptchaRequestMessage(string responseToken, string remoteIPAddress, string secretKey, string verifyUrl)
        {
            path = verifyUrl;
            secret = secretKey;
            response = responseToken;
            remoteip = remoteIPAddress;
        }

        public override string ToString()
        {
            return $"{path}?{HttpUtility.UrlPathEncode($"secret={secret}&response={response}&remoteip={remoteip}")}";
        }
    }
}
