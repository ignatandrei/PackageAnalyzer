using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPackageAnalyzerDocusaurus;
internal partial class MyResource
{
    [EmbedResourceCSharp.FileEmbed("docusaurus.zip")]
    public static partial System.ReadOnlySpan<byte> GetDocusaurusZip();
}
