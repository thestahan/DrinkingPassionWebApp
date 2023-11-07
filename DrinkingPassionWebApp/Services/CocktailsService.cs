using DrinkingPassionWebApp.Features.Cocktails.Dtos;
using DrinkingPassionWebApp.Shared;
using System.Net.Http.Json;

namespace DrinkingPassionWebApp.Services;

public class CocktailsService : ICocktailsService
{
    private readonly HttpClient _httpClient;

    public CocktailsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CocktailDetails?> GetCocktailDetailsAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/cocktails/{id}");

        return await _httpClient.GetFromJsonAsync<CocktailDetails>(request.RequestUri!.ToString());
    }

    public async Task<Pagination<CocktailDto>?> GetPublicCocktailsAsync(int pageIndex)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"api/cocktails/public?pageIndex={pageIndex}");

        return await _httpClient.GetFromJsonAsync<Pagination<CocktailDto>>(request.RequestUri!.ToString());
    }
}