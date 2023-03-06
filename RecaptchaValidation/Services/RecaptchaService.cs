using Microsoft.Extensions.Options;
using RecaptchaValidation.Interfaces;
using RecaptchaValidation.Models;
using System.Runtime.Serialization.Json;

namespace RecaptchaValidation.Services
{
    internal sealed class RecaptchaService : IRecaptchaService
    {
        public HttpClient _httpClient { get; set; }
        public IRecaptchaRequestMessage Request {get; set; }
        public IRecaptchaResponseMessage Response { get; set; }

        // The AddHttpClient in the composition root is for using a class to wrap an HttpClient with a bunch of pre-configured settings such as:
        //  - BaseAddress
        //  - Headers
        //  - TimeOut
        //  - Retry
        // You aren't doing that here, you just want the HTTP client to use by default.
        // If you wanted a special class to use as an HttpClient, you'd create a RecaptchaHttpClient class that
        // takes in an HttpClient and configures that stuff in the constructor. Then in this class you'd pull in the
        // RecaptchaHttpClient and use it _instead_ of the HttpClient you have currently. -z
        public RecaptchaService(HttpClient httpClient, IOptions<RecaptchaOptions> recaptchaOptions) {
            _httpClient= httpClient;
        }

        // Get rid of this method. You are intentionally mutating the class rather than just pass the RecaptchaRequestMessage to the 
        // Execute function where it is needed.
        // All this method does is allow someone to forget to call it, and then you call Execute and get a NullReferenceException.
        // Bad bad bad bad bad bad. -z
        public void InitializeRequest(IRecaptchaRequestMessage request)
        {
            Request = request;
        }

        // This member is set to _public_ but it doesn't exist on the interface.
        // That means the only time that the rest of the code is able to call this is if this class is used AS the RecaptchaService implementation
        // and NOT the interface. This does not occur in this code base.
        // This should be a private method or a local method if it needs to exist at all. -z
        public Task<byte[]> GetResponseContentAsByteArray(HttpResponseMessage responseMessage)
        {
            return responseMessage.Content.ReadAsByteArrayAsync();
        }

        // This needs to take in the request as a method parameter.
        public async Task<IRecaptchaResponseMessage> Execute(RecaptchaRequestMessage request)
        {
            try 
            { 
                // ToString() and GetVerificationUrl() are different. ToString() is intended to be a representation of that full object.
                // In this case, because it is a data transfer object, I would expect to call ToString() and get information on all public members.
                // Create an extension method or a static method to get this data instead.
                string recaptchaVerificationUrl = request.ToString();

                // Asynchronous calls are handled by default in ASP.NET Core to not care about the synchronization context in which they were called.
                // All Controllers have this built in by default.
                // This is not always the case. 
                // And since this Service does not know about where it is called from, it is best practice to append .ConfigureAwait(false) to all
                // asynchronous calls.
                HttpResponseMessage verificationResponse = await _httpClient.PostAsync(recaptchaVerificationUrl, null).ConfigureAwait(false);
                
                verificationResponse.EnsureSuccessStatusCode();

                byte[] responseData = await verificationResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

                string logResponseData = await verificationResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                using (MemoryStream ms = new MemoryStream(responseData))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RecaptchaResponseMessage));
                    Response = (RecaptchaResponseMessage)serializer.ReadObject(ms);
                }
                
                // This is already done by EnsureSuccessStatusCode above.
                if (!Response.success)
                {
                    throw new RecaptchaRequestException();
                }
                
                // No need for the weird mutation of the Response property. Just return the damn thing here. -z
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
                // This comment/code can exist in the RecaptchaRequestException file.
                // This file doesn't care about teaching the user about the RecaptchaRequestException. -z
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
