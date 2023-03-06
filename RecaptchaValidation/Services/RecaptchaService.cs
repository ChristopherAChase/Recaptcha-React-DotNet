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

        public RecaptchaService(HttpClient httpClient) {
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
                RecaptchaResponseMessage Response;

                string recaptchaVerificationUrl = RecaptchaRequestMessage.GetVerificationUrl(requestMessage);

                HttpResponseMessage verificationResponse = await _httpClient.PostAsync(recaptchaVerificationUrl, null).ConfigureAwait(false);
                
                verificationResponse.EnsureSuccessStatusCode();

                byte[] responseData = await verificationResponse.Content.ReadAsByteArrayAsync();

                using (MemoryStream ms = new MemoryStream(responseData))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RecaptchaResponseMessage));
                    Response = (RecaptchaResponseMessage)serializer.ReadObject(ms);
                }
                
                if (!Response.success)
                {
                    throw new RecaptchaRequestException($"Errors returned from Recaptcha Verification URL: {Response.error_codes}");
                }
                return Response;
            }
            catch (HttpRequestException hre)
            {
                //Handle Logging Here...
                Console.WriteLine(hre.ToString());
                return new RecaptchaResponseMessage()
                {
                    success = false,
                    challenge_ts = DateTime.UtcNow.ToString(),
                    hostname = "localhost", 
                    error_codes = new string[] { $"Error: Status Code {hre.StatusCode} | {hre.Message}" }
                };
            }
            catch (RecaptchaRequestException ex)
            {
                //Handle Logging Here...
                Console.WriteLine(ex.ToString());
                return new RecaptchaResponseMessage()
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
            catch (Exception ex)
            {
                //Handle Logging Here...
                Console.WriteLine(ex.ToString());
                return new RecaptchaResponseMessage()
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
        }
    }
}
