var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddHealthChecks();  // ADD THIS

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

// ADD HEALTH CHECK MIDDLEWARE
app.MapHealthChecks("/health");  // For Replit monitoring
app.MapRazorPages();

// ADD EXPLICIT ROOT ENDPOINT
app.MapGet("/", () => {
    return Results.Redirect("/Index");  // Redirect to your Razor page
});

app.Run("http://0.0.0.0:5000");