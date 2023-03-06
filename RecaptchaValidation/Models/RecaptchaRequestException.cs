namespace RecaptchaValidation.Models
{
    public class RecaptchaRequestException : Exception 
    {
        public RecaptchaRequestException()
        {
        }
        public RecaptchaRequestException(string message)
            : base(message)
        {
        }
        public RecaptchaRequestException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
