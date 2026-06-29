$csproj = "$PSScriptRoot\..\src\NuExt.Presentation.Wpf.csproj"
$Configuration = "Release"
$outDir = $PSScriptRoot

dotnet clean $csproj -c $Configuration
dotnet pack $csproj -c $Configuration -o $outDir