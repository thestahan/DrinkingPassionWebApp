using DrinkingPassionWebApp.Features.Cocktails.Dtos;
using DrinkingPassionWebApp.Shared;
using Fluxor;
using System.Net.Http.Json;

namespace DrinkingPassionWebApp.Features.Cocktails.Store;

public record PublicCocktailsState
{
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
            IsLoading = false,
            PaginatedCocktails = action.PaginatedCocktails
        };

    [ReducerMethod]
    public static PublicCocktailsState OnFetchPublicCocktailsFailure(PublicCocktailsState state, FetchPublicCocktailsFailureAction action) =>
        state with
        {
            IsLoading = false,
            IsError = true,
            ErrorMessage = action.ErrorMessage
        };
}

public class PublicCocktailsEffects
{
    private readonly HttpClient _httpClient;

    public PublicCocktailsEffects(HttpClient httpClient) =>
        _httpClient = httpClient;

    [EffectMethod]
    public async Task HandleFetchPublicCocktailsAction(FetchPublicCocktailsAction _, IDispatcher dispatcher)
    {
        try
        {
            var paginatedCocktails = await _httpClient.GetFromJsonAsync<Pagination<CocktailDto>>("https://localhost:5001/api/cocktails/public");

            dispatcher.Dispatch(new FetchPublicCocktailsSuccessAction(paginatedCocktails));
        }
        catch (Exception e)
        {
            dispatcher.Dispatch(new FetchPublicCocktailsFailureAction(e.Message));
        }
    }
}

public record FetchPublicCocktailsAction();
public record FetchPublicCocktailsSuccessAction(Pagination<CocktailDto> PaginatedCocktails);
public record FetchPublicCocktailsFailureAction(string ErrorMessage);