using chd.Base.Hosting.Extensions;
using chd.Base.Hosting.Forms;
using chdScoring.BusinessLogic.Extensions;
using chdScoring.BusinessLogic.Services;
using chdScoring.Contracts.Enums;
using chdScoring.Contracts.Interfaces;
using chdScoring.Contracts.Settings;
using chdScoring.Main.WebServer;
using chdScoring.Main.WebServer.Modules;
using NLog.Web;
using System.Configuration;


//Microsoft.Playwright.Program.Main(["install"]);
//return;
Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
Thread.CurrentThread.SetApartmentState(ApartmentState.STA);



var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

var config = new ConfigurationBuilder().AddJsonFile(path).Build();

var builder = DiFormHost.CreateApplicationBuilder<MainForm>(args);

builder.Configuration.AddJsonFile(path, false, true);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));

builder.Logging.AddNLogWeb();

builder.AddModuleManager().AddModule<ServerModule>().AddModule<WebModule>();

builder.Services.Configure<DBSettings>(nameof(EDBConnection.MySql), builder.Configuration.GetSection($"{nameof(DBSettings)}:{nameof(EDBConnection.MySql)}"));
builder.Services.Configure<DBSettings>(nameof(EDBConnection.SQLite), builder.Configuration.GetSection($"{nameof(DBSettings)}:{nameof(EDBConnection.SQLite)}"));

builder.Services.AddSingleton<IPrintCache, PrintCache>();
builder.Services.AddSingleton<IDatabaseConfiguration, DatabaseConfiguration>();
builder.Services.AddSingleton<IDeviceCacheService, DeviceCacheService>();

var app = builder.Build();
app.Run();


