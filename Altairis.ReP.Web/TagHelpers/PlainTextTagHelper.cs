using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Altairis.ReP.Web.TagHelpers;

public partial class PlaintextTagHelper(HtmlEncoder htmlEncoder) : TagHelper {
    private const string LINK_PATTERN = @"((https?)+\:\/\/)[^\s]+";
    private const int MAX_PATH_LENGTH = 20;
    private readonly HtmlEncoder htmlEncoder = htmlEncoder;

    public string Text { get; set; } = string.Empty;

    private static readonly string[] LineSeparators = ["\r\n", "\n"];

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(output);

        output.TagName = "div";

        var lines = this.Text.Split(LineSeparators, StringSplitOptions.None);
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
        s = LinkRegex().Replace(s, CreateLink);
        return s;
    }

    private static string CreateLink(Match m) {
        var href = m.Value.TrimEnd(',', '.', ')', ']', ';', ':');
        var endsWithPunctation = href != m.Value;
        var text = href;
        try {
            var uri = new Uri(text);
            text = uri.Host;
            if (uri.PathAndQuery.Length > 1) text += (uri.PathAndQuery.Length <= MAX_PATH_LENGTH ? uri.PathAndQuery : string.Concat(uri.PathAndQuery.AsSpan(0, MAX_PATH_LENGTH), "&hellip;"));
        } catch (Exception) { }
        var html = $"<a href=\"{href}\" target=\"_blank\" title=\"{href}\">{text}</a>";
        if (endsWithPunctation) html += m.Value[href.Length..];
        return html;
    }

    [GeneratedRegex(LINK_PATTERN, RegexOptions.IgnoreCase, "cs-CZ")]
    private static partial Regex LinkRegex();

}
