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
        var imagesDir = Path.Combine(dir, "images_"+ nameFile);
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
        await Task.Delay(5000);
        try
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }
        catch (Exception ex)
        {
            Console.WriteLine("error in wait for load state network idle "+ex.Message);
        }
        await page.EvaluateAsync("driverObj.destroy()");
        var titles = await page.Locator("//div[starts-with(@title,'image')]").AllAsync();
        var nr = titles.Count();
        //Console.WriteLine($"Found {nr} images");
        for (var i = 0; i < nr; i++)
        {
            var title = titles[i];
            var name = await title.GetAttributeAsync("title");
            if (string.IsNullOrWhiteSpace(name))
                continue;
            var newName = name.Replace("image", "").Trim();
            newName = newName.Replace(" ", "-");
            try
            {
                var buffer = await title.ScreenshotAsync();

                await File.WriteAllBytesAsync(Path.Combine(imagesDir, $"{newName}.png"), buffer);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error in screenshot {name} " + ex.Message);
            }
        }
        await browser.CloseAsync();
        //Console.WriteLine("Done in "+dir); 
        return true;
    }
}
