using DrinkingPassionWebApp;
using DrinkingPassionWebApp.Services;
using DrinkingPassionWebApp.Services.Interfaces;
using Fluxor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001/") });
builder.Services.AddFluxor(o =>
{
    o.ScanAssemblies(typeof(Program).Assembly);
#if DEBUG
    o.UseReduxDevTools();
#endif
});
builder.Services.AddMudServices();
builder.Services.AddScoped<ICocktailsService, CocktailsService>();
builder.Services.AddScoped<IUsersService, UsersService>();

await builder.Build().RunAsync();