using System.Xml;
using Flurl.Http;
using Newtonsoft.Json.Linq;

namespace TabDownloader.Service;

public class Parser
{
    private readonly AppSettings _settings;
    private readonly CookieJar _cookie;

    public Parser(AppSettings settings, CookieJar cookie)
    {
        _settings = settings;
        _cookie = cookie;
    }

    public async Task<List<string>?> ParseSearchUrls(string url)
    {
        if (string.IsNullOrEmpty(url)) return null;
        if (!url.StartsWith(_settings.SiteUrl)) return null;
        
        var doc = await url
            .WithHeaders(_settings.Headers)
            .WithCookies(_cookie)
            .GetStringAsync()
            .GetHtmlDocument();

        var tabs = doc?.DocumentNode.SelectNodes("//div[@id='search-wrap']//div[@*='songs']/a");
        if (tabs == null) return null;

        var result = new List<string>();
        
        foreach (var tabNode in tabs)
        {
            var tabUrl = _settings.SiteUrl + tabNode.Attributes?["href"]?.Value;
            if (tabUrl != _settings.SiteUrl)
            {
                result.Add(tabUrl);
            }
        }


        return result;
    }
    public async Task<Tab?> ParseTab(string url)
    {
        if (string.IsNullOrEmpty(url)) return null;
        if (!url.StartsWith(_settings.SiteUrl)) return null;

        var doc = await url
            .WithHeaders(_settings.Headers)
            .WithCookies(_cookie)
            .GetStringAsync()
            .GetHtmlDocument();

        if (doc == null) return null;

        var script = doc.DocumentNode.SelectSingleNode("//script[@id='state']").InnerHtml;
        var json = JObject.Parse(script);
        var selectToken = json.SelectToken("meta.current");
        var revisionId = selectToken?["revisionId"]?.ToString();
        var artist = selectToken?["artist"]?.ToString();
        var title = selectToken?["title"]?.ToString();
        
        var xmlUrl = $"{_settings.SiteUrl}/a/ra/player/songrevision/{revisionId}.xml"; // TODO
        var xml = await xmlUrl
            .WithHeaders(_settings.Headers)
            .WithCookies(_cookie)
            .GetStringAsync();
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml);
        var tabId = xmlDoc.SelectSingleNode("/SongRevision/tab/guitarProTab")?.Attributes?["id"]?.InnerText;
        var tabUrl = xmlDoc.SelectSingleNode("/SongRevision/tab/guitarProTab/attachmentUrl")?.InnerText;
        var tabExt = tabUrl?.Split('.').Last();

        if (string.IsNullOrEmpty(artist) || string.IsNullOrEmpty(title) ||
            string.IsNullOrEmpty(tabId) || string.IsNullOrEmpty(tabUrl) ||
            string.IsNullOrEmpty(tabExt)) return null;

        return new Tab(tabId, tabUrl, artist, title, tabExt);
    }
}