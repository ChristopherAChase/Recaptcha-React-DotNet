using Microsoft.Extensions.Options;
using RecaptchaValidation.Interfaces;
using System.Runtime.Serialization;
using System.Web;

namespace RecaptchaValidation.Models
{
    [DataContract]
    public sealed class RecaptchaRequestMessage
    {
        public string Path { get; private set; }
        public string Secret { get; private set; }
        public string Response { get; set; }
        public string RemoteIp { get; set; }

        public RecaptchaRequestMessage(string responseToken, string remoteIPAddress, string secretKey, string verifyUrl)
        {
            Path = verifyUrl;
            Secret = secretKey;
            Response = responseToken;
            RemoteIp = remoteIPAddress;
        }

        public static string GetVerificationUrl(RecaptchaRequestMessage requestMessage)
        {
            return $"{requestMessage.Path}?{HttpUtility.UrlPathEncode($"secret={requestMessage.Secret}&response={requestMessage.Response}&remoteip={requestMessage.RemoteIp}")}";
        }
        
    }
}
