using RecaptchaValidation.Interfaces;
using RecaptchaValidation.Models;
using RecaptchaValidation.Services;

var builder = WebApplication.CreateBuilder(args);
var AllowAllOriginsCorsPolicy = "_AllowAllOriginsCorsPolicy";
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowAllOriginsCorsPolicy,
        policy =>
        {
            policy.AllowAnyOrigin();
        });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<IRecaptchaService, RecaptchaService>();
builder.Services.AddTransient<IRecaptchaService, RecaptchaService>();
builder.Services.Configure<RecaptchaOptions>(
    builder.Configuration.GetSection(RecaptchaOptions.RecaptchaV3));

var app = builder.Build();
app.UseCors(AllowAllOriginsCorsPolicy);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
