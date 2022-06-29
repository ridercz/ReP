using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Altairis.ReP.Web.TagHelpers;
[HtmlTargetElement("modal-box", Attributes = "id,message")]
public class ModalBoxTagHelper : TagHelper {

    public string Message { get; set; }

    public string Icon { get; set; } = "fa-info-circle";

    public string TargetUrl { get; set; } = "#";

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        if (context is null) throw new System.ArgumentNullException(nameof(context));
        if (output is null) throw new System.ArgumentNullException(nameof(output));

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.AddClass("modal", HtmlEncoder.Default);
        output.Content.AppendLine();
        output.Content.AppendHtmlLine("<article>");
        output.Content.AppendHtmlLine($"<header><i class=\"fa {this.Icon}\"></i></header>");
        var messageLines = this.Message.Split('\r', '\n', StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in messageLines) {
            output.Content.AppendHtmlLine($"<p>{line}</p>");
        }
        output.Content.AppendHtmlLine($"<footer><a href=\"{this.TargetUrl}\" class=\"button\">{UI._OK}</a></footer>");
        output.Content.AppendHtmlLine("</article>");
    }
}

