using Crontab.Net.Cron;
using MediatR;

namespace Crontab.Net.Handlers;

public class CrontabListHandler : CrontabHandlerBase, IRequestHandler<CrontabListRequest, IEnumerable<CrontabItemDto>>
{
    public CrontabListHandler(ICrontabWriter crontabWriter) : base(crontabWriter)
    {
    }

    public async Task<IEnumerable<CrontabItemDto>> Handle(CrontabListRequest request,
        CancellationToken cancellationToken)
    {
        var crontabList = await GetCrontab();

        return crontabList.Select(x =>
            new CrontabItemDto(x.Cron.ToString(), x.Cron.GetNextOccurrence(DateTime.Now), x.Task));
    }
}

public record CrontabListRequest : IRequest<IEnumerable<CrontabItemDto>>
{
}

public record CrontabItemDto(string Cron, DateTime NextOccurence, string Task);