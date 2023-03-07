namespace RecaptchaValidation.Extensions;


internal static class IServiceCollectionExtensions
{
    internal static IServiceCollection AddAllowAllOriginsCorsPolicy(this IServiceCollection @this) =>
        @this.AddCors(options => options
            .AddPolicy(Resources.AllowAllOriginsCorsPolicy, policy => policy.AllowAnyOrigin()));
}
