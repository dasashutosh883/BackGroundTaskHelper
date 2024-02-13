using BackgroundTaskHelper.Container;
using BackgroundTaskHelper.Controllers;
using BackgroundTaskHelper.Filters;
using BackgroundTaskHelper.Models;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(
    options =>
    {
        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    });
// Add services to the container.
builder.Services.AddHangfire(x => x.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("connstring")!));
builder.Services.AddHangfireServer(); 
builder.Services.AddCustomContainer(builder.Configuration);

builder.Services.AddSingleton<GlobalListManager<LogModel>>();
builder.Services.AddSingleton<PeriodicHostedService>();

// Add as hosted service using the instance registered as singleton before
builder.Services.AddHostedService(
    provider => provider.GetRequiredService<PeriodicHostedService>());

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
var options = new DashboardOptions
{
    Authorization = new[] { new CustomAuthorizationFilter() }
};
app.UseHangfireDashboard("/hangfire");
app.MapHangfireDashboard();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
