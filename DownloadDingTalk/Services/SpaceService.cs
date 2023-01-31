using DownloadDingTalk.DtoModels;
using DownloadDingTalk.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using Newtonsoft.Json;

namespace DownloadDingTalk.Services
{
    public class SpaceService : ISpaceService
    {
        private readonly ILogger<SpaceService> logger;

        public SpaceService(ILogger<SpaceService> logger)
        {
            this.logger = logger;
        }

        public string SpaceUrl { get; } = "https://alidocs.dingtalk.com/i/desktop/spaces";

        public async Task<IEnumerable<string>> GetAllSpacesAsync()
        {
            if (File.Exists("spaces.json"))
            {
                var readSpacesJsonData = await File.ReadAllTextAsync("spaces.json");
                var spacesFromFile = JsonConvert.DeserializeObject<IEnumerable<string>>(readSpacesJsonData);
                if (spacesFromFile != null && spacesFromFile.Any())
                {
                    return spacesFromFile;
                }
            }

            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
            });
            var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                StorageStatePath = "auth.json",
            });

            var page = await context.NewPageAsync();

            var tcs = new TaskCompletionSource<DingTalkResponse<SpaceData>>();
            // 监听请求事件
            page.RequestFinished += async (object? sender, IRequest e) =>
            {
                if (e.Url == "https://alidocs.dingtalk.com/box/api/v2/mine/spaces?sortBy=1&pageSize=20&createdBy=0&scene=0")
                {
                    var response = await e.ResponseAsync();
                    var spaceDataText = await response.TextAsync();
                    var spaceData = JsonConvert.DeserializeObject<DingTalkResponse<SpaceData>>(spaceDataText, Converter.Settings);
                    //var spaceData = await response.JsonAsync<DingTalkResponse<SpaceData>>();
                    tcs.SetResult(spaceData);
                }
            };

            await page.GotoAsync(SpaceUrl);

            var actualUrl = await page.EvaluateAsync<string>("() => window.location.href");

            if (actualUrl.StartsWith("https://login.dingtalk.com/oauth2/challenge.htm"))
            {
                logger.LogWarning("登录完成以后，按回车键继续");
                Console.ReadLine();

                await context.StorageStateAsync(new BrowserContextStorageStateOptions
                {
                    Path = "auth.json"
                });
            }

            var spaceData = await tcs.Task;
            var spaceDataResult = spaceData.Data.Related.List.Select(r => r.Id);
            var spaceJsonData = JsonConvert.SerializeObject(spaceDataResult);
            await File.WriteAllTextAsync("spaces.json", spaceJsonData);
            return spaceDataResult;
        }
    }
}
