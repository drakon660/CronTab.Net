using FluentAssertions;

namespace Crontab.Net.Tests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        var text = File.ReadAllText("cron.list");
        var list = await CronList.From(text);

        list.Items.Should().NotBeEmpty();
    }
}