using Microsoft.Extensions.Options;
using RecaptchaValidation.Interfaces;
using System.Web;

namespace RecaptchaValidation.Models
{
    // This file isn't being used outside of the assembly. It is also something you don't want people inheriting from.
    // Use _internal_ to protect this message from being utilized by other projects.
    // Use _sealed_ to protect from the awfulness which is inheritance.
    internal sealed class RecaptchaRequestMessage : IRecaptchaRequestMessage
    {
        // Traditionally, properties are PascalCased and fields are _camelCased.
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

        // ToString() is typically used for getting a representation of a C# object.
        // I can see the use here, but I would prefer a utility function that is more descriptive.
        // public string GetVerificationUrl().
        // Or even better, a pure function:
        // public static string GetVerificationUrl(RecaptchaRequestMessage message) => {};
        public override string ToString()
        {
            return $"{path}?{HttpUtility.UrlPathEncode($"secret={secret}&response={response}&remoteip={remoteip}")}";
        }
    }
}
