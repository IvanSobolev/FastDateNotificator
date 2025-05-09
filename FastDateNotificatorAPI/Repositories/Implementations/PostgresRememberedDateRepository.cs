using FastDateNotificatorAPI.Abstractions;
using FastDateNotificatorAPI.Models;
using FastDateNotificatorAPI.Models.Entities;
using FastDateNotificatorAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FastDateNotificatorAPI.Repositories.Implementations;

public class PostgresRememberedDateRepository (DataContext dataContext) : IRememberedDateRepository
{
    private readonly DataContext _dataContext = dataContext;
    
    public async Task<Result> AddAsync(long telegramId, string name, DateOnly date)
    {
        var entity = new RememberedDate
        {
            TelegramId = telegramId,
            Name = name,
            Date = date
        };

        _dataContext.RememberedDates.Add(entity);
        var changes = await _dataContext.SaveChangesAsync();
        return changes > 0 ? Result.Success() : Result.Failure("Failed to add remembered date.", 500);
    }

    public async Task<Result<RememberedDate>> GetAsync(long id)
    {
        var entity = await _dataContext.RememberedDates.FindAsync(id);

        return entity is not null
            ? Result<RememberedDate>.Success(entity)
            : Result<RememberedDate>.Failure("Remembered date not found.", 404);
    }

    public async Task<Result<ICollection<long>>> GetAllUserIdAsync()
    {
        var ids = await _dataContext.RememberedDates
            .Select(x => x.TelegramId)
            .Distinct()
            .ToListAsync();

        return Result<ICollection<long>>.Success(ids);
    }

    public async Task<Result<ICollection<RememberedDate>>> GetForUserAsync(long telegramId)
    {
        var list = await _dataContext.RememberedDates
            .Where(x => x.TelegramId == telegramId)
            .OrderBy(x => x.Date)
            .ToListAsync();

        return Result<ICollection<RememberedDate>>.Success(list);
    }

    public async Task<Result<RememberedDate>> PatchAsync(long id, string? name = null, DateOnly? dateOnly = null)
    {
        if (name is null && dateOnly is null)
            return Result<RememberedDate>.Failure("Nothing to patch.", 400);

        var setClauses = new List<string>();
        var parameters = new List<NpgsqlParameter>();

        if (name is not null)
        {
            setClauses.Add("name = @name");
            parameters.Add(new NpgsqlParameter("name", name));
        }

        if (dateOnly is not null)
        {
            setClauses.Add("date = @date");
            parameters.Add(new NpgsqlParameter("date", dateOnly));
        }

        parameters.Add(new NpgsqlParameter("id", id));
        var setClauseSql = string.Join(", ", setClauses);

        var sql = $"UPDATE \"RememberedDates\" SET {setClauseSql} WHERE \"Id\" = @id RETURNING *;";

        var updatedEntity = await _dataContext.RememberedDates
            .FromSqlRaw(sql, parameters.ToArray())
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (updatedEntity is null)
            return Result<RememberedDate>.Failure("Remembered date not found.", 404);

        return Result<RememberedDate>.Success(updatedEntity);
    }

    public async Task<Result<RememberedDate>> UpdateAsync(long id, long telegramId, string name, DateOnly date)
    {
        var entity = new RememberedDate
        {
            Id = id,
            TelegramId = telegramId,
            Name = name,
            Date = date
        };

        _dataContext.RememberedDates.Attach(entity);
        _dataContext.Entry(entity).Property(e => e.TelegramId).IsModified = true;
        _dataContext.Entry(entity).Property(e => e.Name).IsModified = true;
        _dataContext.Entry(entity).Property(e => e.Date).IsModified = true;

        await _dataContext.SaveChangesAsync();

        return Result<RememberedDate>.Success(entity);
    }

    public async Task<Result> DeleteAsync(long id)
    {
        var entity = new RememberedDate { Id = id };
        _dataContext.RememberedDates.Attach(entity);
        _dataContext.RememberedDates.Remove(entity);
        var changes = await _dataContext.SaveChangesAsync();

        return changes > 0 
            ? Result.Success() 
            : Result.Failure("Delete failed.", 500);
    }
}