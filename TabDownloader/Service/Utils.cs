using HtmlAgilityPack;
using Spectre.Console;

namespace TabDownloader.Service;

public static class Utils
{
    public static string GetUrlFromUser()
    {
        var url = AnsiConsole.Prompt(
            new TextPrompt<string>("[purple]enter a[/] [green]link[/]")
                .PromptStyle("purple")
                .ValidationErrorMessage("[red]entered link is incorrect[/]")
                .Validate(url =>
                    Uri.IsWellFormedUriString(url, UriKind.Absolute)));

        return url;
    }
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