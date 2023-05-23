using Crontab.Net.Cron;
using MediatR;

namespace Crontab.Net.Handlers;

public class CrontabDeleteHandler : CrontabHandlerBase, IRequestHandler<CrontabDeleteRequest>
{
    public CrontabDeleteHandler(ICrontabWriter crontabWriter) : base(crontabWriter)
    {
    }

    public async Task Handle(CrontabDeleteRequest request, CancellationToken cancellationToken)
    {
        var crontabList = await GetCrontab();

        if (request.Index == -1)
            crontabList.Clear();
        else
            crontabList.RemoveAt(request.Index);

        await WriteCrontab(crontabList);
    }
}

public record CrontabDeleteRequest(int Index) : IRequest;