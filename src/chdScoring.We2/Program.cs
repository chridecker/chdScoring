using chdScoring.Web.Components;
using chdScoring.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebUI(builder.Configuration);

// Add services to the container.
builder.Services.AddRazorComponents(options => { options.DetailedErrors = builder.Environment.IsDevelopment(); })
.AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
