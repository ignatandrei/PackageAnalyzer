using Spectre.Console;
using Spectre.Console.Rendering;

namespace NetPackageAnalyzerShowInConsole;

public class WriteToConsole(GenerateData generatorData)
{
    public void WriteMajorDiffers()
    {
        var grid = new Grid();
        
        // Add columns 
        grid.AddColumn();
        grid.AddColumn();
        grid.AddColumn();
        grid.AddRow(new Text[]{
    new Text("Nr", new Style(Color.Red, Color.Black)).LeftJustified(),
    new Text("Package", new Style(Color.Red, Color.Black)).LeftJustified(),
    new Text("Versions", new Style(Color.Green, Color.Black)).Centered(),
    //new Text("Header 3", new Style(Color.Blue, Color.Black)).RightJustified()
});
        var nrMajor = 0;
        //Console.WriteLine("Major differs:" + generatorData.MajorWithMoreVersions().Length);
        foreach (var kvp in generatorData.MajorWithMoreVersionsPackages())
        {
            nrMajor++;
            var item = kvp.Key;
            Grid grdVersion = new ();
            grdVersion.AddColumn();
            grdVersion.AddColumn();
            grdVersion.AddColumn();
            grdVersion.AddRow(new Text[]{
    new Text("Nr", new Style(Color.Red, Color.Black)).LeftJustified(),
    new Text("Version", new Style(Color.Green, Color.Black)).Centered(),
    new Text("Projects", new Style(Color.Blue, Color.Black)).RightJustified()
            });
            var i = 1;
            foreach (var version in kvp.Value.VersionsPerProject)
            {
                string project = version.Value
                    .Select(it=>it.NameCSproj())
                    .Aggregate((a, b) => a + ", " + b);
                grdVersion.AddRow(new IRenderable[]{
                    new Text(i++.ToString()).LeftJustified(),
                    new Text(version.Key).Centered(),
                    new Text(project).RightJustified()
                });
            }


            grid.AddRow(new IRenderable[]{
    new Text(nrMajor.ToString()).LeftJustified(),
    new Text(item).LeftJustified(),
    grdVersion,
    //new Text("Row 3").RightJustified()
            
            });

        }
        AnsiConsole.Write(grid);
    }
}
