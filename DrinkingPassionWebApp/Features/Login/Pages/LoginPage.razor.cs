using DrinkingPassionWebApp.Features.Global;
using DrinkingPassionWebApp.Features.Login.Dtos;
using DrinkingPassionWebApp.Features.Login.Store;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DrinkingPassionWebApp.Features.Login.Pages;

public partial class LoginPage : FluxorComponent
{
    [Inject]
    public IState<LoginState> LoginState { get; set; } = default!;

    [Inject]
    public IState<UserState> UserState { get; set; } = default!;

    [Inject]
    public IDispatcher Dispatcher { get; set; } = default!;

    public LoginDto Model => LoginState.Value.LoginDto;

    public MudForm Form = default!;

    private void SubmitForm()
    {
        Dispatcher.Dispatch(new LoginSubmitAction(Model));
    }
}