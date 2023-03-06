using Microsoft.Extensions.Options;
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
        public RecaptchaResponseMessage Response { get; set; }

        public RecaptchaService(HttpClient httpClient, IOptions<RecaptchaOptions> recaptchaOptions) {
            _httpClient= httpClient;
        }

        public Task<byte[]> GetResponseContentAsByteArray(HttpResponseMessage responseMessage)
        {
            return responseMessage.Content.ReadAsByteArrayAsync();
        }

        public async Task<RecaptchaResponseMessage> Execute(RecaptchaRequestMessage requestMessage)
        {
            try 
            { 
                string recaptchaVerificationUrl = RecaptchaRequestMessage.GetVerificationUrl(requestMessage);

                HttpResponseMessage verificationResponse = await _httpClient.PostAsync(recaptchaVerificationUrl, null);
                
                verificationResponse.EnsureSuccessStatusCode();

                byte[] responseData = await verificationResponse.Content.ReadAsByteArrayAsync();

                string logResponseData = await verificationResponse.Content.ReadAsStringAsync();

                using (MemoryStream ms = new MemoryStream(responseData))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RecaptchaResponseMessage));
                    Response = (RecaptchaResponseMessage)serializer.ReadObject(ms);
                }
                
                if (!Response.success)
                {
                    throw new RecaptchaRequestException();
                }
                return Response;
            }
            catch (HttpRequestException hre)
            {
                //Handle Logging Here...
                Console.WriteLine(hre.ToString());
                Response = new RecaptchaResponseMessage()
                {
                    success = false,
                    challenge_ts = DateTime.UtcNow.ToString(),
                    hostname = "localhost", 
                    error_codes = new string[] { $"Error: Status Code {hre.StatusCode} | {hre.Message}" }
                };
                return Response;
            }
            catch (RecaptchaRequestException ex)
            {

                /*  Here are the possible "business" level codes:
                    missing-input-secret    The secret parameter is missing.
                    invalid-input-secret    The secret parameter is invalid or malformed.
                    missing-input-response  The response parameter is missing.
                    invalid-input-response  The response parameter is invalid or malformed.
                    bad-request             The request is invalid or malformed.
                    timeout-or-duplicate    The response is no longer valid: either is too old or has been used previously.
                */

                //Handle Logging Here...
                Console.WriteLine(ex.ToString());
                return Response;
            }
            catch (Exception ex)
            {
                //Handle Logging Here...
                Console.WriteLine(ex.ToString());
                if (Response == null)
                {
                    Response = new RecaptchaResponseMessage()
                    {
                        success = false,
                        challenge_ts = DateTime.UtcNow.ToString(),
                        hostname = ex.Source,
                        error_codes = new string[]
                        {
                            $"Error: {ex.Message} | {ex.StackTrace}"
                        }
                    };
                }
                return Response;
            }
        }
    }
}
