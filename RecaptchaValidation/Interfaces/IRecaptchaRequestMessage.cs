namespace RecaptchaValidation.Interfaces
{
    // You don't need an interface for a data transfer object (DTO). 
    // I can imagine no other possible implementations of a simple object that can hold 4 strings. -z
    public interface IRecaptchaRequestMessage
    {
        string path { get; }
        string secret { get; }
        string response { get; set; }
        string remoteip { get; set; }
    }
}
