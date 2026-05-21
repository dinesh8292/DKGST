using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DKGST.Blazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add("#app", typeof(App));
builder.RootComponents.Add("head::after", typeof(HeadOutlet));

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001") });
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<MenuService>();

// Local storage for token persistence
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();

await builder.Build().RunAsync();
