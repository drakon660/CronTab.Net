using Crontab.Net.Cron;
using MediatR;
using NCrontab;

namespace Crontab.Net.Handlers;

public sealed class CrontabUpdateHandler : CrontabHandlerBase, IRequestHandler<CrontabUpdateRequest>
{
    public CrontabUpdateHandler(ICrontabWriter crontabWriter) : base(crontabWriter)
    {
    }

    public async Task Handle(CrontabUpdateRequest request, CancellationToken cancellationToken)
    {
        var crontabList = await GetCrontab();

        crontabList.Update(request.CrontabItemDto.Index,
            (CrontabSchedule.Parse(request.CrontabItemDto.Cron), request.CrontabItemDto.Task));

        await WriteCrontab(crontabList);
    }
}

public record CrontabUpdateRequest : IRequest
{
    public CrontabItemUpdateDto CrontabItemDto { get; init; }
}

public record CrontabItemUpdateDto(int Index, string Cron, string Task);