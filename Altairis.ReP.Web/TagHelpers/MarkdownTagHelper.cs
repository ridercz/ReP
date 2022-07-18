using Microsoft.AspNetCore.Razor.TagHelpers;
using Markdig;

namespace Altairis.ReP.Web.TagHelpers;

public class MarkdownTagHelper : TagHelper {

    public string Text { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        output.TagName = string.Empty;
        var pipeline = new MarkdownPipelineBuilder().DisableHtml().UseAdvancedExtensions().Build();
        var html = Markdown.ToHtml(this.Text, pipeline);
        output.Content.SetHtmlContent(html);
    }

}
