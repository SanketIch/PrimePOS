# Execute_Test_Cases.ps1

# Find the test DLL
$testDll = Get-ChildItem -Recurse -Filter "PrimePOSTestCases.dll" | Select-Object -First 1

# Create the results directory
$resultsDir = "TestResults"
mkdir $resultsDir -Force

# Run the tests using vstest.console.exe
vstest.console.exe "$($testDll.FullName)" /Logger:trx /ResultsDirectory:$resultsDir

# Convert TRX to CSV
$trxFile = Get-ChildItem -Path $resultsDir -Filter *.trx | Select-Object -First 1
$xml = [xml](Get-Content $trxFile.FullName)
$testResults = $xml.TestRun.Results.UnitTestResult

$output = foreach ($test in $testResults) {
    [PSCustomObject]@{
        TestName = $test.testName
        Outcome  = $test.outcome
        Duration = $test.duration
    }
}

# Export to CSV
$output | Export-Csv -Path "$resultsDir\TestResults.csv" -NoTypeInformation
