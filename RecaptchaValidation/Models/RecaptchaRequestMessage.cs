using Microsoft.Extensions.Options;
using RecaptchaValidation.Interfaces;
using System.Web;

namespace RecaptchaValidation.Models
{
    public sealed class RecaptchaRequestMessage
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

        public static string GetVerificationUrl(RecaptchaRequestMessage requestMessage)
        {
            return $"{requestMessage.path}?{HttpUtility.UrlPathEncode($"secret={requestMessage.secret}&response={requestMessage.response}&remoteip={requestMessage.remoteip}")}";
        }
        
    }
}
