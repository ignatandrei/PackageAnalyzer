namespace NetPackageAnalyzeHistory;

public record History(int? nrCommits, DateTime? FirstCommit, DateTime? LastCommit)
{
    public static readonly History Empty = new(null, null, null);
    public TimeSpan? DiffCommits
    {
        get
        {

            if (LastCommit == null || FirstCommit == null)
                return null;

            return LastCommit- FirstCommit;
        }
    }
    public int? CommitsPerMonth
    {

        get
        {
            if (LastCommit == null || FirstCommit == null)
                return null;

            var diff = DiffCommits;
            if (diff == null)
                return null;
            var nrMonths = (int)diff.Value.TotalDays / 30;
            if (nrMonths == 0)
                nrMonths++;
            return (int)(nrCommits/ nrMonths);
        }
    }
}

public class HistoryPerYear : Dictionary<int, History>
{
    public HistoryPerYear(IDictionary<int, History> data):base(data)
    {
        
    }
    public History HistoryYear(int year)
    {
        if(!this.ContainsKey(year))
            return History.Empty;

        return this[year];
    }
    public History AllHistory()
    {
        var min = this.Min(it => it.Value.FirstCommit);
        var max = this.Max(it => it.Value.LastCommit);
        var nr = this.Sum(it => it.Value.nrCommits);
        return new History(nr, min, max);
    }
    public int MaxYear()
    {
        return this.Max(it => it.Key);
    }
    public int MinYear()
    {
        return this.Min(it => it.Key);
    }
}

public class FileFolderHistorySimple
{
    public readonly string nameFile;
    public HistoryPerYear? AllHistoryFile { get;private set; }
    public HistoryPerYear? AllHistoryFolder { get; private set; }

    public FileFolderHistorySimple(string nameFile)
    {
        this.nameFile = nameFile;
    }
    private HistoryPerYear NrCommits(string folder,string what)
    {
        ProcessStartInfo startInfo = new()
        {
            //TODO: make time sensitive comment
            //TODO: use where to find where git is
            FileName = "git.exe",
            WorkingDirectory = folder,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            //Arguments = "log --pretty=oneline -n 100000  -- " + what,
            Arguments= "log --date=iso-strict --pretty=\"%ad\" -- "+ what
        };

        // Create and start the process
        Process process = new Process
        {
            StartInfo = startInfo
        };
        process.Start();

        // Read the output
        string output = process.StandardOutput.ReadToEnd();
        string errorOutput = process.StandardError.ReadToEnd();

        // Wait for the process to exit
        process.WaitForExit();
        if (errorOutput.Length > 0)
        {
            throw new ArgumentException(errorOutput);
        }
        var nrLines = output.Split('\r', '\n') ?? [];
        nrLines = nrLines.Where(it => !string.IsNullOrWhiteSpace(it)).ToArray();
        Dictionary<DateTime,int> dates = new();
        foreach (var line in nrLines)
        {
            if(string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            if(line.Length < 10)
            {
                continue;
            }
            var str = line.Substring(0,10);
            if (DateTime.TryParseExact(str,"yyyy-MM-dd",null,System.Globalization.DateTimeStyles.None, out var date))
            {
                if(!dates.ContainsKey(date))
                    dates.Add(date, 0);
                dates[date] = dates[date]+1;
            }
            
        }
        DateTime? LastCommit= null, FirstCommit=null;
        if(dates.Count > 0)
        {
            LastCommit = dates.Max(it=>it.Key);
            FirstCommit= dates.Min(it=>it.Key);
        }
        var ret= dates
            .GroupBy(it => it.Key.Year)

            .ToDictionary(it=>it.Key
            ,it=>
            new History(it.Count(),
            it.Min(it=>it.Key),
            it.Max(it=>it.Key)
            )
            );

        return  new HistoryPerYear(ret);

    }
    public void Initialize(bool AddHistoryForFolder)
    {
        ArgumentNullException.ThrowIfNull(nameFile);
        var folder=Path.GetDirectoryName(nameFile);
        ArgumentNullException.ThrowIfNull(folder);
        AllHistoryFile = NrCommits(folder, nameFile);
        if(AddHistoryForFolder)
            AllHistoryFolder = NrCommits(folder, ".");
        
        //Console.WriteLine("done with " + nameFile + $"{LastCommitFolder != LastCommitFile}");
        
    }
}
