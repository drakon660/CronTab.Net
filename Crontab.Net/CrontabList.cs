using System.Collections;
using System.Text;
using NCrontab;

namespace Crontab.Net;

public sealed class CrontabList : IList<(CrontabSchedule Cron, string Task)>
{
    private const char CommentSign = '#';
    private readonly List<(CrontabSchedule Cron, string Task)> _cronValues;
    private readonly EqualityComparer _comparer = new();

    private CrontabList(List<(CrontabSchedule Cron, string Task)> values)
    {
        _cronValues = values;
    }

    public static async Task<CrontabList> FromAsync(string cronTab)
    {
        var items = new List<(CrontabSchedule Cron, string Task)>();

        using StringReader reader = new StringReader(cronTab);
        while (await reader.ReadLineAsync() is { } textLine)
        {
            if (!textLine.StartsWith(CommentSign) && !string.IsNullOrWhiteSpace(textLine))
            {
                items.Add(SplitValues(textLine));
            }
        }

        return new CrontabList(items);
    }

    public string ToCronTab()
    {
        StringBuilder stringBuilder = new StringBuilder();

        foreach (var cronTabItem in _cronValues)
        {
            stringBuilder.AppendLine($"{cronTabItem.Cron} {cronTabItem.Task}");
        }

        return stringBuilder.ToString();
    }

    private static (CrontabSchedule Cron, string Task) SplitValues(string cronRow)
    {
        if (cronRow.StartsWith('@'))
        {
            throw new NotImplementedException("@ is not supported");
        }

        var split = cronRow.Trim().Split(null);
        var cronPart = split.Take(5);
        var taskPart = split.Skip(5).Take(split.Length - 5);

        return (CrontabSchedule.Parse(string.Join(" ", cronPart)), string.Join(" ", taskPart));
    }

    public IEnumerator<(CrontabSchedule Cron, string Task)> GetEnumerator()
    {
        foreach (var item in _cronValues)
            yield return item;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add((CrontabSchedule Cron, string Task) item) => _cronValues.Add(item);

    public void Clear() => _cronValues.Clear();

    public bool Contains((CrontabSchedule Cron, string Task) item) => _cronValues.Contains(item);

    public void CopyTo((CrontabSchedule Cron, string Task)[] array, int arrayIndex) =>
        _cronValues.CopyTo(array, arrayIndex);

    public bool Remove((CrontabSchedule Cron, string Task) item) => _cronValues.Remove(item);

    public int Count => _cronValues.Count;
    public bool IsReadOnly { get; }

    public int IndexOf((CrontabSchedule Cron, string Task) item)
    {
        for (int i = 0; i < _cronValues.Count; i++)
        {
            if (_comparer.Equals(_cronValues[i].Cron, item.Cron) &&
                _cronValues[i].Task.Equals(item.Task))
            {
                return i;
            }
        }

        return -1;
    }

    public void Insert(int index, (CrontabSchedule Cron, string Task) item) => _cronValues.Insert(index, item);

    public void Update(int index, (CrontabSchedule Cron, string Task) item) => _cronValues[index] = item;

    public void RemoveAt(int index) => _cronValues.RemoveAt(index);

    public (CrontabSchedule Cron, string Task) this[int index]
    {
        get => _cronValues[index];
        set => _cronValues[index] = value;
    }
}

internal sealed class EqualityComparer : IEqualityComparer<CrontabSchedule>
{
    public bool Equals(CrontabSchedule x, CrontabSchedule y) =>
        x?.ToString() == y?.ToString();

    public int GetHashCode(CrontabSchedule obj) =>
        obj.ToString().GetHashCode();
}