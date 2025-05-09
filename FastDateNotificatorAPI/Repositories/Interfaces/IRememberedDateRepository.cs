using FastDateNotificatorAPI.Abstractions;
using FastDateNotificatorAPI.Models.Entities;

namespace FastDateNotificatorAPI.Repositories.Interfaces;

public interface IRememberedDateRepository
{
    Task<Result> AddAsync(long telegramId, string name, DateOnly date);
    Task<Result<RememberedDate>> GetAsync(long id);
    Task<Result<ICollection<RememberedDate>>> GetForUserAsync(long telegramId);
    Task<Result<RememberedDate>> PatchAsync(long id, string? name = null, DateOnly? dateOnly = null);
    Task<Result<RememberedDate>> UpdateAsync(long id, long telegramId, string name, DateOnly date);
    Task<Result> DeleteAsync(long id);
}