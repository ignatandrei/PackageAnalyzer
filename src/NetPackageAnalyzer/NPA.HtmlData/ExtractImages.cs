using Microsoft.Playwright;

namespace NPA.HtmlData;

public class ExtractImages
{

    static ExtractImages()
    {
        var exitCode = Microsoft.Playwright.Program.Main(new[] { "install" });
        if (exitCode != 0)
        {
            throw new Exception($"Playwright exited with code {exitCode}");
        }

    }
    public ExtractImages(string htmlPath)
    {
        HtmlPath = htmlPath;
    }

    public string HtmlPath { get; private set; }

    public async Task<bool> GetImagesAsync()
    {
       
        var dir = Path.GetDirectoryName(HtmlPath)!;
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
        {
            Headless = true
        });
        var context = await browser.NewContextAsync(new()
        {
            
        });

        var page = await context.NewPageAsync();
        //await page.SetContentAsync(File.ReadAllText(HtmlPath));
        await page.GotoAsync(new Uri(HtmlPath).AbsoluteUri);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var titles = await page.Locator("//div[starts-with(@title,'image')]").AllAsync();
        var nr = titles.Count();
        Console.WriteLine($"Found {nr} images");
        for (var i = 0; i < nr; i++)
        {
            var title = titles[i];
            var name=await title.GetAttributeAsync("title");
            name=name.Replace("image", "").Trim();

            var buffer = await title.ScreenshotAsync();
            await File.WriteAllBytesAsync(Path.Combine(dir, $"{name}.png"), buffer);
        }
        await browser.CloseAsync();
        Console.WriteLine("Done in "+dir); 
        return true;
    }
}
