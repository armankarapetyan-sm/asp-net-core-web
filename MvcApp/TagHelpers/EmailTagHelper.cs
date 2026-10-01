using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MvcApp.TagHelpers;

[HtmlTargetElement("email")]
public class EmailTagHelper : TagHelper
{
    public string MailTo { get; set; } = "";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "a";
        output.Attributes.SetAttribute("href", "mailto:" + MailTo);
        output.Attributes.SetAttribute("class", "mail");
    }
}
