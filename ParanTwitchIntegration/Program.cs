using Microsoft.Extensions.Options;
using ParanTwitchIntegration.Application.Helpers;
using ParanTwitchIntegration.Application.Services;
using ParanTwitchIntegration.Application.Services.Twitch;
using ParanTwitchIntegration.Domain.Interfaces;
using ParanTwitchIntegration.Domain.Interfaces.Twitch;
using ParanTwitchIntegration.Domain.Models;
using TwitchLib.Client.Models;
using TwitchLib.Client;
using ParanTwitchIntegration.Application.Factories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Configuration.AddJsonFile(builder.Environment.IsDevelopment() ? "appsettings.Development.json" : "appsettings.json");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<TwitchSettings>(builder.Configuration.GetSection("TwitchSettings"));
builder.Services.Configure<TwitchToken>(builder.Configuration.GetSection("TwitchToken"));
builder.Services.Configure<List<TwitchAccount>>(builder.Configuration.GetSection("TwitchAccounts"));
builder.Services.Configure<MinecraftSettings>(builder.Configuration.GetSection("MinecraftSettings"));
builder.Services.Configure<OpenAISettings>(builder.Configuration.GetSection("OpenAISettings"));

builder.Services.AddSingleton<EventBus>();
builder.Services.AddSingleton<ITwitchTokenService, TwitchTokenService>();
builder.Services.AddSingleton<ITwitchMultiAccountService, TwitchMultiAccountService>();
builder.Services.AddSingleton<ITwitchChatService, TwitchChatService>();
builder.Services.AddSingleton<TwitchClientFactory>();
builder.Services.AddSingleton<OBSSockets>();
//builder.Services.AddSingleton<IMinecraftIntegrationService, MinecraftIntegrationService>();
builder.Services.AddSingleton<IBanjoTTSService, BanjoTTSService>();
builder.Services.AddHttpClient<IOpenAIService, OpenAIService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

/*var twitchService = app.Services.GetRequiredService<ITwitchService>();
twitchService.Start(); */

var twitchMultiAccountService = app.Services.GetRequiredService<ITwitchMultiAccountService>();
twitchMultiAccountService.StartAll();

app.Run();