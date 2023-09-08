using HtmlAgilityPack;

namespace TabDownloader.Service;

public static class Utils
{
    public static async Task<HtmlDocument?> GetHtmlDocument(this Task<string> html)
    {
        var content = await html;
        if (string.IsNullOrEmpty(content)) return null;
        var htmlDocument = new HtmlDocument
        {
            OptionFixNestedTags = true
        };
        htmlDocument.LoadHtml(content);
        return htmlDocument;
    }
}