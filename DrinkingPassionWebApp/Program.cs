using Blazored.LocalStorage;
using DrinkingPassionWebApp;
using DrinkingPassionWebApp.Features.Auth;
using DrinkingPassionWebApp.Services;
using DrinkingPassionWebApp.Services.Interfaces;
using DrinkingPassionWebApp.Shared;
using Fluxor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var settings = new AppSettings();
builder.Configuration.Bind(settings);
builder.Services.AddSingleton(settings);

string? baseApiAddress = string.Empty;
if (builder.HostEnvironment.IsDevelopment())
{
    baseApiAddress = builder.Configuration["BaseApiAddress"];
}
else
{
    baseApiAddress = Environment.GetEnvironmentVariable("BaseApiAddress");
}

if (string.IsNullOrEmpty(baseApiAddress))
{
    throw new ArgumentNullException("BaseApiAddress is not set");
}

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseApiAddress) });
builder.Services.AddFluxor(o =>
{
    o.ScanAssemblies(typeof(Program).Assembly);
#if DEBUG
    o.UseReduxDevTools();
#endif
});
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ICocktailsService, CocktailsService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<DrinkingPassionAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(
    sp => sp.GetRequiredService<DrinkingPassionAuthenticationStateProvider>());
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();