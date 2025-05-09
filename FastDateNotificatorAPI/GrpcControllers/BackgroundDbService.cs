using FastDateNotificatorAPI.BackgroundServiceDb;
using FastDateNotificatorAPI.CommonProtos;
using FastDateNotificatorAPI.Managers.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using BackgroundService = FastDateNotificatorAPI.BackgroundServiceDb.BackgroundService;

namespace FastDateNotificatorAPI.GrpcControllers;

public class BackgroundDbService (IRememberedDateManager manager) : BackgroundService.BackgroundServiceBase
{
    private readonly IRememberedDateManager _manager = manager;

    public override async Task<UserIdsResponse> GetAllUserIds(Empty request, ServerCallContext context)
    {
        var result = await _manager.GetAllUserIdAsync();

        if (!result.IsSuccess)
            throw new RpcException(new Status(StatusCode.Internal, result.Error));

        var response = new UserIdsResponse();
        response.Ids.AddRange(result.Value);
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
}