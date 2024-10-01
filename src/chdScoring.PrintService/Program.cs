using chdScoring.PrintService;
using chdScoring.PrintService.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;
using WindowsFormsLifetime;

var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

var config = new ConfigurationBuilder().AddJsonFile(path).Build();

var builder = WebApplication.CreateBuilder(args);



builder.Configuration.AddJsonFile(path);
builder.Logging.ClearProviders();
builder.Logging.AddNLogWeb();

builder.WebHost.UseUrls(config.GetValue<string>("BaseAddress"));

builder.Services.AddPrint(builder.Configuration);

builder.Host.UseWindowsFormsLifetime<PrintForm>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPrinterService("print");

app.Run();


