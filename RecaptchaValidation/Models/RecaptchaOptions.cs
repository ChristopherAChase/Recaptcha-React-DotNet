namespace RecaptchaValidation.Models
{
    public class RecaptchaOptions
    {
        public const string RecaptchaV3 = "RecaptchaV3";

        public string VerifyUrl { get; set; } = String.Empty;

        public KeysOptions Keys { get; set; }

    }

    public class KeysOptions
    {
        public const string Keys = "Keys";
        public string Site { get; set; } = String.Empty;
        public string Secret { get; set; } = String.Empty;
    }
}
