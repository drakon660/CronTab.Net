using System.Collections;
using NCrontab;

namespace Crontab.Net;

public class CronList
{
    public List<(string Cron, string Task)> Items = new List<(string Cron, string Task)>();

    private CronList()
    {
    }

    public static async Task<CronList> From(string cronTab)
    {
        List<(string Cron, string Task)> Items = new List<(string Cron, string Task)>();
        string textLine;
        using (StringReader reader = new StringReader(cronTab))
        {
            while ((textLine = await reader.ReadLineAsync()) != null)
            {
                if (!textLine.StartsWith('#'))
                {
                    Items.Add(SplitValues(textLine));
                }
            }
        }

        return new CronList() {  Items =  Items };
    }

    public static (string CronPart, string TaskPart) SplitValues(string cronRow)
    {
        var splitted = cronRow.Split(null);
        var cronPart = splitted.Take(5);
        var taskPart = splitted.Skip(5).Take(splitted.Length - 5);

        return (string.Join(" ", cronPart), string.Join(" ", taskPart));
    }
}
