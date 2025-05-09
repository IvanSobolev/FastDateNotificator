namespace FastDateNotificatorAPI.Models.DTOs;

public class UpdateDateDto(long id, long telegramId, string name, DateOnly date)
{
    public long Id { get; set; } = id;
    public long TelegramId { get; set; } = telegramId;
    public string Name { get; set; } = name;
    public DateOnly Date { get; set; } = date;
}