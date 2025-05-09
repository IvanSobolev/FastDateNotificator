namespace FastDateNotificatorAPI.Models.DTOs;

public class PatchDateDto (long id, string? name = null, DateOnly? dateOnly = null)
{
    public long Id { get; set; } = id;
    public string? Name { get; set; } = name;
    public DateOnly? DateOnly { get; set; } = dateOnly;
}