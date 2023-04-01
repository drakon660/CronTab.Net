using Crontab.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CronTab.Net.Web.Pages;

public class CronList : PageModel
{
    public CronTabList? CronTabList { get; set; }
    public string[] Columns = { "Cron", "Next Occurrence", "Task" };
    public async Task<IActionResult> OnGetAsync()
    {
        CronTabList = await CronTabList.FromDemo();
        
        return Page();
    }
}