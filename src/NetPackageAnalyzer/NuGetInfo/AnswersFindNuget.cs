namespace NuGetInfo;
//public partial class StringOrNumber : OneOfBase<string, int>
//{
//    public (bool isNumber, int number) TryGetNumber() =>
//           Match( //this match function is auto generated
//               s => (int.TryParse(s, out var n), n),
//               i => (true, i)
//           );
//}
[UnionType(typeof(FileNotFound))]
[UnionType(typeof(FolderNotFound))]
[UnionType(typeof(NoLicenseFound))]
[UnionType(typeof(LicenseFound))]
public partial class AnswersFindNuget
{

}

public record FileNotFound(string fileNotFound);
public record FolderNotFound(string folderNotFound);
public record NoLicenseFound();
public record LicenseFound(string license);
