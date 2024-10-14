using chdScoring.BusinessLogic.Extensions;
using chdScoring.Main.UI;
using Microsoft.Extensions.Configuration;
using WindowsFormsLifetime;
using NLog.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using chdScoring.Main.UI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using chdScoring.Main.UI.Extensions;
using chd.Api.Base.Extensions;
using chdScoring.BusinessLogic.Hubs;

Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
Thread.CurrentThread.SetApartmentState(ApartmentState.STA);

var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

var config = new ConfigurationBuilder().AddJsonFile(path).Build();

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile(path);
builder.Logging.ClearProviders();
builder.Logging.AddNLogWeb();

builder.WebHost.UseUrls(config.GetValue<string>("BaseAddress"));

builder.Host.UseWindowsFormsLifetime<MainForm>();

builder.Services.AddBaseApi("chdScoringAPI");

builder.Services.AddchdScoringDataAccess(builder.Configuration);
builder.Services.AddHostedService<chdScoringService>();


builder.Services.AddSignalR();

var app = builder.Build();

app.MapChdScoring();

app.AddApiLogger();
app.MapHub<FlightHub>("/chdScoring/flight-hub");


app.UseBaseApi();

app.UseRouting();

app.Run();


