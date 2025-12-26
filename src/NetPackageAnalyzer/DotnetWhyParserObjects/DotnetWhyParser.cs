namespace DotnetWhyParserObjects;

public class DotnetWhyParser
{
    private List<string> _lines = new();
    private int _index = 0;
    
    public DotnetWhyOutput Parse(string output)
    {
        var result = new DotnetWhyOutput();
        _lines = output.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();
        _index = 0;
        
        while (_index < _lines.Count)
        {
            if (_lines[_index].Contains("Project '") && _lines[_index].Contains("'"))
            {
                if (string.IsNullOrEmpty(result.TargetPackage))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(
                        _lines[_index],@"for '([^']+)'"
                    );
                    if (!match.Success)
                    {
                        // Try searching backwards from current position
                        for (int i = _index - 1; i >= 0; i--)
                        {
                            match = System.Text.RegularExpressions.Regex.Match(
                                _lines[i],
                                @"for '([^']+)'"
                            );
                            if (match.Success)
                            {
                                result.TargetPackage = match.Groups[1].Value;
                                break;
                            }
                        }
                    }
                    else
                    {
                        result.TargetPackage = match.Groups[1].Value;
                    }
                }
                var project = ParseProject(result.TargetPackage);
                if (project != null)
                {
                    result.Projects.Add(project);
                    
                    
                }
            }
            else
            {
                _index++;
            }
        }
        result.ConsolidateProjectAndPackages();
        return result;
    }
    
    private ProjectDependency? ParseProject(string targetPackage)
    {
        var projectLine = _lines[_index];
        
        var projectMatch = System.Text.RegularExpressions.Regex.Match(
            projectLine,
            @"Project '([^']+)'"
        );
        
        if (!projectMatch.Success)
        {
            _index++;
            return null;
        }
        
        var project = new ProjectDependency { ProjectName = projectMatch.Groups[1].Value };
        
        // Extract target package  
        var match = System.Text.RegularExpressions.Regex.Match(projectLine, @"for '([^']+)'");
        if (match.Success && string.IsNullOrEmpty(project.ProjectName))
        {
            // This won't work, need to extract from first project
        }
        
        if (projectLine.Contains("does not have a dependency"))
        {
            project.HasDependency = false;
            _index++;
            return project;
        }
        
        _index++;
        
        // Skip to next project, parsing content along the way
        while (_index < _lines.Count && !(_lines[_index].Contains("Project '") && _lines[_index].Contains("'")))
        {
            var currentLine = _lines[_index];

            // Check for framework line
            if (currentLine.TrimStart().StartsWith("[") && currentLine.TrimStart().EndsWith("]"))
            {
                var frameworkMatch = System.Text.RegularExpressions.Regex.Match(
                    currentLine,
                    @"\[([^\]]+)\]"
                );
                if (frameworkMatch.Success && string.IsNullOrEmpty(project.TargetFramework))
                {
                    project.TargetFramework = frameworkMatch.Groups[1].Value;
                }
                _index++;
                
                // Parse all root-level trees after this framework
                ParseFrameworkTrees(project,targetPackage);
            }
            else
            {
                _index++;
            }
        }
        
        return project;
    }
    
    private void ParseFrameworkTrees(ProjectDependency project,string targetPackage)
    {
        while (_index < _lines.Count)
        {
            var line = _lines[_index];
            
            // Stop at next project
            if (line.Contains("Project '") && line.Contains("'"))
                break;
            
            // Stop at next framework
            if (line.TrimStart().StartsWith("[") && line.TrimStart().EndsWith("]"))
                break;
            
            // Empty line - skip
            if (string.IsNullOrWhiteSpace(line))
            {
                _index++;
                continue;
            }
            if (line.Contains(targetPackage))
            {
                var node = ParseTree(targetPackage);
                if (node != null)
                {
                    project.DependencyGraphs.Add(node);
                    //_index = node.FindLineNumberRecursive() ;
                }
            }
            // Root-level tree node
            if (line.Contains("├─") || line.Contains("└─"))
            {
                var node = ParseTree(targetPackage);
                if (node != null)
                {
                    project.DependencyGraphs.Add(node);
                    //_index = node.FindLineNumberRecursive() ;
                }
            }
            _index++;
            
        }
    }
    
    private DependencyNode? ParseTree(string targetPackage)
    {
        var line = _lines[_index];

        if (line.Contains(targetPackage + " "))
        {
            var dn1 = ExtractPackageInfo(line);
            dn1!.LineNumberForPackage = _index;            
            return dn1;
        }
        
        //_index++;
        //line = _lines[_index];
        var node = ExtractPackageInfo(line);
        
        if (node == null)
        {
            _index++;
            node = ExtractPackageInfo(_lines[_index]);
            return node;
        }
        
        var nodeIndent = GetTreeCharPosition(line);
        _index++;
        
        // Parse children
        while (_index < _lines.Count)
        {
            var childLine = _lines[_index];
            
            if (string.IsNullOrWhiteSpace(childLine))
            {
                _index++;
                continue;
            }
            if (childLine.Contains(targetPackage))
            {
                var child = ParseTree(targetPackage);
                node.Dependency=child!;
                break;
            }
            // Stop if we hit a project
            if (childLine.Contains("Project '"))
                break;
            
            // Stop if we hit a framework
            if (childLine.TrimStart().StartsWith("[") && childLine.TrimStart().EndsWith("]"))
                break;
            
            var childIndent = GetTreeCharPosition(childLine);
            
            // If this is a tree node at same or shallower depth, we're done
            if ((childLine.Contains("├─") || childLine.Contains("└─")) && childIndent <= nodeIndent)
                break;
            
            // If this is a child node
            if ((childLine.Contains("├─") || childLine.Contains("└─")) && childIndent > nodeIndent)
            {
                var child = ParseTree(targetPackage);
                if (child != null)
                {
                    node.Dependency=child;
                    if (child.HasReachedPackage())
                        break;
                }
            }
            else
            {
                _index++;
            }
        }
        
        return node;
    }
    
    private DependencyNode? ExtractPackageInfo(string line)
    {
        var trimmed = line.Trim();
        trimmed = System.Text.RegularExpressions.Regex.Replace(trimmed, @"^[│├└\─\s]+", "");
        
        var match = System.Text.RegularExpressions.Regex.Match(
            trimmed,
            @"(.+?)\s+\(v([^)]+)\)"
        );
        
        if (!match.Success)
            return null;
        
        return new DependencyNode
        {
            Name = match.Groups[1].Value.Trim(),
            Version = match.Groups[2].Value.Trim()
        };
    }
    
    private int GetTreeCharPosition(string line)
    {
        // Find the position of ├─ or └─
        for (int i = 0; i < line.Length - 1; i++)
        {
            if ((line[i] == '├' && line[i + 1] == '─') ||
                (line[i] == '└' && line[i + 1] == '─'))
            {
                return i;
            }
        }
        return int.MaxValue;
    }
}
