using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;
using CSharpFunctionalExtensions;
using NCrontab;

namespace Crontab.Net;

// public class CronTabList : IList<(string cron,string task)>
// {
//     const string Demo = "* * * * * sh run something " +
//     "\n*/5 * * * * sh run something2 " +
//     "\n0 2 * * 1-5 python /path/to/my/script.py " +
//     "\n0 */2 * * * sh /path/to/my/script.sh " +
//     "\n30 4,16 * * * perl /path/to/my/script.pl " +
//     "\n0 0 * * 0 tar -czvf /path/to/backup.tar.gz /path/to/files/ " +
//     "\n*/15 * * * * python3 /path/to/my/script.py arg1 arg2 " +
//     "\n0 0 1 * * php /path/to/my/script.php " +
//     "\n*/10 * * * * sh /path/to/my/script.sh >> /var/log/mylog.log " +
//     "\n0 8,16 * * 1-5 /usr/bin/python3 /path/to/my/script.py >> /var/log/mylog.log 2>&1";
//     
//     private readonly List<(string Cron, string Task)> _cronValues;
//
//     private CronTabList(List<(string Cron, string Task)> values)
//     {
//         _cronValues = values;
//     }
//
//     public static async Task<CronTabList> FromDemo()
//     {
//         List<(string Cron, string Task)> items = new List<(string Cron, string Task)>();
//         string textLine;
//
//         using StringReader reader = new StringReader(Demo);
//         while ((textLine = await reader.ReadLineAsync()) != null)
//         {
//             if (!textLine.StartsWith('#'))
//             {
//                 items.Add(SplitValues(textLine));
//             }
//         }
//
//         return new CronTabList(items);
//     }
//     
//     public static async Task<CronTabList> FromAsync(string cronTab)
//     {
//         List<(string Cron, string Task)> items = new List<(string Cron, string Task)>();
//         string textLine;
//
//         using StringReader reader = new StringReader(cronTab);
//         while ((textLine = await reader.ReadLineAsync()) != null)
//         {
//             if (!textLine.StartsWith('#'))
//             {
//                 items.Add(SplitValues(textLine));
//             }
//         }
//
//         return new CronTabList(items);
//     }
//
//     public string ToCronTab()
//     {
//         StringBuilder stringBuilder = new StringBuilder();
//
//         foreach (var cronTabItem in _cronValues)
//         {
//             stringBuilder.AppendLine($"{cronTabItem.Cron} {cronTabItem.Task}");
//         }
//
//         return stringBuilder.ToString();
//     }
//
//     public static (string CronPart, string TaskPart) SplitValues(string cronRow)
//     {
//         if (cronRow.StartsWith('@'))
//         {
//             throw new NotImplementedException("@ is not supported");
//         }
//         else
//         {
//             var splitted = cronRow.Trim().Split(null);
//             var cronPart = splitted.Take(5);
//             var taskPart = splitted.Skip(5).Take(splitted.Length - 5);
//
//             return (string.Join(" ", cronPart), string.Join(" ", taskPart));
//         }
//     }
//
//     public IEnumerator<(string cron, string task)> GetEnumerator()
//     {
//         foreach (var item in _cronValues)
//         {
//             yield return item;
//         }
//     }
//
//     IEnumerator IEnumerable.GetEnumerator()
//     {
//         return GetEnumerator();
//     }
//
//     public void Add((string cron, string task) item)
//     {
//         CrontabSchedule crontabSchedule = CrontabSchedule.TryParse(item.cron);
//
//         if (crontabSchedule is not null)
//             _cronValues.Add(item);
//     }
//
//     public void Clear()
//     {
//        _cronValues.Clear();
//     }
//
//     public bool Contains((string cron, string task) item)
//     {
//         return _cronValues.Contains(item);
//     }
//
//     public void CopyTo((string cron, string task)[] array, int arrayIndex)
//     {
//         _cronValues.CopyTo(array, arrayIndex);
//     }
//
//     public bool Remove((string cron, string task) item)
//     {
//         return _cronValues.Remove(item);
//     }
//
//     public int Count => _cronValues.Count;
//     public bool IsReadOnly { get; }
//     public int IndexOf((string cron, string task) item)
//     {
//         return  _cronValues.IndexOf(item);
//     }
//
//     public void Insert(int index, (string cron, string task) item)
//     {
//        _cronValues.Insert(index, item);
//     }
//
//     public void RemoveAt(int index)
//     {
//         _cronValues.RemoveAt(index);
//     }
//
//     public (string cron, string task) this[int index]
//     {
//         get => _cronValues[index];
//         set => _cronValues[index] = value;
//     }
// }

