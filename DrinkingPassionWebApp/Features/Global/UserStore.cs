using DrinkingPassionWebApp.Features.Login.Store;
using Fluxor;

namespace DrinkingPassionWebApp.Features.Global;

public record UserState
{
    public string? Email { get; init; }
    public string? DisplayName { get; init; }
    public string? Token { get; init; }
    public string? TokenExpiration { get; init; }
    public bool IsLoggedIn { get; init; }
    public ICollection<string>? Roles { get; init; } =
        new List<string>();
}

public class UserFeature : Feature<UserState>
{
    public override string GetName() => nameof(UserFeature);

    protected override UserState GetInitialState() =>
        new()
        {
            Email = null,
            DisplayName = null,
            Token = null,
            TokenExpiration = null,
            IsLoggedIn = false,
            Roles = new List<string>()
        };
}

public static class UserReducers
{
    [ReducerMethod]
    public static UserState OnUserLogin(UserState state, LoginSuccessAction action) =>
        state with
        {
            DisplayName = action.LoginReturnDto.DisplayName,
            Email = action.LoginReturnDto.Email,
            Token = action.LoginReturnDto.Token,
            TokenExpiration = action.LoginReturnDto.TokenExpiration,
            Roles = action.LoginReturnDto.Roles,
            IsLoggedIn = true
        };

    [ReducerMethod]
    public static UserState UserUserLogout(UserState state, UserLogoutAction _) =>
        state with
        {
            Email = null,
            DisplayName = null,
            Token = null,
            TokenExpiration = null,
            IsLoggedIn = false,
            Roles = new List<string>()
        };
}

public record UserLogoutAction();