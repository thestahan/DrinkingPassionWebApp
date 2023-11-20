using Fluxor;

namespace DrinkingPassionWebApp.Features.Auth;

public record UserState
{
    public string? Email { get; init; }
    public string? DisplayName { get; init; }
    public ICollection<string> Roles { get; init; } = new List<string>();
    public bool IsUserSet { get; set; }
}

public class UserFeature : Feature<UserState>
{
    public override string GetName() => nameof(UserFeature);

    protected override UserState GetInitialState() =>
        new()
        {
            Email = null,
            DisplayName = null,
            Roles = new List<string>(),
            IsUserSet = false
        };
}

public static class UserReducers
{
    [ReducerMethod]
    public static UserState OnUserLoggedIn(UserState state, UserLoggedInAction action) =>
        state with
        {
            Email = action.Email,
            DisplayName = action.DisplayName,
            Roles = action.Roles,
            IsUserSet = true
        };

    [ReducerMethod]
    public static UserState OnUserLogout(UserState state, UserLogoutAction _) =>
        state with
        {
            Email = null,
            DisplayName = null,
            Roles = new List<string>(),
            IsUserSet = false
        };
}

public class UserEffects
{
    private readonly DrinkingPassionAuthenticationStateProvider _authenticationStateProvider;

    public UserEffects(DrinkingPassionAuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
    }

    [EffectMethod(typeof(UserLogoutAction))]
    public async Task HandleUserLogoutAction(IDispatcher _)
    {
        await _authenticationStateProvider.Logout();
    }
}

public record UserLoggedInAction(string Email, string? DisplayName, ICollection<string> Roles);
public record UserLogoutAction();