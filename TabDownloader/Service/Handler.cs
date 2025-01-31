﻿using Flurl.Http;
using Spectre.Console;

namespace TabDownloader.Service;

public class Handler
{
    private readonly Parser _parser;
    private readonly AppSettings _settings;
    private readonly CookieJar _cookies;

    public Handler(Parser parser, AppSettings settings, CookieJar cookies)
    {
        _parser = parser;
        _settings = settings;
        _cookies = cookies;
    }

    public async Task OpenSite()
    {
        await _settings.SiteUrl
            .WithHeaders(_settings.Headers)
            .WithCookies(_cookies)
            .GetStringAsync();
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
        if (tab is not null)
        {
            var folder = Path.Join("Tabs", tab.Artist);
            var invalidChars = Path.GetInvalidFileNameChars();
            var safeFileName = new string(tab.GetFileName().Select(ch => invalidChars.Contains(ch) ? '_' : ch).ToArray());
            var downloadedPath = await tab.DownloadUrl
                .WithHeaders(_settings.Headers)
                .WithCookies(_cookies)
                .DownloadFileAsync(folder, safeFileName);
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