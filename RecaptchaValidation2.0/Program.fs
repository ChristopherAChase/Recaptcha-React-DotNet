namespace RecaptchaValidationFSharp

open System.Net.Http

#nowarn "20"
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

module Program =
    let exitCode = 0

    let getNow () = System.DateTimeOffset.Now

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)

        builder.Services.AddCors(fun o -> o.AddPolicy("_AllowAllOriginsCorsPolicy", fun p -> p.AllowAnyOrigin() |> ignore))

        let config = builder.Configuration
        builder.Services.Configure<RecaptchaOptions>(config.GetSection("RecaptchaV3"))

        builder.Services.AddTransient<ValidateRecaptcha>(System.Func<_,_>(
            fun sp ->
                let client = sp.GetRequiredService<HttpClient>()
                RecaptchaValidator.validateRecaptcha getNow client))

        builder.Services.AddControllers()

        let app = builder.Build()

        app.UseHttpsRedirection()

        app.UseAuthorization()
        app.MapControllers()

        app.Run()

        exitCode
