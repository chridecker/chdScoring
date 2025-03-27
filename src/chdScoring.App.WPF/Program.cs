using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using chdScoring.App.WPF.Extensions;
using chdScoring.App.WPF;
using chd.Base.UI.WPF.Extensions;

Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);

Thread.CurrentThread.SetApartmentState(ApartmentState.STA);

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddChdScoringApp(builder.Configuration);
builder.Services.AddWPF<App>(builder.Configuration);


builder.Services.AddWpfBlazorWebView();
builder.Services.AddBlazorWebViewDeveloperTools();

var app = builder.Build();

app.Run();