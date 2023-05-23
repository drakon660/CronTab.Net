namespace Crontab.Net.Cron;

public sealed class FakeCrontabWriter : ICrontabWriter
{
    public Task<(int ExitCode, string Output)> ListAsync()
    {
        return Task.FromResult((1,Demo.Data));
    }

    public Task<(int ExitCode, string Output)> WriteTextAsync(string cronTab)
    {
        Demo.Data = cronTab;
        return Task.FromResult((1,Demo.Data));
    }

    public Task<(int ExitCode, string Output)> WriteAsync(string cronTabFileName)
    {
        throw new NotImplementedException();
    }

    public Task<(int ExitCode, string Output)> DeleteAsync()
    {
        Demo.Data = string.Empty;
        return Task.FromResult((1,Demo.Data));
    }
}