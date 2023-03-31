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

public class Crontab : IList<(string, string)>
{
    private (string, string)[] _array;

    public Crontab(int capacity)
    {
        _array = new (string, string)[capacity];
    }

    public (string, string) this[int index]
    {
        get => _array[index];
        set => _array[index] = value;
    }

    public int Count => _array.Length;

    public bool IsReadOnly => false;

    public void Add((string, string) item)
    {
        if (Count == _array.Length)
        {
            Array.Resize(ref _array, _array.Length * 2);
        }
        _array[Count] = item;
    }

    public void Clear()
    {
        Array.Clear(_array, 0, Count);
    }

    public bool Contains((string, string) item)
    {
        for (int i = 0; i < Count; i++)
        {
            if (_array[i].Equals(item))
            {
                return true;
            }
        }
        return false;
    }

    public void CopyTo((string, string)[] array, int arrayIndex)
    {
        Array.Copy(_array, 0, array, arrayIndex, Count);
    }

    public IEnumerator<(string, string)> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
        {
            yield return _array[i];
        }
    }

    public int IndexOf((string, string) item)
    {
        for (int i = 0; i < Count; i++)
        {
            if (_array[i].Equals(item))
            {
                return i;
            }
        }
        return -1;
    }

    public void Insert(int index, (string, string) item)
    {
        if(_array.Length - 1 >= index)
            _array[index] = item;
    }

    public bool Remove((string, string) item)
    {
        int index = IndexOf(item);
        if (index == -1)
        {
            return false;
        }
        RemoveAt(index);
        return true;
    }

    public  void RemoveAt(int index)
    {
        for (int i = index; i < _array.Length - 1; i++)
        {
            // moving elements downwards, to fill the gap at [index]
            _array[i] = _array[i + 1];
        }
        // finally, let's decrement Array's size by one
        Array.Resize(ref _array, _array.Length - 1);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}