// See https://aka.ms/new-console-template for more information

using System.Runtime.InteropServices;
using System.Text;
using CliWrap;
using NCrontab;

Console.WriteLine("Hello, World!234324");

Console.WriteLine(RuntimeInformation.RuntimeIdentifier);

var stringBuilder = new StringBuilder();

var result = await Cli.Wrap("crontab")
    .WithArguments("-l")
    .WithWorkingDirectory("").WithStandardOutputPipe(PipeTarget.ToStringBuilder(stringBuilder))
    .ExecuteAsync();

string readText;


NCrontab.CrontabSchedule.Parse("");

Console.WriteLine(stringBuilder.ToString());

