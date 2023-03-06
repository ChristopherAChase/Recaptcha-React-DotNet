namespace RecaptchaValidation.Interfaces
{
    public interface IRecaptchaRequestMessage
    {
        string path { get; }
        string secret { get; }
        string response { get; set; }
        string remoteip { get; set; }

    }
}
