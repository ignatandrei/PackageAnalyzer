using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
#nullable enable
 
 namespace NS_GeneratedJson_includeV1_gen_json {


/// <summary>
/// generated with RSCG_UTILS 2023.914.2016
/// </summary>
public partial class Problems{

[System.Text.Json.Serialization.JsonPropertyName("level")]
public string? Level {get;set;}  
[System.Text.Json.Serialization.JsonPropertyName("text")]
public string? Text {get;set;}  
public static Problems? Deserialize(string? text){ if(text == null) return null; return System.Text.Json.JsonSerializer.Deserialize<Problems>(text); } 
}
/// <summary>
/// generated with RSCG_UTILS 2023.914.2016
/// </summary>
public partial class Frameworks{

[System.Text.Json.Serialization.JsonPropertyName("framework")]
public string? Framework {get;set;}  
[System.Text.Json.Serialization.JsonPropertyName("topLevelPackages")]
public IList<TopLevelPackages>? TopLevelPackages {get;set;}  
[System.Text.Json.Serialization.JsonPropertyName("transitivePackages")]
public IList<TransitivePackages>? TransitivePackages {get;set;}  
public static Frameworks? Deserialize(string? text){ if(text == null) return null; return System.Text.Json.JsonSerializer.Deserialize<Frameworks>(text); } 
}
/// <summary>
/// generated with RSCG_UTILS 2023.914.2016
/// </summary>
public partial class Projects{

[System.Text.Json.Serialization.JsonPropertyName("path")]
public string? Path {get;set;}  
[System.Text.Json.Serialization.JsonPropertyName("frameworks")]
public IList<Frameworks>? Frameworks {get;set;}  
public static Projects? Deserialize(string? text){ if(text == null) return null; return System.Text.Json.JsonSerializer.Deserialize<Projects>(text); } 
}
/// <summary>
/// generated with RSCG_UTILS 2023.914.2016
/// </summary>
public partial class TopLevelPackages{

[System.Text.Json.Serialization.JsonPropertyName("id")]
public string? Id {get;set;}  
[System.Text.Json.Serialization.JsonPropertyName("requestedVersion")]
public string? RequestedVersion {get;set;}  
[System.Text.Json.Serialization.JsonPropertyName("resolvedVersion")]
public string? ResolvedVersion {get;set;}  
[System.Text.Json.Serialization.JsonPropertyName("autoReferenced")]
public string? AutoReferenced {get;set;}  
public static TopLevelPackages? Deserialize(string? text){ if(text == null) return null; return System.Text.Json.JsonSerializer.Deserialize<TopLevelPackages>(text); } 
}
/// <summary>
/// generated with RSCG_UTILS 2023.914.2016
/// </summary>
public partial class TransitivePackages{

[System.Text.Json.Serialization.JsonPropertyName("id")]
public string? Id {get;set;}  
[System.Text.Json.Serialization.JsonPropertyName("resolvedVersion")]
public string? ResolvedVersion {get;set;}  
public static TransitivePackages? Deserialize(string? text){ if(text == null) return null; return System.Text.Json.JsonSerializer.Deserialize<TransitivePackages>(text); } 
}
/// <summary>
/// generated with RSCG_UTILS 2023.914.2016
/// </summary>
public partial class includeV1_gen_json{

[System.Text.Json.Serialization.JsonPropertyName("version")]
public int? Version {get;set;}  
[System.Text.Json.Serialization.JsonPropertyName("parameters")]
public string? Parameters {get;set;}  
[System.Text.Json.Serialization.JsonPropertyName("problems")]
public IList<Problems>? Problems {get;set;}  
[System.Text.Json.Serialization.JsonPropertyName("projects")]
public IList<Projects>? Projects {get;set;}  
public static includeV1_gen_json? Deserialize(string? text){ if(text == null) return null; return System.Text.Json.JsonSerializer.Deserialize<includeV1_gen_json>(text); } 
}//end class
} //endnamespace
#nullable restore