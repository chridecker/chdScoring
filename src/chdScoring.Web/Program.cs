using chd.Api.Base.Client.Extensions;
using chdScoring.Main.Client.Extensions;
using chdScoring.Web.Services;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin()
              .SetIsOriginAllowed(origin => true)
              .AllowCredentials(); // wichtig für SignalR
    });
});


builder.Services.AddChdScoringClient(_ => builder.Configuration.GetApiKey("chdScoringApi"));
builder.Services.AddSingleton<ImageCache>();
builder.Services.AddSingleton<IconStyleService>(_ => new IconStyleService() { IconStyle = chd.UI.Base.Contracts.Enum.EIconStyle.Regular });
// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
