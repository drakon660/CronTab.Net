using FluentAssertions;
using NCrontab;

namespace Crontab.Net.Tests;

public class CronTabListTests
{
    private readonly string _crontab;
    public CronTabListTests()
    {
        _crontab = "* * * * * sh run cos " +
                         "\n*/5 * * * * sh run cos1 " +
                         "\n0 2 * * 1-5 python /path/to/my/script.py " +
                         "\n0 */2 * * * sh /path/to/my/script.sh " +
                         "\n30 4,16 * * * perl /path/to/my/script.pl " +
                         "\n0 0 * * 0 tar -czvf /path/to/backup.tar.gz /path/to/files/ " +
                         "\n*/15 * * * * python3 /path/to/my/script.py arg1 arg2 " +
                         "\n0 0 1 * * php /path/to/my/script.php " +
                         "\n*/10 * * * * sh /path/to/my/script.sh >> /var/log/mylog.log " +
                         "\n0 8,16 * * 1-5 /usr/bin/python3 /path/to/my/script.py >> /var/log/mylog.log 2>&1";
    }
    
    [Fact]
    public async Task Check_Count()
    {
        CrontabList crontabList = await CrontabList.FromAsync(_crontab);
        crontabList.Count.Should().Be(10);
    }
    
    [Fact]
    public async Task Check_List()
    {
       var cronTabList = await CrontabList.FromAsync(_crontab);
        cronTabList.Should().BeEquivalentTo(new List<(CrontabSchedule,string)>()
        {
            ("* * * * *".ToCron(), "sh run cos"),
            ("*/5 * * * *".ToCron(), "sh run cos1"),
            ("0 2 * * 1-5".ToCron(), "python /path/to/my/script.py"),
            ("0 */2 * * *".ToCron(), "sh /path/to/my/script.sh"),
            ("30 4,16 * * *".ToCron(), "perl /path/to/my/script.pl"),
            ("0 0 * * 0".ToCron(), "tar -czvf /path/to/backup.tar.gz /path/to/files/"),
            ("*/15 * * * *".ToCron(), "python3 /path/to/my/script.py arg1 arg2"),
            ("0 0 1 * *".ToCron(), "php /path/to/my/script.php"),
            ("*/10 * * * *".ToCron(), "sh /path/to/my/script.sh >> /var/log/mylog.log"),
            ("0 8,16 * * 1-5".ToCron(), "/usr/bin/python3 /path/to/my/script.py >> /var/log/mylog.log 2>&1")
        });
    }
    
    [Fact]
    public async Task Check_Add_As_Last()
    {
        var cronTabList = await CrontabList.FromAsync(_crontab);
        var item = (CrontabSchedule.Parse("0 0 3 * *"), "sh -1 das");
        cronTabList.Add(item);

        var indexOfAdded = cronTabList.IndexOf(item);
        indexOfAdded.Should().Be(cronTabList.Count -1);
    }
    
    [Fact]
    public async Task Check_Add_On_Position()
    {
        var cronTabList = await CrontabList.FromAsync(_crontab);
        var item = (CrontabSchedule.Parse("0 0 3 * *"), "sh -1 das");
        var removed = ("0 2 * * 1-5".ToCron(), "python /path/to/my/script.py");
        cronTabList.Insert(2, item);

        var indexOfAdded = cronTabList.IndexOf(item);
        var indexOfRemoved = cronTabList.IndexOf(removed);
        
        indexOfAdded.Should().Be(2);
        indexOfRemoved.Should().Be(3);
    }
    
    [Fact]
    public async Task Check_Remove_At_Position()
    {
        var cronTabList = await CrontabList.FromAsync(_crontab);
        cronTabList.RemoveAt(2);

        cronTabList.Count.Should().Be(9);
    }
}