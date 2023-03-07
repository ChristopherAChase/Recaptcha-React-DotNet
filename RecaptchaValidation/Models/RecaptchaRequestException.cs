namespace RecaptchaValidation.Models;

/*  Here are the possible "business" level codes:
    missing-input-secret    The secret parameter is missing.
    invalid-input-secret    The secret parameter is invalid or malformed.
    missing-input-response  The response parameter is missing.
    invalid-input-response  The response parameter is invalid or malformed.
    bad-request             The request is invalid or malformed.
    timeout-or-duplicate    The response is no longer valid: either is too old or has been used previously.
*/
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
