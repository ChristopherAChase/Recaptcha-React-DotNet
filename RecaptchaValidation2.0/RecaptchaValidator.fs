[<RequireQualifiedAccess>]
module internal RecaptchaValidator

open System
open System.Net.Http
open System.Text.Json
open FSharpPlus
open System.Threading.Tasks

let private asTask<'a> (valueTask: ValueTask<'a>) = valueTask.AsTask()

let validateRecaptcha: (unit -> DateTimeOffset) -> HttpClient -> ValidateRecaptcha = fun getNow httpClient request ->
    let verificationUrl = RecaptchaRequestMessage.verificationUrl request
    async {
        let! responseMessage =
            httpClient.PostAsync(verificationUrl, null)
            |> Task.map (fun result -> result.EnsureSuccessStatusCode())
            |> Task.bind (fun result -> result.Content.ReadAsStreamAsync())
            |> Task.bind (JsonSerializer.DeserializeAsync<RecaptchaResponseMessage> >> asTask)
            |> Async.AwaitTask

        return responseMessage
    }