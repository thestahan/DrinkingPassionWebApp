using DrinkingPassionWebApp.Features.Cocktails.Dtos;
using Fluxor;
using System.Net.Http.Json;

namespace DrinkingPassionWebApp.Features.Cocktails.Store;

public record CocktailDetailsState
{
    public bool IsInitialized { get; set; }
    public bool IsLoading { get; init; }
    public bool IsError { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;
    public CocktailDetails? CocktailDetails { get; init; }
}

public class CocktailDetailsFeature : Feature<CocktailDetailsState>
{
    public override string GetName() => nameof(CocktailDetailsFeature);

    protected override CocktailDetailsState GetInitialState() =>
        new()
        {
            IsInitialized = false,
            IsLoading = false,
            IsError = false,
            CocktailDetails = null
        };
}

public static class CocktailDetailsReducers
{
    [ReducerMethod]
    public static CocktailDetailsState OnFetchCocktailDetails(CocktailDetailsState state, FetchCocktailDetailsAction _) =>
        state with
        {
            IsLoading = true
        };

    [ReducerMethod]
    public static CocktailDetailsState OnFetchCocktailDetailsSuccess(CocktailDetailsState state, FetchCocktailDetailsSuccessAction action) =>
        state with
        {
            IsInitialized = true,
            IsLoading = false,
            CocktailDetails = action.CocktailDetails
        };

    [ReducerMethod]
    public static CocktailDetailsState OnFetchCocktailDetailsFailure(CocktailDetailsState state, FetchCocktailDetailsFailureAction action) =>
        state with
        {
            IsInitialized = true,
            IsLoading = false,
            IsError = true,
            ErrorMessage = action.ErrorMessage
        };
}

public class CocktailDetailsEffects
{
    private readonly HttpClient _httpClient;

    public CocktailDetailsEffects(HttpClient httpClient) =>
        _httpClient = httpClient;

    [EffectMethod]
    public async Task HandleFetchCocktailDetailsAction(FetchCocktailDetailsAction action, IDispatcher dispatcher)
    {
        try
        {
            var cocktailDetails = await _httpClient.GetFromJsonAsync<CocktailDetails>("https://localhost:5001/api/cocktails/" + action.CocktailId);

            dispatcher.Dispatch(new FetchCocktailDetailsSuccessAction(cocktailDetails!));
        }
        catch (Exception ex)
        {
            dispatcher.Dispatch(new FetchCocktailDetailsFailureAction(ex.Message));
        }
    }
}

public record FetchCocktailDetailsAction(int CocktailId);
public record FetchCocktailDetailsSuccessAction(CocktailDetails CocktailDetails);
public record FetchCocktailDetailsFailureAction(string ErrorMessage);