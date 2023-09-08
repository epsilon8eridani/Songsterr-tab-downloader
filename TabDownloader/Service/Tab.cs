namespace TabDownloader.Service;

public record Tab(string Id, string DownloadUrl, string Artist, string Title, string Extension)
{
    public string GetFileName()
    {
        return $"{Artist} - {Title} ({Id}).{Extension}";
    }
}

