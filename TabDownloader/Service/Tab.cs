namespace TabDownloader.Service;

public record Tab(string RevisionId, string DownloadUrl, string Artist, string Title, string Extension)
{
    public string GetFileName()
    {
        var fileName = $"{Artist} - {Title} ({RevisionId}).{Extension}";
        foreach (var invalidChar in Path.GetInvalidFileNameChars())
        {
            fileName = fileName.Replace(invalidChar.ToString(), "");
        }
        return fileName;
    }
}

