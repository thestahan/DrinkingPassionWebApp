using DrinkingPassionWebApp.Features.Auth;
using DrinkingPassionWebApp.Features.Login.Dtos;
using DrinkingPassionWebApp.Shared;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace DrinkingPassionWebApp.Features.Login.Store;

public record LoginState
{
    public bool IsSending { get; init; }
    public bool IsError { get; init; }
    public string? ErrorMessage { get; init; }
    public LoginDto LoginDto { get; set; } = default!;
}

public class LoginFeature : Feature<LoginState>
{
    public override string GetName() => nameof(LoginFeature);

    protected override LoginState GetInitialState() =>
        new()
        {
            IsSending = false,
            IsError = false,
            ErrorMessage = null,
            LoginDto = new LoginDto(),
        };
}

public static class LoginReducers
{
    [ReducerMethod]
    public static LoginState OnLoginSuccess(LoginState state, LoginSuccessAction _) =>
        state with
        {
            IsSending = false,
            IsError = false
        };

    [ReducerMethod]
    public static LoginState OnLoginFailure(LoginState state, LoginFailureAction action) =>
        state with
        {
            IsSending = false,
            IsError = true,
            ErrorMessage = action.ApiErrorResponse.Message
        };

    [ReducerMethod]
    public static LoginState OnLoginSubmit(LoginState state, LoginSubmitAction _) =>
        state with
        {
            IsSending = true
        };
}

public class LoginEffects
{
    private readonly DrinkingPassionAuthenticationStateProvider _authenticationStateProvider;
    private readonly NavigationManager _navigationManager;

    public LoginEffects(
        DrinkingPassionAuthenticationStateProvider authenticationStateProvider,
        NavigationManager navigationManager)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _navigationManager = navigationManager;
    }

    [EffectMethod]
    public async Task HandleLoginSubmitAction(LoginSubmitAction action, IDispatcher dispatcher)
    {
        var result = await _authenticationStateProvider.Login(action.LoginDto);

        result.Switch(
            user =>
            {
                dispatcher.Dispatch(new LoginSuccessAction(user));
                dispatcher.Dispatch(new UserLoggedInAction(
                    Email: user.Email,
                    DisplayName: user.DisplayName,
                    Roles: user.Roles));

                _navigationManager.NavigateTo("/");
            },
            error =>
            {
                dispatcher.Dispatch(new LoginFailureAction(error));
            });
    }
}

public record LoginSuccessAction(User User);
public record LoginFailureAction(ApiErrorResponse ApiErrorResponse);
public record LoginSubmitAction(LoginDto LoginDto);