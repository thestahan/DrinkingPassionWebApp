using DrinkingPassionWebApp.Features.Login.Dtos;
using DrinkingPassionWebApp.Shared;
using OneOf;

namespace DrinkingPassionWebApp.Services.Interfaces;

public interface IUsersService
{
    public Task<OneOf<LoginReturnDto, ApiErrorResponse>> LoginUser(LoginDto loginDto);
}