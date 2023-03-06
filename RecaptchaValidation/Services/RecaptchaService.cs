using RecaptchaValidation.Interfaces;
using RecaptchaValidation.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace RecaptchaValidation.Services
{
    public class RecaptchaService : IRecaptchaService
    {
        public HttpClient _httpClient { get; set; }

        public RecaptchaService(HttpClient httpClient) {
            _httpClient= httpClient;
        }

        public async Task<RecaptchaResponseMessage> ExecuteAsync(RecaptchaRequestMessage requestMessage)
        {
            try 
            {
                RecaptchaResponseMessage Response;

                string recaptchaVerificationUrl = RecaptchaRequestMessage.GetVerificationUrl(requestMessage);

                HttpResponseMessage verificationResponse = await _httpClient.PostAsync(recaptchaVerificationUrl, null).ConfigureAwait(false);
                
                verificationResponse.EnsureSuccessStatusCode();

                byte[] responseData = await verificationResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

                using (MemoryStream ms = new MemoryStream(responseData))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RecaptchaResponseMessage));
                    Response = (RecaptchaResponseMessage)serializer.ReadObject(ms);
                }
                
                if (!Response.Success)
                {
                    throw new RecaptchaRequestException($"Errors returned from Recaptcha Verification URL: {string.Join(',', Response?.ErrorCodes)}");
                }
                return Response;
            }
            catch (HttpRequestException hre)
            {
                //Handle Logging Here...
                Console.WriteLine(hre.ToString());
                return new RecaptchaResponseMessage()
                {
                    Success = false,
                    ChallengeTimestamp = DateTimeOffset.UtcNow.ToString(),
                    HostName = "localhost", 
                    ErrorCodes = new string[] { $"Error: Status Code {hre.StatusCode} | {hre.Message}" }
                };
            }
            catch (RecaptchaRequestException ex)
            {
                //Handle Logging Here...
                Console.WriteLine(ex.ToString());
                return new RecaptchaResponseMessage()
                {
                    Success = false,
                    ChallengeTimestamp = DateTimeOffset.UtcNow.ToString(),
                    HostName = ex.Source,
                    ErrorCodes = new string[]
                        {
                            $"Error: {ex.Message}"
                        }
                };
            }
            catch (Exception ex)
            {
                //Handle Logging Here...
                Console.WriteLine(ex.ToString());
                return new RecaptchaResponseMessage()
                {
                    Success = false,
                    ChallengeTimestamp = DateTimeOffset.UtcNow.ToString(),
                    HostName = ex.Source,
                    ErrorCodes = new string[]
                        {
                            $"Error: {ex.Message} | {ex.StackTrace}"
                        }
                };
            }
        }
    }
}
