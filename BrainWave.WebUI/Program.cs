using BrainWave.Infrastructure;
using BrainWave.Infrastructure.Data;
using BrainWave.WebUI.Middlewares;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Web.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(); 
builder.Services.AddStorage(builder.Configuration);

/*builder.Services.AddTransient<ExceptionHandlingMiddleware>();*/

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
