using Crontab.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CronTab.Net.Web.Pages;

public class CronList : PageModel
{
    public CrontabList CronTabList { get; set; }
    public async Task<IActionResult> OnGetAsync()
    {
        //CronTabWrapper wrapper = new CronTabWrapper();
        //var result = await wrapper.ListAsync(); 
        
        //CronTabList = await CrontabList.FromAsync(result.Output);

        //var cronlist  = new CrontabList();
        //cronlist.AddCronTab("* * * * *", "echo 111");

        //CronTabList = cronlist; 
        
        return Page();
    }
}