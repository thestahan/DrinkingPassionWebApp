using DrinkingPassionWebApp.Features.Cocktails.Dtos;
using DrinkingPassionWebApp.Shared;

namespace DrinkingPassionWebApp.Services.Interfaces;

public interface ICocktailsService
{
    public Task<Pagination<CocktailDto>?> GetPublicCocktailsAsync(int page);

    public Task<CocktailDetails?> GetCocktailDetailsAsync(int id);
}