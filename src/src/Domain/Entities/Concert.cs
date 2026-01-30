namespace Collection10Api.src.Domain.Entities;

public class Concert
{
    public Guid Guid { get; set; }
    public string Artist { get; set; }
    public string Venue { get; set; }
    public DateOnly ShowDate { get; set; }
    public string Photo { get; set; }
    public bool Active { get; set; }
}
