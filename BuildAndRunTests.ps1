param($Configuration="Release")#Default Build Config is Release

Clear-Host
Write-Host "Initializing..."
#MSBUILD and MSTEST Executables
$msbuild = "MSBuild.exe"
$mstest = "MSTest.exe"
#We assume that the script will be on Solution Directory!
$SolutionDir = (Get-Item -Path ".\" -Verbose).FullName
#We will Build to the BuildResults Folter
$formattedDate = Get-Date -format yyyy-MM-dd_HHmmss
$outputFolder = "$SolutionDir\BuildResults\$formattedDate\"
#MS Build 
$msBuildOptions = "/noconsolelogger /nologo /verbosity:quiet /fl /p:Configuration="+$Configuration
#We assume that Project Outputs are on bin/ folder
$releaseFolder = $SolutionDir + "\bin\$Configuration"

# if the output folder exists, delete it
if ([System.IO.Directory]::Exists($outputFolder))
{
 [System.IO.Directory]::Delete($outputFolder, 1)
}
#To avoid situations where the script is not on same folder as the solution, we just go there!
cd $SolutionDir

Write-Host "Cleaning Solution(Gft.FoodMenu.sln)..."
#First we Clean
$clean = $msbuild + " ""Gft.FoodMenu.sln"" $msBuildOptions /t:Clean"
Invoke-Expression $clean
if ($LastExitCode -ne 0) {
    throw "Error Cleaning. MSBUILD Failed with exit code $LastExitCode. Please refer to the msbuild.log for more information."
	Exit
}

Write-Host "Building Solution(Gft.FoodMenu.sln)[$Configuration]..."
#Then we Build!
$build = $msbuild + " ""Gft.FoodMenu.sln"" $msBuildOptions  /t:Build"
Invoke-Expression $build
if ($LastExitCode -ne 0) {
    throw "Error Building. MSBUILD Failed with exit code $LastExitCode. Please refer to the msbuild.log for more information."	
	Exit
}

Write-Host "Moving Build Results to the Output folder..."
# Then we mode built files to the output folder
[System.IO.Directory]::Move($releaseFolder, $outputFolder)
if ($LastExitCode -ne 0) {
    throw "Error Moving Build Results to Output Folder. Mode Operation Failed with exit code $LastExitCode."
	Exit
}

Write-Host "Running Unit Tests..."
#Then we should prepare to run the Unit Tests!
$testDLL=$outputFolder +"\Gft.FoodMenu.Tests.dll"
$msTestOptions = " /nologo /resultsfile:""$outputFolder\TestResults.trx"" /testcontainer:""$testDLL"""
Invoke-Expression "$mstest $msTestOptions"
if ($LastExitCode -ne 0) {
    throw "One or more Tests Failed. Please refer to the Test Results File($outputFolder\TestResults.trx) for more information."
	Exit
}
else
{
	Write-Host
	Write-Host
	Write-Host "WELL DONE! Build and tests were both SUCESSFULL! Output is now on '$outputFolder' folder" -foreground "green"
	Write-Host
}

