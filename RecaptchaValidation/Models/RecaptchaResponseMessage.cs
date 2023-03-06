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
        [DataMember]
        public bool success { get; set; }
        [DataMember] 
        public decimal score { get; set; }
        [DataMember] 
        public string action { get; set; }
        [DataMember]
        public string challenge_ts { get; set; }
        [DataMember]
        public string hostname { get; set; }
        [DataMember(Name ="error-codes")]
        public string[] error_codes { get; set; }

    }
}