public class CronTabList : IList<(CrontabSchedule Cron, string Task)>
{
    const string Demo = "* * * * * sh run something " +
                        "\n*/5 * * * * sh run something2 " +
                        "\n0 2 * * 1-5 python /path/to/my/script.py " +
                        "\n0 */2 * * * sh /path/to/my/script.sh " +
                        "\n30 4,16 * * * perl /path/to/my/script.pl " +
                        "\n0 0 * * 0 tar -czvf /path/to/backup.tar.gz /path/to/files/ " +
                        "\n*/15 * * * * python3 /path/to/my/script.py arg1 arg2 " +
                        "\n0 0 1 * * php /path/to/my/script.php " +
                        "\n*/10 * * * * sh /path/to/my/script.sh >> /var/log/mylog.log " +
                        "\n0 8,16 * * 1-5 /usr/bin/python3 /path/to/my/script.py >> /var/log/mylog.log 2>&1";

    private readonly List<(CrontabSchedule Cron, string Task)> _cronValues;
    private readonly EqualityComparer<CrontabSchedule> _comparer = new ();

    private CronTabList(List<(CrontabSchedule Cron, string Task)> values)
    {
        
        _cronValues = values;
    }

    public static async Task<CronTabList> FromDemo()
    {
        List<(CrontabSchedule Cron, string Task)> items = new List<(CrontabSchedule Cron, string Task)>();
        string textLine;

        using StringReader reader = new StringReader(Demo);
        while ((textLine = await reader.ReadLineAsync()) != null)
        {
            if (!textLine.StartsWith('#'))
            {
                items.Add(SplitValues(textLine));
            }
        }

        return new CronTabList(items);
    }

    public static async Task<CronTabList> FromAsync(string cronTab)
    {
        List<(CrontabSchedule Cron, string Task)> items = new List<(CrontabSchedule Cron, string Task)>();
        string textLine;

        using StringReader reader = new StringReader(cronTab);
        while ((textLine = await reader.ReadLineAsync()) != null)
        {
            if (!textLine.StartsWith('#'))
            {
                items.Add(SplitValues(textLine));
            }
        }

        return new CronTabList(items);
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

    public static (CrontabSchedule Cron, string Task) SplitValues(string cronRow)
    {
        if (cronRow.StartsWith('@'))
        {
            throw new NotImplementedException("@ is not supported");
        }
        else
        {
            var splitted = cronRow.Trim().Split(null);
            var cronPart = splitted.Take(5);
            var taskPart = splitted.Skip(5).Take(splitted.Length - 5);

            return (CrontabSchedule.Parse(string.Join(" ", cronPart)), string.Join(" ", taskPart));
        }
    }

    public IEnumerator<(CrontabSchedule Cron, string Task)> GetEnumerator()
    {
        foreach (var item in _cronValues)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add((CrontabSchedule Cron, string Task) item)
    {
        _cronValues.Add(item);
    }

    public void Clear()
    {
        _cronValues.Clear();
    }

    public bool Contains((CrontabSchedule Cron, string Task) item)
    {
        return _cronValues.Contains(item);
    }

    public void CopyTo((CrontabSchedule Cron, string Task)[] array, int arrayIndex)
    {
        _cronValues.CopyTo(array, arrayIndex);
    }

    public bool Remove((CrontabSchedule Cron, string Task) item)
    {
        return _cronValues.Remove(item);
    }

    public int Count => _cronValues.Count;
    public bool IsReadOnly { get; }

    public int IndexOf((CrontabSchedule Cron, string Task) item)
    {
        for (int i = 0; i < _cronValues.Count; i++)
        {
            if (_comparer.Equals(_cronValues[i].Cron,item.Cron) &&
                _cronValues[i].Task.Equals(item.Task))
            {
                return i;
            }
        }
        return -1;
    }

    public void Insert(int index, (CrontabSchedule Cron, string Task) item)
    {
       _cronValues.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        _cronValues.RemoveAt(index);
    }

    public (CrontabSchedule Cron, string Task) this[int index]
    {
        get => _cronValues[index];
        set => _cronValues[index] = value;
    }
}

sealed class EqualityComparer<CrontabSchedule> : IEqualityComparer<CrontabSchedule>
{
    public bool Equals(CrontabSchedule x, CrontabSchedule y) =>
        x?.ToString() == y?.ToString();

    public int GetHashCode(CrontabSchedule obj) =>
        obj.ToString().GetHashCode();
}