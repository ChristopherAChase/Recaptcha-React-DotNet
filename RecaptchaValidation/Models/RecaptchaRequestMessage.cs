using System.Runtime.Serialization;
using System.Web;

namespace RecaptchaValidation.Models;

[DataContract]
internal sealed class RecaptchaRequestMessage
{
    public string Path { get; }
    public string Secret { get; }
    public string Response { get; }
    public string RemoteIp { get; }

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
