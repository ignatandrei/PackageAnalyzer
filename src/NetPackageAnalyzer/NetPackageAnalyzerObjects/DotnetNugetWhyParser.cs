using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NetPackageAnalyzerObjects
{
    public class DependencyNode
    {
        public string Name { get; set; } = string.Empty;
        public List<DependencyNode> Children { get; set; } = new();
    }

    public class NugetWhyProjectResult
    {
        public string ProjectName { get; set; } = string.Empty;
        public bool HasDependency { get; set; }
        public List<DependencyNode> DependencyTrees { get; set; } = new(); // only tree structure
    }

    public static class DotnetNugetWhyParser
    {
        public static List<NugetWhyProjectResult> Parse(string output)
        {
            var results = new List<NugetWhyProjectResult>();
            NugetWhyProjectResult? current = null;
            List<string>? currentGraph = null;
            List<DependencyNode> allTrees = new();
            var lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var projectRegex = new Regex(@"^Project '([^']+)' (does not have a dependency on|has the following dependency graph\(s\) for) '([^']+)'", RegexOptions.Compiled);
            foreach (var line in lines)
            {
                var trimmed = line.TrimEnd();
                var match = projectRegex.Match(trimmed.Trim());
                if (match.Success)
                {
                    if (current != null)
                    {
                        if (currentGraph != null && currentGraph.Count > 0)
                        {
                            var tree = BuildTreeFromGraph(currentGraph);
                            if (tree != null) allTrees.Add(tree);
                            currentGraph = null;
                        }
                        current.DependencyTrees = allTrees;
                        results.Add(current);
                    }
                    current = new NugetWhyProjectResult
                    {
                        ProjectName = match.Groups[1].Value,
                        HasDependency = match.Groups[2].Value.StartsWith("has the following")
                    };
                    currentGraph = null;
                    allTrees = new();
                    continue;
                }
                if (current == null) continue;
                if (!current.HasDependency) continue;
                // Start of a new dependency graph
                if (trimmed.StartsWith("["))
                {
                    if (currentGraph != null && currentGraph.Count > 0)
                    {
                        var tree = BuildTreeFromGraph(currentGraph);
                        if (tree != null) allTrees.Add(tree);
                    }
                    currentGraph = new List<string> { trimmed };
                }
                else if (currentGraph != null)
                {
                    currentGraph.Add(trimmed);
                }
            }
            if (current != null)
            {
                if (currentGraph != null && currentGraph.Count > 0)
                {
                    var tree = BuildTreeFromGraph(currentGraph);
                    if (tree != null) allTrees.Add(tree);
                }
                current.DependencyTrees = allTrees;
                results.Add(current);
            }
            return results;
        }

        // Parses a single dependency graph (list of lines) into a DependencyNode tree
        private static DependencyNode? BuildTreeFromGraph(List<string> graphLines)
        {
            if (graphLines == null || graphLines.Count == 0)
                return null;
            // The first line is the target framework, skip it if it matches [net...]
            int start = 0;
            if (graphLines[0].StartsWith("["))
                start = 1;
            var stack = new Stack<(int indent, DependencyNode node)>();
            DependencyNode? root = null;
            for (int i = start; i < graphLines.Count; i++)
            {
                var line = graphLines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;
                int indent = GetIndentLevel(line);
                string name = line.TrimStart(' ', '?', '?', '?', '?');
                if (string.IsNullOrWhiteSpace(name)) continue;
                var node = new DependencyNode { Name = name };
                while (stack.Count > 0 && stack.Peek().indent >= indent)
                    stack.Pop();
                if (stack.Count == 0)
                {
                    root = node;
                }
                else
                {
                    stack.Peek().node.Children.Add(node);
                }
                stack.Push((indent, node));
            }
            return root;
        }

        // Determines the indentation level based on leading tree characters and spaces
        private static int GetIndentLevel(string line)
        {
            int indent = 0;
            foreach (char c in line)
            {
                if (c == ' ')
                    indent++;
                else if (c == '?' || c == '?' || c == '?' || c == '?')
                    indent++;
                else
                    break;
            }
            return indent;
        }
    }
}
