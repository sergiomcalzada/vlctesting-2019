$env:ASPNETCORE_ENVIRONMENT="testing"
$env:ConnectionStrings:DefaultConnection="Data Source=localhost,5001;Initial Catalog=Test;User ID=sa;
Password=Pass@word!"

$env:Redis:IsEnabled="True"
$env:Redis:Configuration="localhost:5000"

$dir = Split-Path $PSScriptRoot -Parent

Write-Host "Starting environment"
docker-compose -f $PSScriptRoot\docker-compose.yml up -d
Start-Sleep -Seconds 5

$files = get-childitem $Invocation.MyCommand.Path -recurse -Include *.WebTest.csproj

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

Write-Host "Stopping environment"
#docker-compose -f $PSScriptRoot\docker-compose.yml down