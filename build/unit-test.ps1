$dir = Split-Path $PSScriptRoot -Parent
$files = get-childitem $Invocation.MyCommand.Path -recurse -Include *.Test.csproj

$files | % {
	$path = $_.FullName
	$name = $_.BaseName
	$testOutputPath = $dir+ "\\TestResults\\" + $name
	$coverletOutputPath = $testOutputPath + "\\"
	dotnet test $path `
				--configuration Release `
				--logger "trx;logfilename=testresult.trx" `
				--results-directory $testOutputPath `
				/p:CollectCoverage=true `
				/p:CoverletOutputFormat="json%2ccobertura" `
				/p:CoverletOutput=$coverletOutputPath  `
				/p:Exclude="[*.Test]*%2c[xunit.*]*%2c[*]System.*\"
}