namespace DrinkingPassionWebApp.Shared;

public record ApiErrorResponse
{
    public int StatusCode { get; init; }
    public string? Message { get; init; }
}