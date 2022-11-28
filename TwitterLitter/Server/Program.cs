using Microsoft.AspNetCore.ResponseCompression;
using TwitterLitter.Server.Classes;
using TwitterLitter.Server.Interfaces;
using TwitterLitter.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ICancellationService, CancellationService>();
builder.Services.AddSingleton<ITwitterSampleStreamClient, TwitterClientHandler>();
builder.Services.AddSingleton<ITwitterStatisticsService, TwitterStatisticsService>();
builder.Services.AddSingleton<ITweetProcessingService, TweetProcessingService>();
builder.Services.AddHostedService<TwitterManager>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");


app.Run();
