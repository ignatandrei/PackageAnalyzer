namespace NetPackageAnalyzerObjects;

public record InfoSolution(int nrProjects, 
    int nrPackages, int nrOutdated, int nrDeprecated, 
    long totalCommits,
    int nrTestProjects,
    int nrMajorDiffers)
{
    public void x()
    {
        
    }
}
