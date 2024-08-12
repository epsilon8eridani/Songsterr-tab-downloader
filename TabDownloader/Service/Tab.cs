namespace TabDownloader.Service;

public record Tab(string RevisionId, string DownloadUrl, string Artist, string Title, string Extension)
{
    public string GetFileName()
    {
        return $"{Artist} - {Title} ({RevisionId}).{Extension}";
    }
}

