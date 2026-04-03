var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapHealthChecks("/health");
app.MapHealthChecks("/healthz");

// ITO ANG PAGBABAGO: Gawing Razor Pages ang root
app.MapRazorPages(); // Ito na mismo ang bahala sa root

app.Run();