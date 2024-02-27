using DrinkingPassionWebApp.Features.Login.Dtos;
using DrinkingPassionWebApp.Services.Interfaces;
using DrinkingPassionWebApp.Shared;
using Fluxor;
using Microsoft.AspNetCore.Components.Authorization;
using OneOf;
using System.Security.Claims;

namespace DrinkingPassionWebApp.Features.Auth;

public class DrinkingPassionAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IUsersService _usersService;
    private readonly IDispatcher _dispatcher;

    public DrinkingPassionAuthenticationStateProvider(IUsersService usersService, IDispatcher dispatcher)
    {
        _usersService = usersService;
        _dispatcher = dispatcher;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var principal = new ClaimsPrincipal();
        var user = await _usersService.GetUserFromLocalStorage();

        if (user is not null)
        {
            _dispatcher.Dispatch(new UserLoggedInAction(
                    Email: user.Email,
                    DisplayName: user.DisplayName,
                    Roles: user.Roles));

            principal = user.ToClaimsPrincipal();
        }

        return new(principal);
    }

    public async Task<OneOf<User, ApiErrorResponse>> Login(LoginDto loginDto)
    {
        OneOf<User, ApiErrorResponse> loginResult = await _usersService.LoginUser(loginDto);

        if (loginResult.TryPickT0(out User user, out ApiErrorResponse apiError))
        {
            var authState = new AuthenticationState(user.ToClaimsPrincipal());
            NotifyAuthenticationStateChanged(Task.FromResult(authState));

            return user;
        }

        return apiError;
    }

    public async Task Logout()
    {
        await _usersService.Logout();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new())));
    }
}
