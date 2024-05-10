using CheckDrive.Web.Constants;
using CheckDrive.Web.Exceptions;
using CheckDrive.Web.Filters;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
    options.Filters.Add(new ApiExceptionFilter()));

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Configurations.SynfusionLicenseKey);

var app = builder.Build();

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
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
