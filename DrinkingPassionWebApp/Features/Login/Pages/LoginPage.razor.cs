using DrinkingPassionWebApp.Features.Auth;
using DrinkingPassionWebApp.Features.Login.Dtos;
using DrinkingPassionWebApp.Features.Login.Store;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace DrinkingPassionWebApp.Features.Login.Pages;

public partial class LoginPage : FluxorComponent
{
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = default!;

    [Inject]
    public IState<LoginState> LoginState { get; set; } = default!;

    [Inject]
    public IState<UserState> UserState { get; set; } = default!;

    [Inject]
    public IDispatcher Dispatcher { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    public LoginDto Model => LoginState.Value.LoginDto;

    public MudForm Form = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        var user = (await AuthState).User;
        var userIdentity = user.Identity;
        if (userIdentity is not null && userIdentity.IsAuthenticated)
        {
            NavigationManager.NavigateTo("/");
        }
    }

    private void SubmitForm()
    {
        Dispatcher.Dispatch(new LoginSubmitAction(Model));
    }
}