using Blazored.LocalStorage;
using DrinkingPassionWebApp.Features.Auth;
using DrinkingPassionWebApp.Features.Login.Dtos;
using DrinkingPassionWebApp.Services.Interfaces;
using DrinkingPassionWebApp.Shared;
using Newtonsoft.Json;
using OneOf;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DrinkingPassionWebApp.Services;

public class UsersService : IUsersService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public UsersService(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<OneOf<User, ApiErrorResponse>> LoginUser(LoginDto loginDto)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"api/accounts/login")
        {
            Content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json")
        };

        HttpResponseMessage response = await _httpClient.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<ApiErrorResponse>(responseContent)!;
        }

        var returnDto = JsonConvert.DeserializeObject<LoginReturnDto>(responseContent)!;
        var claimsPrincipal = CreateClaimsPrincipalFromToken(returnDto.Token);
        var user = User.FromClaimsPrincipal(claimsPrincipal);

        await _localStorage.SetItemAsync("Token", returnDto.Token);
        await _localStorage.SetItemAsync("TokenExpiration", returnDto.TokenExpiration);

        return user;
    }

    public async Task<User?> GetUserFromLocalStorage()
    {
        var token = await _localStorage.GetItemAsync<string?>("Token");

        if (token is null)
        {
            return null;
        }

        var claimsPrincipal = CreateClaimsPrincipalFromToken(token);

        return User.FromClaimsPrincipal(claimsPrincipal);
    }

    public async Task Logout()
    {
        await _localStorage.ClearAsync();
    }

    private static ClaimsPrincipal CreateClaimsPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var identity = new ClaimsIdentity();

        if (tokenHandler.CanReadToken(token))
        {
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
            identity = new(jwtSecurityToken.Claims, AuthConstants.AuthenticationType);
        }

        return new(identity);
    }
}