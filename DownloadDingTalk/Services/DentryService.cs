using DownloadDingTalk.DtoModels;
using DownloadDingTalk.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace DownloadDingTalk.Services
{
    public class DentryService : IDentryService
    {
        private readonly ILogger<DentryService> logger;

        SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public DentryService(ILogger<DentryService> logger)
        {
            this.logger = logger;
        }
        /// <summary>
        /// 知识库名称
        /// </summary>
        public string SpaceName { get; set; }

        /// <summary>
        /// 目录树状结构
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 重建目录
        /// </summary>
        public void RestoreMenu(DingTalkResponse<DentryData> dentry)
        {
            SpaceName = dentry.Data.SpaceProfile.Name;
        }

        /// <summary>
        /// 保持目录结构下载知识库的所有文档.
        /// </summary>
        /// <param name="spaceId">知识库Id.</param>
        /// <returns>任务.</returns>
        public async Task DownloadAllFilesAsync(string spaceId)
        {
            using var playwright = await Playwright.CreateAsync();
            logger.LogInformation("staring browser");
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
            });
            logger.LogInformation("stared browser");

            logger.LogInformation("restoring cookies");
            var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                StorageStatePath = "auth.json",
            });

            logger.LogInformation("restored cookies");

            logger.LogInformation("opening new page");
            var page = await context.NewPageAsync();
            logger.LogInformation("opened new page");

            // 待访问和已访问过的节点，value值表示父目录结构，防止重复下载
            var dentryIds = new ConcurrentDictionary<string, (bool isVisted, string parentDirectory)>();
            var downloadCompletionTCS = new TaskCompletionSource();
            // 监听请求事件
            page.RequestFinished += async (object? sender, IRequest e) =>
            {
                if (e.Url == $"https://alidocs.dingtalk.com/box/api/v1/dentry/list?spaceId={spaceId}&dentryId=&pageSize=500")
                {
                    logger.LogInformation("get root dentry info");

                    var response = await e.ResponseAsync();
                    var dentryDataText = await response.TextAsync();
                    var dentryData = JsonConvert.DeserializeObject<DingTalkResponse<DentryData>>(dentryDataText, Converter.Settings);

                    await semaphore.WaitAsync();
                    logger.LogInformation("one thread start downloading file");
                    await DownloadFileAndExpandChildren(dentryData, page, dentryIds, string.Empty);
                    logger.LogInformation("one thread end downloading file");
                    semaphore.Release();
                }

                string pattern = $@"https:\/\/alidocs\.dingtalk\.com\/box\/api\/v1\/dentry\/list\?spaceId={spaceId}&dentryId=([a-zA-Z0-9]{{16}})&pageSize=500";
                Match match = Regex.Match(e.Url, pattern);
                if (match.Success)
                {
                    logger.LogInformation("get sub dentry info");

                    var response = await e.ResponseAsync();
                    var dentryDataText = await response.TextAsync();
                    var dentryData = JsonConvert.DeserializeObject<DingTalkResponse<DentryData>>(dentryDataText, Converter.Settings);

                    var parentDirectory = string.Empty;

                    await semaphore.WaitAsync();
                    logger.LogInformation("one thread start downloading file");
                    if (dentryIds.TryGetValue(match.Groups[1].Value, out var tupleValue) && !tupleValue.isVisted)
                    {
                        dentryIds.AddOrUpdate(match.Groups[1].Value, (true, tupleValue.parentDirectory), (_, _) => (true, tupleValue.parentDirectory));
                        parentDirectory = tupleValue.parentDirectory;
                        await DownloadFileAndExpandChildren(dentryData, page, dentryIds, parentDirectory);
                    }
                    logger.LogInformation("one thread end downloading file");
                    semaphore.Release();
                }
            };

            await page.GotoAsync("https://alidocs.dingtalk.com/i/spaces/" + spaceId, new PageGotoOptions() { Timeout = 60000 });

            await downloadCompletionTCS.Task;

            await browser.CloseAsync();
            playwright.Dispose();
        }

        private async Task DownloadFileAndExpandChildren(DingTalkResponse<DentryData> dentryData, IPage page, ConcurrentDictionary<string, (bool, string)> dentryIds, string currentPath)
        {
            var spaceName = dentryData.Data.SpaceProfile.Name;
            var childrenDirectory = currentPath;
            if (string.IsNullOrEmpty(childrenDirectory))
            {
                childrenDirectory = Path.Combine(Directory.GetCurrentDirectory(), spaceName);
            }

            foreach (var item in dentryData.Data.Children)
            {
                // 如果是文件类型下载文件
                if (item.DentryType == DentryType.File)
                {
                    var fileWithoutExtensionName = Path.GetFileNameWithoutExtension(item.Name);
                    var fileExtension = Path.GetExtension(item.Name);
                    // 如果是excel则点击export-excel下载
                    if (fileExtension == ".axls")
                    {
                        var fileName = fileWithoutExtensionName + ".xlsx";
                        var fileFullPath = Path.Combine(childrenDirectory, fileName);
                        var fileExist = File.Exists(fileFullPath);
                        var isExist = Directory.Exists(childrenDirectory) && fileExist;
                        if (!isExist)
                        {
                            logger.LogInformation($"File: {fileFullPath} not exist start downloading file");

                            //await page.GetByRole(AriaRole.Button, new() { Name = fileWithoutExtensionName, Exact = true }).ClickAsync();
                            var btn = page.Locator($"div[data-rbd-draggable-id='{item.DentryId}']");
                            await ScrollToFindElement(page, btn, item.Name);
                            await btn.ClickAsync();

                            await page.FrameLocator("#wiki-new-sheet-iframe").GetByTestId("submenu-menubar-table").GetByText("Table").ClickAsync(new LocatorClickOptions() { Timeout = 20000 });
                            await page.FrameLocator("#wiki-new-sheet-iframe").GetByTestId("submenu-export-excel").Locator("div").Nth(1).ClickAsync(new LocatorClickOptions() { Timeout = 20000 });

                            try
                            {
                                var download = await page.RunAndWaitForDownloadAsync(async () =>
                                {
                                    await page.FrameLocator("#wiki-new-sheet-iframe").GetByText("Excel (.xlsx, table as a whole)").ClickAsync(new LocatorClickOptions() { Timeout = 20000 });
                                });
                                var filePath = Path.Combine(childrenDirectory, download.SuggestedFilename);
                                await download.SaveAsAsync(filePath);
                            }
                            catch (TimeoutException)
                            {
                                await File.WriteAllLinesAsync("FailDownloadFile.txt", new[] { item.Name });
                                logger.LogError($"Error: Download {item.Name} timeout!");
                                return;
                            }
                        }
                    }
                    // 如果是doc则点击export-doc下载
                    else if (fileExtension == ".adoc")
                    {
                        var fileName = fileWithoutExtensionName + ".docx";
                        var fileFullPath = Path.Combine(childrenDirectory, fileName);
                        var fileExist = File.Exists(fileFullPath);
                        var isExist = Directory.Exists(childrenDirectory) && fileExist;
                        if (!isExist)
                        {
                            logger.LogInformation($"File: {fileFullPath} not exist start downloading file");

                            //await page.GetByRole(AriaRole.Button, new() { Name = fileWithoutExtensionName, Exact = true }).ClickAsync();
                            var btn = page.Locator($"div[data-rbd-draggable-id='{item.DentryId}']");
                            await ScrollToFindElement(page, btn, item.Name);
                            await btn.ClickAsync();

                            try
                            {
                                await page.FrameLocator("#wiki-doc-iframe").Locator("div").Filter(new() { HasText = "ShareEditPresent" }).GetByTestId("doc-header-more-button").First.ClickAsync();

                                await page.FrameLocator("#wiki-doc-iframe").GetByText("Download As").HoverAsync();

                                var download = await page.RunAndWaitForDownloadAsync(async () =>
                                {
                                    await page.FrameLocator("#wiki-doc-iframe").GetByText("Word(.docx)").ClickAsync();
                                });

                                var filePath = Path.Combine(childrenDirectory, download.SuggestedFilename);
                                await download.SaveAsAsync(filePath);
                            }
                            catch (TimeoutException)
                            {
                                await File.WriteAllLinesAsync("FailDownloadFile.txt", new[] { item.Name });
                                logger.LogError($"Error: Download {item.Name} timeout!");
                                return;
                            }
                        }
                    }
                    else if (fileExtension == ".amind")
                    {
                        //await page.GetByRole(AriaRole.Button, new() { Name = fileWithoutExtensionName, Exact = true }).ClickAsync(new LocatorClickOptions() { Timeout = 20000 });

                        //await page.FrameLocator("#wiki-mind-iframe").GetByTestId("ldlg55l972_root").GetByTestId("more_button").Locator("svg").ClickAsync(new LocatorClickOptions() { Timeout = 20000 });

                        //var download = await page.RunAndWaitForDownloadAsync(async () =>
                        //{
                        //    await page.FrameLocator("#wiki-mind-iframe").GetByText("Download to picture").ClickAsync(new LocatorClickOptions() { Timeout = 20000 });
                        //});

                        //var filePath = Path.Combine(childrenDirectory, download.SuggestedFilename);
                        //await download.SaveAsAsync(filePath);
                    }
                    else if (fileExtension == ".able")
                    {
                        //await page.GetByRole(AriaRole.Button, new() { Name = fileWithoutExtensionName, Exact = true }).ClickAsync(new LocatorClickOptions() { Timeout = 20000 });

                        //var frame = page.Frames.First(f => f.Name == "wiki-notable-iframe");
                        //await frame.WaitForLoadStateAsync(LoadState.Load);

                        //var notifyDivChinese = page.FrameLocator("#wiki-notable-iframe").Locator("div").Filter(new() { HasText = "欢迎使用新版钉钉多维表这是一款可视化数据库表格，更适合做业务数据管理，如：项目管理、销售管理、内容管理、信息收集，快来试试吧" }).GetByRole(AriaRole.Button);//.ClickAsync();
                        //var isExistNotifyDivChinese = await notifyDivChinese.CountAsync();
                        //if (isExistNotifyDivChinese > 0)
                        //{
                        //    await notifyDivChinese.ClickAsync();
                        //}

                        //var notifyDivEnglish = page.FrameLocator("#wiki-notable-iframe").GetByRole(AriaRole.Img, new() { Name = "template-close", Exact = true }).Locator("svg");//.ClickAsync();
                        //var isExistNotifyDivEnglish = await notifyDivEnglish.CountAsync();
                        //if (isExistNotifyDivEnglish > 0)
                        //{
                        //    await notifyDivEnglish.ClickAsync(new LocatorClickOptions() { Timeout = 20000 });
                        //}

                        //var ts = await page.FrameLocator("#wiki-notable-iframe").Locator("div").ElementHandlesAsync();
                        //var tsl = ts.Where(r => r.InnerTextAsync().Result == "SmartTable").ToList();

                        //var t = page.FrameLocator("#wiki-notable-iframe").Locator("div").Filter(new() { HasText = "SmartTable" });
                        //var count = await t.CountAsync();
                        //await t.ClickAsync();

                        //await page.FrameLocator("#wiki-notable-iframe").Locator("div").Filter(new() { HasText = "SmartTable" }).ClickAsync(new LocatorClickOptions() { Timeout = 20000 });


                        //var download = await page.RunAndWaitForDownloadAsync(async () =>
                        //{
                        //    await page.FrameLocator("#wiki-notable-iframe").GetByText("Download as Excel (.xlsx)").ClickAsync(new LocatorClickOptions() { Timeout = 20000 });
                        //});

                        //var filePath = Path.Combine(childrenDirectory, download.SuggestedFilename);
                        //await download.SaveAsAsync(filePath);
                    }
                    else if (fileExtension == ".adraw")
                    {
                        //await page.GetByRole(AriaRole.Button, new() { Name = fileWithoutExtensionName, Exact = true }).ClickAsync(new LocatorClickOptions() { Timeout = 20000 });

                        //await page.FrameLocator("#wiki-draw-iframe").GetByTestId("ldlfw5mk2_root").GetByText("SmartTable").ClickAsync(new LocatorClickOptions() { Timeout = 20000 });

                        //var download = await page.RunAndWaitForDownloadAsync(async () =>
                        //{
                        //    await page.FrameLocator("#wiki-draw-iframe").GetByText("Download as picture").ClickAsync(new LocatorClickOptions() { Timeout = 20000 });
                        //});

                        //var filePath = Path.Combine(childrenDirectory, download.SuggestedFilename);
                        //await download.SaveAsAsync(filePath);
                    }
                    else if (fileExtension == ".dlink")
                    {

                    }
                    else
                    {
                        var btn = page.Locator($"div[data-rbd-draggable-id='{item.DentryId}']");
                        await ScrollToFindElement(page, btn, item.Name);
                        await btn.ClickAsync();

                        //await page.GetByRole(AriaRole.Button, new() { Name = item.Name, Exact = true }).ClickAsync(new LocatorClickOptions() { Timeout = 120000 });

                        var fileFullPath = Path.Combine(childrenDirectory, item.Name);
                        var fileExist = File.Exists(fileFullPath);
                        var isExist = Directory.Exists(childrenDirectory) && fileExist;
                        if (!isExist)
                        {
                            logger.LogInformation($"File: {fileFullPath} not exist start downloading file");

                            try
                            {
                                await Task.Delay(1000);
                                var frame = page.Frames.First(f => f.Name == "uni-preview");
                                await frame.WaitForLoadStateAsync(LoadState.Load);

                                var download = await page.RunAndWaitForDownloadAsync(async () =>
                                {
                                    await page.FrameLocator("#uni-preview").GetByTestId("doc-new-download").First.ClickAsync();
                                });

                                var filePath = Path.Combine(childrenDirectory, download.SuggestedFilename);
                                await download.SaveAsAsync(filePath);
                            }
                            catch (TimeoutException)
                            {
                                await File.WriteAllLinesAsync("FailDownloadFile.txt", new[] { item.Name });
                                logger.LogError($"Error: Download {item.Name} timeout!");
                                return;
                            }
                        }
                    }
                }
                else
                {
                }

                // 如果有子对象则展开子对象树
                if (item.HasChildren)
                {
                    if (!dentryIds.ContainsKey(item.DentryId))
                    {
                        var subChildrenDirectory = Path.Combine(childrenDirectory, item.Name);
                        dentryIds.TryAdd(item.DentryId, (false, subChildrenDirectory));
                    }
                    logger.LogInformation($"expand {item.Name} sub dentry");
                    var btn = page.Locator($"div[data-rbd-draggable-id='{item.DentryId}']").First;
                    await ScrollToFindElement(page, btn, item.Name);
                    await btn.ClickAsync();
                    logger.LogInformation($"sleep 500ms");
                    await Task.Delay(500);
                }
            }
        }

        private async Task ScrollToFindElement(IPage page, ILocator btn, string dentryName)
        {
            var tree = await page.Locator(".MAINSITE_CATALOG-node-tree-list").First.EvaluateHandleAsync("(node)=>node");

            while (!await btn.IsVisibleAsync())
            {
                var scrollTopJson = await tree.EvaluateAsync("node => node.scrollTop");
                var scrollHeightJson = await tree.EvaluateAsync("node => node.scrollHeight");
                var clientHeightJson = await tree.EvaluateAsync("node => node.clientHeight");
                var scrollTop = scrollTopJson?.GetDouble();
                var scrollHeight = scrollHeightJson?.GetDouble();
                var clientHeight = clientHeightJson?.GetDouble();

                if (scrollTop >= scrollHeight - clientHeight)
                {
                    logger.LogInformation($"scroller reach bottom, not found {dentryName}");

                    // 滚动条已到底
                    break;
                }
                else
                {
                    logger.LogInformation($"scroll down by 300");
                    await tree.EvaluateAsync($"el=>el.scrollBy(0,300)");
                    logger.LogInformation($"sleep 500ms");
                    await Task.Delay(500);
                }
            }

            if (!await btn.IsVisibleAsync())
            {
                logger.LogInformation($"scroller to bottom but not found element, scroll to top");

                await tree.EvaluateAsync($"el=>el.scroll(0,0)");
                logger.LogInformation($"sleep 500ms");
                await Task.Delay(500);
            }

            while (!await btn.IsVisibleAsync())
            {
                var scrollTopJson = await tree.EvaluateAsync("node => node.scrollTop");
                var scrollHeightJson = await tree.EvaluateAsync("node => node.scrollHeight");
                var clientHeightJson = await tree.EvaluateAsync("node => node.clientHeight");
                var scrollTop = scrollTopJson?.GetDouble();
                var scrollHeight = scrollHeightJson?.GetDouble();
                var clientHeight = clientHeightJson?.GetDouble();

                if (scrollTop >= scrollHeight - clientHeight)
                {
                    // 滚动条已到底
                    logger.LogInformation($"scroller from top reach bottom, not found {dentryName}");
                    break;
                }
                else
                {
                    logger.LogInformation($"scroll down from top by 300");
                    await tree.EvaluateAsync($"el=>el.scrollBy(0,300)");
                    logger.LogInformation($"sleep 1000ms");
                    await Task.Delay(1000);
                }
            }
        }
    }
}
