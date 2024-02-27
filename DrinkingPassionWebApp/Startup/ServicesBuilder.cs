using Blazored.LocalStorage;
using DrinkingPassionWebApp.Features.Auth;
using DrinkingPassionWebApp.Services;
using DrinkingPassionWebApp.Services.Interfaces;
using DrinkingPassionWebApp.Shared;
using Fluxor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

namespace DrinkingPassionWebApp.Startup;

public static class ServicesBuilder
{
    public static void ConfigureServices(WebAssemblyHostBuilder builder)
    {
        var settings = BindConfiguration<AppSettings>(builder);
        builder.Services.AddSingleton(settings);

        string baseApiAddress = GetBaseApiAddress(builder)
            ?? throw new ArgumentException("BaseApiAddress is not set");

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
    }

    private static TSettings BindConfiguration<TSettings>(WebAssemblyHostBuilder builder) where TSettings : new()
    {
        var settings = new TSettings();
        builder.Configuration.Bind(settings);
        return settings;
    }

    private static string? GetBaseApiAddress(WebAssemblyHostBuilder builder)
    {
        return builder.HostEnvironment.IsDevelopment()
            ? builder.Configuration["BaseApiAddress"]
            : Environment.GetEnvironmentVariable("BaseApiAddress");
    }
}
