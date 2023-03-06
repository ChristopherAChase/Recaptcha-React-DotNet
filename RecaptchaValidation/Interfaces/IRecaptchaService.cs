namespace RecaptchaValidation.Interfaces
{
    public interface IRecaptchaService
    {
        IRecaptchaRequestMessage Request { get; set; }
        IRecaptchaResponseMessage Response { get; set; }
        void InitializeRequest(IRecaptchaRequestMessage request);
        Task<IRecaptchaResponseMessage> Execute();
    }
}
