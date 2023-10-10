using chdScoring.BusinessLogic.Extensions;
using chdScoring.Main.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using WindowsFormsLifetime;
using NLog.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using chdScoring.Main.UI.Services;
using chdScoring.DataAccess.Repositories;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.Repositories.Base;
using chdScoring.DataAccess.Contracts.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.Main.UI.Extensions;
using NLog.LayoutRenderers;

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

builder.Services.AddchdScoringDataAccess(builder.Configuration);
builder.Services.AddHostedService<chdScoringService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.AddApiLogger();
app.MapChdScoring("chdScoring");
app.Run();


