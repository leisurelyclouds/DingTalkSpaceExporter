using DownloadDingTalk.Services;
using Microsoft.Extensions.Hosting;

namespace DownloadDingTalk.HostedServices
{
    public class DownloadDingTalkSpaceHostService : IHostedService
    {
        private readonly ISpaceService spaceService;
        private readonly IDentryService dentryService;

        public DownloadDingTalkSpaceHostService(ISpaceService spaceService, IDentryService dentryService)
        {
            this.spaceService = spaceService;
            this.dentryService = dentryService;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //await cookieService.LoginAndSaveCookie(CookieService.DefaultCookieFileName);

            var spaceIds = await spaceService.GetAllSpacesAsync();

            foreach (var spaceId in spaceIds)
            {
                await dentryService.DownloadAllFilesAsync(spaceId);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
