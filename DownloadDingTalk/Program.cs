using DownloadDingTalk.HostedServices;
using DownloadDingTalk.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

class Program
{
    public static async Task Main(string[] args)
    {
        // 安装chromium浏览器
        Microsoft.Playwright.Program.Main(new[] { "install", "chromium" });

        await Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddTransient<ISpaceService, SpaceService>();
                services.AddTransient<IDentryService, DentryService>();
                services.AddHostedService<DownloadDingTalkSpaceHostService>();
            })
            .ConfigureHostConfiguration(hostConfig =>
            {
            })
            .ConfigureLogging(logConfig =>
            {
            })
            .Build()
            .RunAsync();
    }
}
