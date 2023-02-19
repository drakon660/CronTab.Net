using System.Data;
using System.Text;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.ObjectPool;

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

[HtmlTargetElement("tabler-head")]
public class TablerHead : TagHelper
{
    [HtmlAttributeName("for-columns")]
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
[HtmlTargetElement("tabler-body")]
public class TablerBody : TagHelper
{
    [HtmlAttributeName("for-rows")]
    public IEnumerable<object> Values { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "tbody";

        var sb = new StringBuilder();

        sb.Append("<tr>");
        foreach (var value in Values)
        {
            sb.AppendFormat($"<td>{value}</td>");
        }
        sb.Append("</tr>");
        
        output.Content.SetHtmlContent(sb.ToString());  
    }
}

[HtmlTargetElement("tabler-dropdown")]
public class TablerDropdown : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        
    }
}
