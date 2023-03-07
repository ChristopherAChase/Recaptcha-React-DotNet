[<AutoOpen>]
module internal Types

type KeysOptions =
    { Site: string
      Secret: string }

type RecaptchaOptions =
    { VerifyUrl: string
      Keys: KeysOptions}

type RecaptchaRequestMessage =
    { Path: string
      Secret: string
      Token: string
      RemoteIp: string }

[<CLIMutable>]
type RecaptchaResponseMessage =
    { Success: bool
      Score: decimal option
      Action: string option
      ChallengeTimestamp: string
      HostName: string
      ErrorCodes: string[] }

type ValidateRecaptcha = RecaptchaRequestMessage -> Async<RecaptchaResponseMessage>