using Crontab.Net.Cron;
using MediatR;
using NCrontab;

namespace Crontab.Net.Handlers;

public class CrontabInsertHandler : CrontabHandlerBase, IRequestHandler<CrontabInsertRequest>
{
    public CrontabInsertHandler(ICrontabWriter crontabWriter) : base(crontabWriter)
    {
    }

    public async Task Handle(CrontabInsertRequest request, CancellationToken cancellationToken)
    {
        var crontabList = await GetCrontab();

        if (request.CrontabItemDto.Index < 0)
            crontabList.Add((CrontabSchedule.Parse(request.CrontabItemDto.Cron), request.CrontabItemDto.Task));
        else
            crontabList.Insert(request.CrontabItemDto.Index,
                (CrontabSchedule.Parse(request.CrontabItemDto.Cron), request.CrontabItemDto.Task));

        await WriteCrontab(crontabList);
    }
}

public record CrontabInsertRequest : IRequest
{
    public CrontabItemInsertDto CrontabItemDto { get; init; }
}

public record CrontabItemInsertDto(int Index, string Cron, string Task);