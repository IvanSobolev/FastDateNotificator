using FastDateNotificatorAPI.Abstractions;
using FastDateNotificatorAPI.Models.DTOs;
using FastDateNotificatorAPI.Models.Entities;

namespace FastDateNotificatorAPI.Managers.Interfaces;

public interface IRememberedDateManager
{
    Task<Result> AddAsync(AddDateDto addDate);
    Task<Result<RememberedDate>> GetAsync(long id);
    Task<Result<ICollection<RememberedDate>>> GetForUserAsync(long telegramId);
    Task<Result<RememberedDate>> PatchAsync(PatchDateDto patchDate);
    Task<Result<RememberedDate>> UpdateAsync(UpdateDateDto updateDate);
    Task<Result> DeleteAsync(long id);
}