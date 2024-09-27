using NetPackageAnalyzerObjects;
using RSCG_WhatIAmDoing;
using RSCG_WhatIAmDoing_Common;
using System;
namespace WhatIAmDoingData;

//[InterceptStatic("System.Console.*")] // regex
//internal class InterceptorMethodStatic : InterceptorMethodStaticBase, IInterceptorMethodStatic
//{
//}

//[InterceptInstanceClass(typeof(GenerateFiles), ".*")] //regex
//[InterceptInstanceClass(typeof(ProcessOutput), ".*")] //regex
//[InterceptInstanceClass("NetPackageAnalyzerObjects.*", ".*")]
//[InterceptInstanceClass("NetPackageAnalyzerObjects.ProcessOutput", "Build")]
class InterceptorMethodInstanceClass : InterceptorMethodInstanceClassBase, IInterceptorMethodInstanceClass
{

}
