using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TabDownloader;
using TabDownloader.Service;

await Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration(builder =>
    {
        builder.AddJsonFile("appsettings.json");
    })
    .ConfigureServices((context, services) =>
    {
        var settings = context.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>()!;
        services.AddSingleton(settings);
        services.AddSingleton<Parser>();
        var commandLineArgs = new CommandLineArgs { Args = Environment.GetCommandLineArgs() };
        services.AddSingleton(commandLineArgs);
        services.AddSingleton<Handler>();
        services.AddSingleton<ConsoleMenu>();
    })
    .UseConsoleLifetime()
    .Build()
    .Services
    .GetService<ConsoleMenu>()
    ?.Show()!;