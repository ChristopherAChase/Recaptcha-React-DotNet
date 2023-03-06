using RecaptchaValidation.Interfaces;
using System.Runtime.Serialization;

namespace RecaptchaValidation.Models
{
    /* 
     * Used DataContract/DataMember because these are the names used 
     * by google's api request and response objects. Didn't want to deal 
     * with mapping or anything like that. 
     */
     // You can use [DataMember(Name = "", Order = 0)] to handle name mapping. -z
     
    // Again, internal, sealed, and the interface isn't necesary for a data transfer object.
    [DataContract]
    internal sealed class RecaptchaResponseMessage: IRecaptchaResponseMessage
    {
        [DataMember]
        public bool success { get; set; }
        [DataMember]
        public string challenge_ts { get; set; }
        [DataMember]
        public string hostname { get; set; }
        [DataMember(Name ="error-codes")]
        public string[] error_codes { get; set; }

    }
}
