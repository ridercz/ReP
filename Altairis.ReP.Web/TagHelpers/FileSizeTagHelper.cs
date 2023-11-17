using Microsoft.AspNetCore.Razor.TagHelpers;


namespace Altairis.ReP.Web.TagHelpers;

public class FileSizeTagHelper : TagHelper {
    private const long KiloByte = 1024;
    private const long MegaByte = 1024 * KiloByte;
    private const long GigaByte = 1024 * MegaByte;
    private const long TeraByte = 1024 * GigaByte;

    public long Value { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        output.TagName = "span";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("title", $"{this.Value:N0} B");
        output.Content.SetContent(FormatSize(this.Value));
        base.Process(context, output);
    }

    private static string FormatSize(long value) {
        if (value >= TeraByte) return $"{(decimal)value / TeraByte:N2} TB";
        if (value >= GigaByte) return $"{(decimal)value / GigaByte:N2} GB";
        if (value >= MegaByte) return $"{(decimal)value / MegaByte:N2} MB";
        if (value >= KiloByte) return $"{(decimal)value / KiloByte:N2} kB";
        return $"{value:N0} B";
    }
}
