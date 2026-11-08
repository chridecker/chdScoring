using chdScoring.Main.WebServer;
using NLog.Web;
using chd.Base.Hosting.Forms;
using chd.Base.Hosting.Extensions;
using chdScoring.Main.WebServer.Modules;
using chdScoring.BusinessLogic.Extensions;


//Microsoft.Playwright.Program.Main(["install"]);
//return;
Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
Thread.CurrentThread.SetApartmentState(ApartmentState.STA);



var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

var config = new ConfigurationBuilder().AddJsonFile(path).Build();

var builder = DiFormHost.CreateApplicationBuilder<MainForm>(args);

builder.Configuration.AddJsonFile(path, false, true);
builder.Logging.AddNLogWeb();

builder.AddModuleManager().AddModule<ServerModule>();

builder.Services.AddchdScoringDataAccess(builder.Configuration);



var app = builder.Build();
app.Run();


