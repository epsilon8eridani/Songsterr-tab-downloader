using Microsoft.Extensions.Hosting;
using Spectre.Console;
using TabDownloader.Service;

namespace TabDownloader;

public class ConsoleMenu
{
    private readonly Handler _handler;
    private readonly IHostApplicationLifetime _lifetime;

    public ConsoleMenu(Handler handler, IHostApplicationLifetime lifetime)
    {
        _handler = handler;
        _lifetime = lifetime;
    }
    public async Task Show()
    {
        while (!_lifetime.ApplicationStopping.IsCancellationRequested)
        {
            var prompt = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .AddChoices(
                        "download single tab from url",
                        "download multiple tabs from search url",
                        "exit"
                    ));
            try
            {
                switch (prompt)
                {
                    case "download single tab from url":
                        await _handler.DownloadTab();
                        break;
                    case "download multiple tabs from search url":
                        await _handler.DownloadTabs();
                        break;
                    case "exit":
                        _lifetime.StopApplication();
                        break;
                }
            }
            catch(Exception e)
            {
                AnsiConsole.WriteException(e);
            }
        }
    }
}