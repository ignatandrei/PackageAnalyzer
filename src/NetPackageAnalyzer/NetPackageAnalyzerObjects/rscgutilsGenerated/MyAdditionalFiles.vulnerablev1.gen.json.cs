﻿
    public static partial class MyAdditionalFiles
    {
        //https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/raw-string                
        public const string vulnerablev1_gen_json =  """"""""""
{
  "version": 1,
  "parameters": "--vulnerable",
  "sources": [
    "https://api.nuget.org/v3/index.json",
    "C:/Program Files (x86)/Microsoft SDKs/NuGetPackages/",
    "D:/MyPackages/.nuget/packages"
  ],
  "projects": [
    {
      "path": "D:/gth/TILT/src/backend/Net6/NetTilt/NetTilt.Auth/NetTilt.Auth.csproj",
      "frameworks": [
        {
          "framework": "net6.0",
          "topLevelPackages": [
            {
              "id": "System.IdentityModel.Tokens.Jwt",
              "requestedVersion": "6.22.0",
              "resolvedVersion": "6.22.0",
              "vulnerabilities": [
                {
                  "severity": "Moderate",
                  "advisoryurl": "https://github.com/advisories/GHSA-59j7-ghrg-fj52"
                }
              ]
            }
          ]
        }
      ]
    }
  ]
}

"""""""""";
    }