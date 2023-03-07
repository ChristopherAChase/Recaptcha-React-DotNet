namespace RecaptchaValidationFSharp.Controllers

open System
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Options
open Microsoft.Extensions.Logging
open System.Text.Json

[<ApiController>]
[<Route("api/[controller]")>]
type internal RecaptchaController (validate: ValidateRecaptcha, config: IOptions<RecaptchaOptions>, logger: ILogger<RecaptchaController>) =
    inherit ControllerBase()

    let toStringOrDefault =
        Option.ofObj
        >> Option.map string
        >> Option.defaultValue String.Empty

    [<HttpPost("verify")>]
    member this.Verify([<FromQuery>] recaptchaToken: string) =
        let remoteIp = this.HttpContext.Connection.RemoteIpAddress |> toStringOrDefault

        let request = RecaptchaRequestMessage.fromOptions recaptchaToken remoteIp config.Value

        async {
            let! response = validate request

            match response.Success with
            | false ->
                let message = sprintf "Successfully received a response from the recaptcha API but playload indicated failure with errors: \n%A" response.ErrorCodes
                logger.LogWarning(message, response)

                return this.Problem(message)
            | true ->
                let json = JsonSerializer.Serialize(response)
                let message = sprintf "Controller received token! \n\n %s" json

                return this.Ok(message)
        } |> Async.StartAsTask
