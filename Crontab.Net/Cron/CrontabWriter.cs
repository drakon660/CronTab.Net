using System.Text;
using CliWrap;

namespace Crontab.Net.Cron;

public sealed class CrontabWriter : ICrontabWriter
{
    public async Task<(int ExitCode, string Output)> WriteTextAsync(string cronTab)
    {
        string tempFile = Path.GetTempFileName();
        
        if (!cronTab.EndsWith(Environment.NewLine))
        {
            cronTab += Environment.NewLine;
        }
        
        await File.WriteAllTextAsync(tempFile, cronTab);
            
        StringBuilder outputBuilder = new StringBuilder();

        try
        {
            var result = await Cron(tempFile, in outputBuilder).ExecuteAsync();
            return (result.ExitCode, outputBuilder.ToString());
        }
        catch
        {
            //skip stacktrace
        }
        finally
        {
            File.Delete(tempFile);
        }
        return (-1, outputBuilder.ToString());
    }
    public async Task<(int ExitCode, string Output)> WriteAsync(string cronTabFileName)
    {
        var fileText = await File.ReadAllTextAsync(cronTabFileName);
        
        if (!fileText.EndsWith(Environment.NewLine))
        {
            fileText += Environment.NewLine;
            await File.WriteAllTextAsync(cronTabFileName, fileText);
        }
            
        StringBuilder outputBuilder = new StringBuilder();

        try
        {
            var result = await Cron(cronTabFileName, in outputBuilder).ExecuteAsync();
            return (result.ExitCode, outputBuilder.ToString());
        }
        catch
        {
            //skip stacktrace
        }
        return (-1, outputBuilder.ToString());
    }
    public async Task<(int ExitCode, string Output)> ListAsync()
    {
        StringBuilder outputBuilder = new StringBuilder();

        try
        {
            var result = await Cron("-l", in outputBuilder)
                .ExecuteAsync();
            
            return (result.ExitCode, outputBuilder.ToString());
        }
        catch
        {
            //skip stack trace
        }

        return (-1, outputBuilder.ToString());
    }

    public async Task<(int ExitCode, string Output)> DeleteAsync()
    {
        StringBuilder outputBuilder = new StringBuilder();
        
        try
        {
            var result = await Cron("-r", in outputBuilder)
                .ExecuteAsync();

            if (result is null)
                return (-1, outputBuilder.ToString());

            return (result.ExitCode, outputBuilder.ToString());
        }
        catch
        {
            //skip stack trace
        }

        return (-1, outputBuilder.ToString());
    }

    private Command Cron(string arguments, in StringBuilder outputBuilder) => Cli.Wrap("crontab").WithArguments(arguments)
        .WithWorkingDirectory("").WithStandardOutputPipe(PipeTarget.ToStringBuilder(outputBuilder))
        .WithStandardErrorPipe(PipeTarget.ToStringBuilder(outputBuilder));
}