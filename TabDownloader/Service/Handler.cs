using Flurl.Http;
using Spectre.Console;

namespace TabDownloader.Service;

public class Handler
{
    private readonly Parser _parser;
    private readonly AppSettings _settings;

    public Handler(Parser parser, AppSettings settings)
    {
        _parser = parser;
        _settings = settings;
    }

    public async Task DownloadTab()
    {
        var tabUrl = AnsiConsole.Prompt(
            new TextPrompt<string>("[purple]enter a[/] [green]link[/]")
                .PromptStyle("purple")
                .ValidationErrorMessage("[red]entered link is incorrect[/]")
                .Validate(url =>
                    Uri.IsWellFormedUriString(url, UriKind.Absolute) && url.StartsWith(_settings.SiteUrl)));

        var tab = await _parser.ParseTab(tabUrl);
        if (tab != null)
        {
            var folder = Path.Join("Tabs", tab.Artist);
            var downloadedPath = await tab.DownloadUrl
                .WithHeaders(_settings.Headers)
                .DownloadFileAsync(folder, tab.GetFileName());
            if (!string.IsNullOrEmpty(downloadedPath))
            {
                AnsiConsole.MarkupLine("[green]successfully downloaded to:[/]");
                AnsiConsole.Write(new TextPath(downloadedPath)
                    .RootColor(Color.Yellow)
                    .SeparatorColor(Color.Yellow)
                    .StemColor(Color.Yellow)
                    .LeafColor(Color.Green));
            }
            else
            {
                AnsiConsole.WriteLine("[red]tab is not downloaded[/]");
            }
        }
        else
        {
            AnsiConsole.WriteLine("[red]tab is not downloaded[/]");
        }
    }
}