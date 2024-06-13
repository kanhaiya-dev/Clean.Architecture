namespace Clean.Architecture.Core.Entities.Buisness;

public class Product
{
    public int Id { get; init; }

    public string? Title { get; init; }

    public string? Description { get; init; }

    public decimal? Price { get; init; }
}
