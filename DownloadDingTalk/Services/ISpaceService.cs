namespace DownloadDingTalk.Services
{
    public interface ISpaceService
    {
        Task<IEnumerable<string>> GetAllSpacesAsync();
    }
}