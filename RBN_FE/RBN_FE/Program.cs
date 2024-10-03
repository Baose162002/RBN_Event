using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RBN_FE;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
// Add session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // Uncomment the following line to enable HTTPS redirection in production
    // app.UseHsts();
}

app.UseCors("AllowAllOrigins");



// Middleware pipeline configuration
app.UseStaticFiles();
app.UseRouting();

// Add session middleware before using session
app.UseSession();  // Ensure this is before accessing session in middleware or Razor pages

app.UseMiddleware<AdminRoleMiddleware>();

app.UseAuthorization();

app.MapRazorPages();

app.Run();