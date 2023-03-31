// See https://aka.ms/new-console-template for more information
using Crontab.Net;

using System.Runtime.InteropServices;
using System.Text;
using CliWrap;
using NCrontab;

Console.WriteLine("Hello, World!234324");

Console.WriteLine(RuntimeInformation.RuntimeIdentifier);

CronTabWrapper wrapper = new CronTabWrapper();
var result = await wrapper.ListAsync(); 
//var result = await wrapper.WriteAsync("drakon_cron");
Console.WriteLine(result.ExitCode);
Console.WriteLine(result.Output);

var cronList = await CronTabList.FromAsync(result.Output);

//var items = cronList.Items;

//cronList.AddCronTab("* * * * *", "echo \"1\" > /Users/drakon660/Desktop/12.txt");
//* * * * * echo "1" > /Users/drakon660/Desktop/28.txt
//var cronOut = cronList.ToCronTab();
//var result1 = await wrapper.WriteTextAsync(cronOut);
//Console.WriteLine(result.Output);

CrontabSchedule crontabSchedule = CrontabSchedule.TryParse("sdsdsd");
var t = 0;

