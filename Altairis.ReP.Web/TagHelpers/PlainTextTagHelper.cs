using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Altairis.ReP.Web.TagHelpers;
public class PlaintextTagHelper(HtmlEncoder htmlEncoder) : TagHelper {
    private const string LINK_PATTERN = @"((https?)+\:\/\/)[^\s]+";
    private const int MAX_PATH_LENGTH = 20;
    private readonly HtmlEncoder htmlEncoder = htmlEncoder ?? throw new ArgumentNullException(nameof(htmlEncoder));

    public string Text { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        if (context is null) throw new System.ArgumentNullException(nameof(context));
        if (output is null) throw new System.ArgumentNullException(nameof(output));

        output.TagName = "div";

        var lines = this.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        var sb = new StringBuilder();
        for (var i = 0; i < lines.Length; i++) {
            sb.Append(this.ProcessLine(lines[i]));
            if (i != lines.Length - 1) sb.AppendLine("<br />");
        }

        output.Content.SetHtmlContent(sb.ToString());
    }

    private string ProcessLine(string s) {
        if (string.IsNullOrWhiteSpace(s)) return string.Empty;
        s = this.htmlEncoder.Encode(s);
        s = Regex.Replace(s, LINK_PATTERN, CreateLink, RegexOptions.IgnoreCase);
        return s;
    }

    private static string CreateLink(Match m) {
        var href = m.Value.TrimEnd(',', '.', ')', ']', ';', ':');
        var endsWithPunctation = href != m.Value;
        var text = href;
        try {
            var uri = new Uri(text);
            text = uri.Host;
            if (uri.PathAndQuery.Length > 1) text += (uri.PathAndQuery.Length <= MAX_PATH_LENGTH ? uri.PathAndQuery : uri.PathAndQuery.Substring(0, MAX_PATH_LENGTH) + "&hellip;");
        } catch (Exception) { }
        var html = $"<a href=\"{href}\" target=\"_blank\" title=\"{href}\">{text}</a>";
        if (endsWithPunctation) html += m.Value[href.Length..];
        return html;
    }

}
