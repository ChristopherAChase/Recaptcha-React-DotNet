using System.Runtime.Serialization;

namespace RecaptchaValidation.Models;

[DataContract]
public sealed class RecaptchaResponseMessage
{
    [DataMember(Name ="success")]
    public bool Success { get; set; }

    [DataMember(Name ="score")] 
    public decimal Score { get; set; }

    [DataMember(Name = "action")]
    public string Action { get; set; } = String.Empty;

    [DataMember(Name = "challenge_ts")]
    public string ChallengeTimestamp { get; set; } = String.Empty;

    [DataMember(Name = "hostname")]
    public string HostName { get; set; } = String.Empty;

    [DataMember(Name = "error-codes")]
    public string[] ErrorCodes { get; set; } = Array.Empty<string>();
}
