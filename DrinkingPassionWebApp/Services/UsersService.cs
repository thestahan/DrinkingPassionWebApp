using DrinkingPassionWebApp.Features.Login.Dtos;
using DrinkingPassionWebApp.Services.Interfaces;
using DrinkingPassionWebApp.Shared;
using Newtonsoft.Json;
using OneOf;
using System.Text;

namespace DrinkingPassionWebApp.Services;

public class UsersService : IUsersService
{
    private readonly HttpClient _httpClient;

    public UsersService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<OneOf<LoginReturnDto, ApiErrorResponse>> LoginUser(LoginDto loginDto)
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

        return JsonConvert.DeserializeObject<LoginReturnDto>(responseContent)!;
    }
}