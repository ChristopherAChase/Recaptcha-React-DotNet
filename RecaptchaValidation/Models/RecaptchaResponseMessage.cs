using RecaptchaValidation.Interfaces;
using System.Runtime.Serialization;

namespace RecaptchaValidation.Models
{
    /* 
     * Used DataContract/DataMember because these are the names used 
     * by google's api request and response objects. Didn't want to deal 
     * with mapping or anything like that. 
     */
    [DataContract]
    public sealed class RecaptchaResponseMessage
    {
        [DataMember(Name ="success")]
        public bool Success { get; set; }
        [DataMember(Name ="score")] 
        public decimal Score { get; set; }
        [DataMember(Name ="action")] 
        public string Action { get; set; }
        [DataMember(Name ="challenge_ts")]
        public string ChallengeTimestamp { get; set; }
        [DataMember(Name ="hostname")]
        public string HostName { get; set; }
        [DataMember(Name ="error-codes")]
        public string[] ErrorCodes { get; set; }

    }
}
