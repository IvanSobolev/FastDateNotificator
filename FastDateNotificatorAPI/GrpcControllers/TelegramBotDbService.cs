using FastDateNotificatorAPI.CommonProtos;
using FastDateNotificatorAPI.Managers.Interfaces;
using FastDateNotificatorAPI.Models.DTOs;
using FastDateNotificatorAPI.TelegramBotDateDb;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace FastDateNotificatorAPI.GrpcControllers;

public class TelegramBotDbService (IRememberedDateManager manager) : TelegramBotService.TelegramBotServiceBase
{
    private readonly IRememberedDateManager _manager = manager;
    public override async Task<Empty> AddDate(AddDateRequest request, ServerCallContext context)
    {
        var result =
            await _manager.AddAsync(new AddDateDto
                (
                    request.TelegramId, 
                    request.Name, 
                    DateOnly.Parse(request.Date)
                ));
        
        if (!result.IsSuccess)
            throw new RpcException(new Status(StatusCode.Internal, result.Error));

        return new Empty();
    }

    public override async Task<RememberedDateDto> GetDate(IdRequest request, ServerCallContext context)
    {
        var result = await _manager.GetAsync(request.Id);
        
        if (!result.IsSuccess)
            throw new RpcException(new Status(StatusCode.Internal, result.Error));

        var response = new RememberedDateDto()
        {
            Id = result.Value.Id,
            TelegramId = result.Value.TelegramId,
            Name = result.Value.Name,
            Date = result.Value.Date.ToString("yyyy-MM-dd"),
        };
        return response;
    }

    public override async Task<UserDatesResponse> GetForUser(UserRequest request, ServerCallContext context)
    {
        var result = await _manager.GetForUserAsync(request.TelegramId);

        if (!result.IsSuccess)
            throw new RpcException(new Status(StatusCode.Internal, result.Error));

        var response = new UserDatesResponse();
        response.Dates.AddRange(result.Value.Select(d => new RememberedDateDto
        {
            Id = d.Id,
            TelegramId = d.TelegramId,
            Name = d.Name,
            Date = d.Date.ToString("yyyy-MM-dd")
        }));

        return response;
    }

    public override async Task<RememberedDateDto> PatchDate(PatchDateRequest request, ServerCallContext context)
    {
        var result = await _manager.PatchAsync(new PatchDateDto
        (
            request.Id, 
            request.Name, 
            DateOnly.Parse(request.Date)
        ));
        
        if (!result.IsSuccess)
            throw new RpcException(new Status(StatusCode.Internal, result.Error));

        var response = new RememberedDateDto()
        {
            Id = result.Value.Id,
            TelegramId = result.Value.TelegramId,
            Name = result.Value.Name,
            Date = result.Value.Date.ToString("yyyy-MM-dd"),
        };
        return response;
    }

    public override async Task<RememberedDateDto> UpdateDate(UpdateDateRequest request, ServerCallContext context)
    {
        var result = await _manager.UpdateAsync(new UpdateDateDto
        (
            request.Id, 
            request.TelegramId,
            request.Name, 
            DateOnly.Parse(request.Date)
        ));
        
        if (!result.IsSuccess)
            throw new RpcException(new Status(StatusCode.Internal, result.Error));

        var response = new RememberedDateDto()
        {
            Id = result.Value.Id,
            TelegramId = result.Value.TelegramId,
            Name = result.Value.Name,
            Date = result.Value.Date.ToString("yyyy-MM-dd"),
        };
        return response;
    }

    public override async Task<Empty> DeleteDate(IdRequest request, ServerCallContext context)
    {
        var result = await _manager.DeleteAsync(request.Id);
        
        if (!result.IsSuccess)
            throw new RpcException(new Status(StatusCode.Internal, result.Error));

        return new Empty();
    }
}