[<RequireQualifiedAccess>]
module internal RecaptchaRequestMessage

open System.Web

let fromOptions token remoteIp ({ VerifyUrl = url; Keys = keys }: RecaptchaOptions) =
    { Path = url
      Secret = keys.Secret
      Token = token
      RemoteIp = remoteIp }

let private encodeRequest request =
    let secret = sprintf "secret=%s" request.Secret
    let response = sprintf "response=%s" request.Token
    let remoteIp = sprintf "remoteip=%s" request.RemoteIp

    let result = sprintf "%s&%s&%s" secret response remoteIp
    HttpUtility.UrlPathEncode(result)

let verificationUrl request =
    let query = encodeRequest request
    sprintf "%s?%s" request.Path query