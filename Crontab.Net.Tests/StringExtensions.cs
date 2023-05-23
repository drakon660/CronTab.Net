using NCrontab;

namespace Crontab.Net.Tests;

public static class StringExtensions
{
    public static CrontabSchedule ToCron(this string cron)=> CrontabSchedule.Parse(cron);
}