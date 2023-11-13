using DrinkingPassionWebApp.Features.Cocktails.Dtos;
using DrinkingPassionWebApp.Services.Interfaces;
using DrinkingPassionWebApp.Shared;
using Fluxor;

namespace DrinkingPassionWebApp.Features.Cocktails.Store;

public record PublicCocktailsState
{
    public bool IsInitialized { get; init; }
    public bool IsLoading { get; init; }
    public bool IsError { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;
    public Pagination<CocktailDto>? PaginatedCocktails { get; init; }
}

public class PublicCocktailsFeature : Feature<PublicCocktailsState>
{
    public override string GetName() => nameof(PublicCocktailsFeature);

    protected override PublicCocktailsState GetInitialState() =>
        new()
        {
            IsInitialized = false,
            IsLoading = false,
            IsError = false,
            PaginatedCocktails = null
        };
}

public static class PublicCocktailsReducers
{
    [ReducerMethod]
    public static PublicCocktailsState OnFetchPublicCocktails(PublicCocktailsState state, FetchPublicCocktailsAction _) =>
        state with
        {
            IsLoading = true
        };

    [ReducerMethod]
    public static PublicCocktailsState OnFetchPublicCocktailsSuccess(PublicCocktailsState state, FetchPublicCocktailsSuccessAction action) =>
        state with
        {
            IsInitialized = true,
            IsLoading = false,
            PaginatedCocktails = action.PaginatedCocktails
        };

    [ReducerMethod]
    public static PublicCocktailsState OnFetchPublicCocktailsFailure(PublicCocktailsState state, FetchPublicCocktailsFailureAction action) =>
        state with
        {
            IsInitialized = true,
            IsLoading = false,
            IsError = true,
            ErrorMessage = action.ErrorMessage
        };
}

public class PublicCocktailsEffects
{
    private readonly ICocktailsService _cocktailsService;

    public PublicCocktailsEffects(ICocktailsService cocktailsService) =>
        _cocktailsService = cocktailsService;

    [EffectMethod]
    public async Task HandleFetchPublicCocktailsAction(FetchPublicCocktailsAction action, IDispatcher dispatcher)
    {
        try
        {
            var paginatedCocktails = await _cocktailsService.GetPublicCocktailsAsync(action.PageIndex);

            dispatcher.Dispatch(new FetchPublicCocktailsSuccessAction(paginatedCocktails!));
        }
        catch (Exception e)
        {
            dispatcher.Dispatch(new FetchPublicCocktailsFailureAction(e.Message));
        }
    }
}

public record FetchPublicCocktailsAction(int PageIndex);
public record FetchPublicCocktailsSuccessAction(Pagination<CocktailDto> PaginatedCocktails);
public record FetchPublicCocktailsFailureAction(string ErrorMessage);