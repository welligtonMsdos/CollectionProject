namespace Collection10Api.src.Domain.Entities;

public class Vinyl
{
    public Guid Guid { get; set; }
    public required string Artist { get; set; }
    public required string Album { get; set; }
    public int Year { get; set; }
    public string Photo { get; set; }
    public decimal Price { get; set; }
    public bool Active { get; set; }
}
