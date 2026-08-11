using System;
using System.Text.RegularExpressions;

public static partial class HtmlHelper
{
    // ======= Code Regex của bạn =======
    private static readonly Regex ScriptStyleRegex = new(
        "<(script|style)[^>]*?>.*?</\\1>",
        RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase
    );
    private static readonly Regex CommentRegex = new("<!--.*?-->", RegexOptions.Compiled | RegexOptions.Singleline);
    private static readonly Regex LineBreakRegex = new(@"<(br|hr)\s*/?>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex BlockElementRegex = new(
        @"</?(?:p|div|h[1-6]|li|tr|td|th|blockquote|section|article|header|footer|nav|aside)\s*/?>",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );
    private static readonly Regex TagRegex = MyRegex();
    private static readonly Regex WhitespaceRegex = new(@"\s+", RegexOptions.Compiled);
    private static readonly Regex MultipleLineBreaksRegex = new(@"\n{3,}", RegexOptions.Compiled);

    public static string StripHtmlRegex(string html, bool preserveLineBreaks = true)
    {
        if (string.IsNullOrEmpty(html)) return string.Empty;

        html = ScriptStyleRegex.Replace(html, string.Empty);
        html = CommentRegex.Replace(html, string.Empty);

        if (preserveLineBreaks)
        {
            html = LineBreakRegex.Replace(html, "\n");
            html = BlockElementRegex.Replace(html, "\n");
        }

        html = TagRegex.Replace(html, string.Empty);
        html = System.Net.WebUtility.HtmlDecode(html);

        if (preserveLineBreaks)
        {
            var lines = html.Split('\n');
            html = string.Join("\n", Array.ConvertAll(lines, line => WhitespaceRegex.Replace(line.Trim(), " ")));
            html = MultipleLineBreaksRegex.Replace(html, "\n\n");
        }
        else
        {
            html = WhitespaceRegex.Replace(html, " ");
        }

        return html.Trim();
    }

    [GeneratedRegex("<[^>]+>", RegexOptions.Compiled)]
    private static partial Regex MyRegex();
}
