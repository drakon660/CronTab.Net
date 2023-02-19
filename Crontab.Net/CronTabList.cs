using System.Collections;
using System.Text;
using NCrontab;

namespace Crontab.Net;

public class CronTabList : IReadOnlyCollection<(string,string)>
{
    public List<(string Cron, string Task)> Items = new List<(string Cron, string Task)>();

    public CronTabList()
    {
    }

    public static async Task<CronTabList> FromAsync(string cronTab)
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
        
        return new CronTabList() {  Items =  items };
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
        CrontabSchedule crontabSchedule = CrontabSchedule.TryParse(cron);
        
        if(crontabSchedule is not null)
            Items.Add((cron,task));
    }

    public static (string CronPart, string TaskPart) SplitValues(string cronRow)
    {
        var splitted = cronRow.Split(null);
        var cronPart = splitted.Take(5);
        var taskPart = splitted.Skip(5).Take(splitted.Length - 5);

        return (string.Join(" ", cronPart), string.Join(" ", taskPart));
    }

    public IEnumerator<(string, string)> GetEnumerator()
    {
        foreach (var item in Items)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count { get; }
}

