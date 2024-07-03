namespace TabDownloader.Service;

public record AppSettings
{
    public string SiteUrl { get; set; } = "https://www.songsterr.com";
    public Dictionary<string, string> Headers = new()
    {
        ["Accept"] = "*/*",
        ["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36",
        ["sec-ch-ua"] = "\"Not/A)Brand\";v=\"8\", \"Chromium\";v=\"126\", \"Google Chrome\";v=\"126\"",
        ["sec-ch-ua-mobile"] = "?0",
        ["sec-ch-ua-platform"] = "\n\"Windows\"",
        ["Sec-Fetch-Dest"] = "empty",
        ["Sec-Fetch-Mode"] = "no-cors",
        ["Sec-Fetch-Site"] = "same-origin",
        ["Sec-Fetch-User"] = "?1",
        ["Upgrade-Insecure-Requests"] = "1"
    };
}