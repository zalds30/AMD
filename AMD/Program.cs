var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Remove HSTS and HTTPS redirection for Render
// app.UseHttpsRedirection(); // COMMENT OR REMOVE
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

// Render PORT setup
app.Run($"http://0.0.0.0:{Environment.GetEnvironmentVariable("PORT") ?? "5000"}");