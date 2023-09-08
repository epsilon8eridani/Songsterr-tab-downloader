namespace TabDownloader.Service;

public record AppSettings
{
    public string SiteUrl { get; set; } = "https://www.songsterr.com";
    public Dictionary<string, string> Headers = new()
    {
        ["Accept"] = "*/*",
        ["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36",
        ["sec-ch-ua"] = "\"Chromium\";v=\"116\", \"Not)A;Brand\";v=\"24\", \"Google Chrome\";v=\"116\"",
        ["sec-ch-ua-mobile"] = "?0",
        ["sec-ch-ua-platform"] = "\"Windows\"",
        ["Sec-Fetch-Dest"] = "document",
        ["Sec-Fetch-Mode"] = "navigate",
        ["Sec-Fetch-Site"] = "same-origin",
        ["Sec-Fetch-User"] = "?1",
        ["Upgrade-Insecure-Requests"] = "1"
    };
}