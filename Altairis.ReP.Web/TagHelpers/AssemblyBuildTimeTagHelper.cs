using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Altairis.ReP.Web.TagHelpers;

public class AssemblyBuildTimeTagHelper : TagHelper {
    private static readonly DateTime buildTime = GetAssemblyBuildDate(Assembly.GetEntryAssembly());
    private static readonly string? buildComputer = GetAssemblyBuildComputer(Assembly.GetEntryAssembly());

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        output.TagName = "span";
        output.Content.SetContent($"{buildTime:yyyyMMdd.HHmmss}@{buildComputer}");
    }

    private static DateTime GetAssemblyBuildDate(Assembly? assembly) {
        if (assembly == null) return DateTime.MinValue;
        var attrs = assembly.GetCustomAttributes<AssemblyMetadataAttribute>();
        var dateString = attrs.FirstOrDefault(x => x.Key.Equals("BuildDate"))?.Value;
        var parseResult = DateTime.TryParseExact(dateString, "s", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var dt);
        return parseResult ? dt : DateTime.MinValue;
    }

    private static string? GetAssemblyBuildComputer(Assembly? assembly) {
        if (assembly == null) return null;
        var attrs = assembly.GetCustomAttributes<AssemblyMetadataAttribute>();
        return attrs.FirstOrDefault(x => x.Key.Equals("BuildComputer"))?.Value;
    }

}
