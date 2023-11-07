namespace DrinkingPassionWebApp.Features.Cocktails.Dtos;

public class CocktailDetails
{
    public int Id { get; init; }
    public string Name { get; init; } = default!;
    public string? Picture { get; init; }
    public string? Description { get; init; }
    public string? PreparationInstruction { get; init; }
    public string BaseIngredient { get; init; } = default!;
    public int IngredientsCount { get; init; }

    public ICollection<Ingredient> Ingredients { get; init; } =
        new List<Ingredient>();

    public class Ingredient
    {
        public int Id { get; init; }
        public int ProductId { get; init; }
        public double Amount { get; init; }
        public string Name { get; init; } = default!;
        public string Unit { get; init; } = default!;
    }
}