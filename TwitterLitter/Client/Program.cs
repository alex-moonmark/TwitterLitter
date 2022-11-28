using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TwitterLitter.Client;
using TwitterLitter.Client.Classes;
using TwitterLitter.Client.Interfaces;
using TwitterLitter.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ITwitterService, TwitterService>();
builder.Services.AddScoped<ITweetFormatHelper, TweetFormatHelper>();

await builder.Build().RunAsync();
