using System.Text;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.ObjectPool;

namespace CronTab.Net.Web;

[HtmlTargetElement("tabler-grid")]
public class SuperTagHelper : TagHelper
{
    [HtmlAttributeName("for-customername")]  
    public ModelExpression CustomerName { get; set; }  
    [HtmlAttributeName("for-city")]  
    public ModelExpression City { get; set; }  
    public override void Process(TagHelperContext context, TagHelperOutput output)  
    {  
        output.TagName = "CustomerDetails";  
        output.TagMode = TagMode.StartTagAndEndTag;

        var sb = new StringBuilder();

        sb.AppendFormat("Customer Name: {0}", this.CustomerName.Model);  
        sb.AppendFormat("City: {0}", this.City.Model);  
  
        output.PreContent.SetHtmlContent(sb.ToString());  
    }  
}  
