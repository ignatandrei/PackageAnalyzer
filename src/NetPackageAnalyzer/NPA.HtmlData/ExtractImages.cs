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
        var nameFile= Path.GetFileNameWithoutExtension(HtmlPath);
        var imagesDir = Path.Combine(dir, "images");
        if (!Directory.Exists(imagesDir))
        {
            Directory.CreateDirectory(imagesDir);
        }
        imagesDir = Path.Combine(imagesDir, nameFile);
        if (!Directory.Exists(imagesDir))
        {
            Directory.CreateDirectory(imagesDir);
        }
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
        var resp= await page.GotoAsync(new Uri(HtmlPath).AbsoluteUri);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await page.EvaluateAsync("driverObj.destroy()");
        var titles = await page.Locator("//div[starts-with(@title,'image')]").AllAsync();
        var nr = titles.Count();
        //Console.WriteLine($"Found {nr} images");
        for (var i = 0; i < nr; i++)
        {
            var title = titles[i];
            var name=await title.GetAttributeAsync("title");
            if(string.IsNullOrWhiteSpace(name))
                continue;
            name =name.Replace("image", "").Trim();
            name = name.Replace(" ", "-");
            var buffer = await title.ScreenshotAsync();
            
            await File.WriteAllBytesAsync(Path.Combine(imagesDir, $"{name}.png"), buffer);
        }
        await browser.CloseAsync();
        //Console.WriteLine("Done in "+dir); 
        return true;
    }
}
