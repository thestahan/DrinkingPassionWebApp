namespace DrinkingPassionWebApp.Features.Cocktails.Dtos;

public class CocktailDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Picture { get; set; }
    public required string BaseIngredient { get; set; }
    public int IngredientsCount { get; set; }
    public bool IsPrivate { get; set; }
}
