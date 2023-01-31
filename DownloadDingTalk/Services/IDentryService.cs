namespace DownloadDingTalk.Services
{
    public interface IDentryService
    {
        Task DownloadAllFilesAsync(string spaceId);
    }
}