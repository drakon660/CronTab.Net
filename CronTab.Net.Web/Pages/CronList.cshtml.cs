using System.Runtime.CompilerServices;
using Crontab.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CronTab.Net.Web.Pages;

public class CronList : PageModel
{
    public CronTabList? CronTabList { get; set; } = default;
    public async Task<IActionResult> OnGetAsync()
    {
        var cronlist  = new CronTabList();
        cronlist.AddCronTab("* * * * *", "echo 111");

        CronTabList = cronlist; 
        
        return Page();
    }
}