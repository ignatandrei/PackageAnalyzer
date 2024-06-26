﻿
    public static partial class MyAdditionalFiles
    {
        //https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/raw-string                
        public const string includeV1_gen_json =  """"""""""
{
  "version": 1,
  "parameters": "--include-transitive",
  "problems": [
    {
      "level": "warning",
      "text": "(A) : Auto-referenced package."
    }
  ],
  "projects": [
    {
      "path": "C:/gth/RSCG_Utils/src/RSCG_Utils/RSCG_Utils_Console/RSCG_Utils_Console.csproj",
      "frameworks": [
        {
          "framework": "net7.0"
        }
      ]
    },
    {
      "path": "C:/gth/RSCG_Utils/src/RSCG_Utils/RSCG_Utils/RSCG_Utils.csproj",
      "frameworks": [
        {
          "framework": "netstandard2.0",
          "topLevelPackages": [
            {
              "id": "Microsoft.CodeAnalysis.Analyzers",
              "requestedVersion": "3.3.4",
              "resolvedVersion": "3.3.4"
            },
            {
              "id": "Microsoft.CodeAnalysis.CSharp",
              "requestedVersion": "4.7.0",
              "resolvedVersion": "4.7.0"
            },
            {
              "id": "Microsoft.SourceLink.GitHub",
              "requestedVersion": "1.1.1",
              "resolvedVersion": "1.1.1"
            },
            {
              "id": "NETStandard.Library",
              "requestedVersion": "[2.0.3, )",
              "resolvedVersion": "2.0.3",
              "autoReferenced": "true"
            },
            {
              "id": "RazorBlade",
              "requestedVersion": "0.4.3",
              "resolvedVersion": "0.4.3"
            },
            {
              "id": "System.Text.Json",
              "requestedVersion": "7.0.3",
              "resolvedVersion": "7.0.3"
            }
          ],
          "transitivePackages": [
            {
              "id": "Microsoft.Bcl.AsyncInterfaces",
              "resolvedVersion": "7.0.0"
            },
            {
              "id": "Microsoft.Build.Tasks.Git",
              "resolvedVersion": "1.1.1"
            },
            {
              "id": "Microsoft.CodeAnalysis.Common",
              "resolvedVersion": "4.7.0"
            },
            {
              "id": "Microsoft.NETCore.Platforms",
              "resolvedVersion": "1.1.0"
            },
            {
              "id": "Microsoft.SourceLink.Common",
              "resolvedVersion": "1.1.1"
            },
            {
              "id": "System.Buffers",
              "resolvedVersion": "4.5.1"
            },
            {
              "id": "System.Collections.Immutable",
              "resolvedVersion": "7.0.0"
            },
            {
              "id": "System.Memory",
              "resolvedVersion": "4.5.5"
            },
            {
              "id": "System.Numerics.Vectors",
              "resolvedVersion": "4.5.0"
            },
            {
              "id": "System.Reflection.Metadata",
              "resolvedVersion": "7.0.0"
            },
            {
              "id": "System.Runtime.CompilerServices.Unsafe",
              "resolvedVersion": "6.0.0"
            },
            {
              "id": "System.Text.Encoding.CodePages",
              "resolvedVersion": "7.0.0"
            },
            {
              "id": "System.Text.Encodings.Web",
              "resolvedVersion": "7.0.0"
            },
            {
              "id": "System.Threading.Tasks.Extensions",
              "resolvedVersion": "4.5.4"
            }
          ]
        }
      ]
    }
  ]
}

"""""""""";
    }