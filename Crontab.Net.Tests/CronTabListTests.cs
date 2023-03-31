using FluentAssertions;

namespace Crontab.Net.Tests;

public class CronTabListTests
{
    [Fact]
    public void Check_insert_method()
    {
        Crontab crontab = new Crontab(1);
        crontab.Insert(0, ("*****","cos"));
        crontab.Should().BeEquivalentTo(new (string,string)[] { ("*****","cos") });
    }
}