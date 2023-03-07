using RecaptchaValidation.Interfaces;
using RecaptchaValidation.Models;
using System.Text.Json;

namespace RecaptchaValidation.Services;

internal sealed class RecaptchaService : IRecaptchaService
{
    public HttpClient _httpClient { get; set; }

    public RecaptchaService(HttpClient httpClient) {
        _httpClient= httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<RecaptchaResponseMessage> ExecuteAsync(RecaptchaRequestMessage requestMessage)
    {
        try 
        {
            var recaptchaVerificationUrl = RecaptchaRequestMessage.GetVerificationUrl(requestMessage);

            var verificationResponse = await _httpClient.PostAsync(recaptchaVerificationUrl, null).ConfigureAwait(false);
            
            verificationResponse.EnsureSuccessStatusCode();

            var responseData = await verificationResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

            using var stream = await verificationResponse.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var result = await JsonSerializer.DeserializeAsync<RecaptchaResponseMessage>(stream).ConfigureAwait(false);

            if (result is null)
            {
                throw new RecaptchaRequestException($"The resulting {nameof(RecaptchaResponseMessage)} was found to be null.");
            }

            if (!result.Success)
            {
                throw new RecaptchaRequestException($"Errors returned from Recaptcha Verification URL: {string.Join(',', result.ErrorCodes)}");
            }

            return result;
        }
        catch (HttpRequestException exn)
        {
            //Handle Logging Here...
            Console.WriteLine(exn.ToString());
            return new RecaptchaResponseMessage()
            {
                Success = false,
                ChallengeTimestamp = DateTimeOffset.UtcNow.ToString(),
                HostName = "localhost", 
                ErrorCodes = new string[] { $"Error: Status Code {exn.StatusCode} | {exn.Message}" }
            };
        }
        catch (RecaptchaRequestException exn)
        {
            //Handle Logging Here...
            Console.WriteLine(exn.ToString());
            return new RecaptchaResponseMessage()
            {
                Success = false,
                ChallengeTimestamp = DateTimeOffset.UtcNow.ToString(),
                HostName = exn?.Source ?? "No host name provided.",
                ErrorCodes = new string[]
                {
                    $"Error: {exn?.Message ?? "No message provided."}"
                }
            };
        }
        catch (Exception exn)
        {
            //Handle Logging Here...
            Console.WriteLine(exn.ToString());
            return new RecaptchaResponseMessage()
            {
                Success = false,
                ChallengeTimestamp = DateTimeOffset.UtcNow.ToString(),
                HostName = exn?.Source ?? "No host name provided.",
                ErrorCodes = new string[]
                {
                    $"Error: {exn ?.Message ?? "No message provided." } | {exn?.StackTrace ?? "No stack trace provided."}"
                }
            };
        }
    }
}
