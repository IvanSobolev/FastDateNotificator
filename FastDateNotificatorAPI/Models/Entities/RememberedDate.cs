namespace FastDateNotificatorAPI.Models.Entities;

public class RememberedDate
{
    public long Id { get; set; }
    public long TelegramId { get; set; }
    public string Name { get; set; }
    public DateOnly Date { get; set; }
}