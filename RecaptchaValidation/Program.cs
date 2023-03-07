using RecaptchaValidation;
using RecaptchaValidation.Extensions;
using RecaptchaValidation.Interfaces;
using RecaptchaValidation.Models;
using RecaptchaValidation.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAllowAllOriginsCorsPolicy();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IRecaptchaService, RecaptchaService>();
builder.Services.Configure<RecaptchaOptions>(
    builder.Configuration.GetSection(RecaptchaOptions.RecaptchaV3));

var app = builder.Build();

app.UseCors(Resources.AllowAllOriginsCorsPolicy);

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
