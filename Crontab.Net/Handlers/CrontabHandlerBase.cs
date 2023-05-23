using Crontab.Net.Cron;

namespace Crontab.Net.Handlers;

public abstract class CrontabHandlerBase
{
    private readonly ICrontabWriter _crontabWriter;

    protected CrontabHandlerBase(ICrontabWriter crontabWriter)
    {
        _crontabWriter = crontabWriter;
    }

    protected async Task<CrontabList> GetCrontab()
    {
        var result = await _crontabWriter.ListAsync();
        return await CrontabList.FromAsync(result.Output);
    }

    protected async Task WriteCrontab(CrontabList crontabList)
    {
        var crontab = crontabList.ToCronTab();
        await _crontabWriter.WriteTextAsync(crontab);
    }
}