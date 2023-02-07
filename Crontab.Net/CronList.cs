using System.Collections;
using System.ComponentModel;
using System.Text;
using NCrontab;

namespace Crontab.Net;

public class CronList
{
    public List<(string Cron, string Task)> Items = new List<(string Cron, string Task)>();

    private CronList()
    {
    }

    public static async Task<CronList> FromAsync(string cronTab)
    {
        List<(string Cron, string Task)> items = new List<(string Cron, string Task)>();
        string textLine;

        using StringReader reader = new StringReader(cronTab);
        while ((textLine = await reader.ReadLineAsync()) != null)
        {
            if (!textLine.StartsWith('#'))
            {
                items.Add(SplitValues(textLine));
            }
        }

        return new CronList() {  Items =  items };
    }

    public string ToCronTab()
    {
        StringBuilder stringBuilder = new StringBuilder();

        foreach (var cronTabItem in Items)
        {
            stringBuilder.AppendLine($"{cronTabItem.Cron} {cronTabItem.Task}");
        }
        
        return stringBuilder.ToString();
    }

    public void AddCronTab(string cron, string task)
    {
        Items.Add((cron,task));
    }

    public static (string CronPart, string TaskPart) SplitValues(string cronRow)
    {
        var splitted = cronRow.Split(null);
        var cronPart = splitted.Take(5);
        var taskPart = splitted.Skip(5).Take(splitted.Length - 5);

        return (string.Join(" ", cronPart), string.Join(" ", taskPart));
    }
}
