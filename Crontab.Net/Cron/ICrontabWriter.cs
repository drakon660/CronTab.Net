namespace Crontab.Net.Cron;

public interface ICrontabWriter
{
    Task<(int ExitCode, string Output)> ListAsync();
    Task<(int ExitCode, string Output)> WriteTextAsync(string cronTab);
    Task<(int ExitCode, string Output)> WriteAsync(string cronTabFileName);
    Task<(int ExitCode, string Output)> DeleteAsync();
}