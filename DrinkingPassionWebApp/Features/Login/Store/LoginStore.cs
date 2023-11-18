using DrinkingPassionWebApp.Features.Login.Dtos;
using DrinkingPassionWebApp.Services.Interfaces;
using DrinkingPassionWebApp.Shared;
using Fluxor;

namespace DrinkingPassionWebApp.Features.Login.Store;

public record LoginState
{
    public bool IsSaving { get; init; }
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
            IsSaving = false,
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
            IsSaving = false,
            IsError = false
        };

    [ReducerMethod]
    public static LoginState OnLoginFailure(LoginState state, LoginFailureAction action) =>
        state with
        {
            IsSaving = false,
            IsError = true,
            ErrorMessage = action.ApiErrorResponse.Message
        };

    [ReducerMethod]
    public static LoginState OnLoginSubmit(LoginState state, LoginSubmitAction action) =>
        state with
        {
            IsSaving = true
        };
}

public class LoginEffects
{
    private readonly IUsersService _usersService;

    public LoginEffects(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [EffectMethod]
    public async Task HandleLoginSubmitAction(LoginSubmitAction action, IDispatcher dispatcher)
    {
        var result = await _usersService.LoginUser(action.LoginDto);

        result.Switch(
            dto => dispatcher.Dispatch(new LoginSuccessAction(dto)),
            error => dispatcher.Dispatch(new LoginFailureAction(error)));
    }
}

public record LoginSuccessAction(LoginReturnDto LoginReturnDto);
public record LoginFailureAction(ApiErrorResponse ApiErrorResponse);
public record LoginSubmitAction(LoginDto LoginDto);