using FastDateNotificatorAPI.Abstractions;
using FastDateNotificatorAPI.Managers.Interfaces;
using FastDateNotificatorAPI.Models.DTOs;
using FastDateNotificatorAPI.Models.Entities;
using FastDateNotificatorAPI.Repositories.Interfaces;

namespace FastDateNotificatorAPI.Managers.Implementations;

public class RemembaredDateManager (IRememberedDateRepository repository) : IRememberedDateManager
{
    private readonly IRememberedDateRepository _repository = repository;
    
    public async Task<Result> AddAsync(AddDateDto addDate)
    {
        return await _repository.AddAsync(addDate.TelegramId, addDate.Name, addDate.Date);
    }

    public async Task<Result<RememberedDate>> GetAsync(long id)
    {
        return await _repository.GetAsync(id);
    }

    public async Task<Result<ICollection<RememberedDate>>> GetForUserAsync(long telegramId)
    {
        return await _repository.GetForUserAsync(telegramId);
    }

    public async Task<Result<RememberedDate>> PatchAsync(PatchDateDto patchDate)
    {
        return await _repository.PatchAsync(patchDate.Id, patchDate.Name, patchDate.DateOnly);
    }

    public async Task<Result<RememberedDate>> UpdateAsync(UpdateDateDto updateDate)
    {
        return await _repository.UpdateAsync(updateDate.Id, updateDate.TelegramId, updateDate.Name, updateDate.Date);
    }

    public async Task<Result> DeleteAsync(long id)
    {
        return await _repository.DeleteAsync(id);
    }
}