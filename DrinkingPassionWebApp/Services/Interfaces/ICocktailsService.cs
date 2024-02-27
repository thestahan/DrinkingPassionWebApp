using DrinkingPassionWebApp.Features.Cocktails.Dtos;
using DrinkingPassionWebApp.Shared;

namespace DrinkingPassionWebApp.Services.Interfaces;

public interface ICocktailsService
{
    public Task<Pagination<CocktailDto>?> GetPublicCocktails(int pageIndex);

    public Task<CocktailDetails?> GetCocktailDetails(int id);
}
