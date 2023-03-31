using System.Data;
using System.Text;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Primitives;

namespace CronTab.Net.Web;

[HtmlTargetElement("tabler-table")]
public class TablerTable : TagHelper
{
    private const string tableClass = "table card-table table-vcenter text-nowrap datatable";
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)  
    {  
        output.TagName = "table";
        output.Attributes.Add("class", tableClass);

        var child = await output.GetChildContentAsync();
        output.Content.SetHtmlContent(child);  
    }
}

[HtmlTargetElement("tabler-table-columns")]
public class TablerTableColumn : TagHelper
{
    [HtmlAttributeName("source")]
    public ICollection<string> Columns { get; set; }
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "thead";

        var sb = new StringBuilder();

        sb.Append("<tr>");
        foreach (var column in Columns)
        {
            sb.AppendFormat($"<th>{column}</th>");
        }
        sb.Append("</tr>");
        
        output.Content.SetHtmlContent(sb.ToString());  
    }
}
[HtmlTargetElement("tabler-table-rows")]
public class TablerTableRows : TagHelper
{
    // [HtmlAttributeName("for-rows")]
    // public IEnumerable<object> Values { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)  
    {  
        output.TagName = "tbody";

        //var sb = new StringBuilder();

        // sb.Append("<tr>");
        // foreach (var value in Values)
        // {
        //     sb.AppendFormat($"<td>{value}</td>");
        // }
        // sb.Append("</tr>");
        
        var child = await output.GetChildContentAsync();
        output.Content.SetHtmlContent(child);
    }
}

// [HtmlTargetElement("tabler-table-row")]
// public class TablerTableItem : TagHelper
// {
//     public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
//     {
//         output.TagName = "<tr>";
//         var sb = new StringBuilder();
//         
//         output.Content.SetHtmlContent(sb.ToString());
//     }
// }

[HtmlTargetElement("tabler-dropdown")]
public class TablerDropdown : TagHelper
{
    private const string Button = "<button class=\"btn dropdown-toggle align-text-top\" data-bs-boundary=" +
                                  "\"viewport\" data-bs-toggle=\"dropdown\">Actions</button>";

    [HtmlAttributeName("for-items")]
    public IEnumerable<string> Items { get; set; }
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "span";
        
        output.Attributes.Add("class", "dropdown");

        var sb = new StringBuilder();
        sb.AppendLine(Button);

        sb.AppendLine("<div class=\"dropdown-menu dropdown-menu-end\">");

        foreach (var item in Items)
        {
            sb.AppendFormat("<a class=\"dropdown-item\" href=\"#\">{0}</a>",item);
        }

        sb.AppendLine("</div>");

        output.Content.SetHtmlContent(sb.ToString());
    }
}
