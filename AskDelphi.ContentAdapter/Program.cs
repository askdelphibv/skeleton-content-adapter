using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var startup = new AskDelphi.ContentAdapter.Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);
var app = builder.Build();

System.AppDomain.CurrentDomain.SetData("ContentRootPath", app.Environment.ContentRootPath);
System.AppDomain.CurrentDomain.SetData("WebRootPath", app.Environment.WebRootPath);

startup.Configure(app, app.Environment);

app.Run();