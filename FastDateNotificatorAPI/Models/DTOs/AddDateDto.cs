namespace FastDateNotificatorAPI.Models.DTOs;

public class AddDateDto (long tgId, string name, DateOnly dateOnly)
{
    public long TelegramId { get; set; } = tgId;
    public string Name { get; set; } = name;
    public DateOnly Date { get; set; } = dateOnly;
}