var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// FOR REPLIT: Keep it simple
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

// REPLIT FIX: Remove PORT env variable, use direct port
app.Run("http://0.0.0.0:5000");