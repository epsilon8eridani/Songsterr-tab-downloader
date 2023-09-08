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

    public async Task DownloadTabs(string? searchUrl = null)
    {
        searchUrl ??= Utils.GetUrlFromUser();

        var tabUrls = await _parser.ParseSearchUrls(searchUrl);
        if (tabUrls != null)
        {
            for (var index = 0; index < tabUrls.Count; index++)
            {
                var tabUrl = tabUrls[index];
                await DownloadTab(tabUrl);
                await Task.Delay(500);
                AnsiConsole.MarkupLine($"[purple]{index + 1}/{tabUrls.Count}[/]");
            }
        }
    }

    public async Task DownloadTab(string? tabUrl = null)
    {
        tabUrl ??= Utils.GetUrlFromUser();

        var tab = await _parser.ParseTab(tabUrl);
        if (tab != null)
        {
            var folder = Path.Join("Tabs", tab.Artist);
            var downloadedPath = await tab.DownloadUrl
                .WithHeaders(_settings.Headers)
                .DownloadFileAsync(folder, tab.GetFileName());
            if (!string.IsNullOrEmpty(downloadedPath))
            {
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