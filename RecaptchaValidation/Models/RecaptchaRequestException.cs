namespace RecaptchaValidation.Models
{
    // Add the comment giving context to the exception here instead of the Service.
    public sealed class RecaptchaRequestException : Exception 
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
