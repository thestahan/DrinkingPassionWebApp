using DrinkingPassionWebApp.Features.Auth;
using DrinkingPassionWebApp.Features.Login.Dtos;
using DrinkingPassionWebApp.Shared;
using OneOf;

namespace DrinkingPassionWebApp.Services.Interfaces;

public interface IUsersService
{
    Task<OneOf<User, ApiErrorResponse>> LoginUser(LoginDto loginDto);

    Task<User?> GetUserFromLocalStorage();

    Task Logout();
}