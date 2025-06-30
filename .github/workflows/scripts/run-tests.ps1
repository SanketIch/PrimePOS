# Find the test DLL
$testDll = Get-ChildItem -Recurse -Filter "PrimePOSTestCases.dll" | Select-Object -First 1

# Create the results directory
$resultsDir = "TestResults"
mkdir $resultsDir -Force

# Run all tests and output .trx
vstest.console.exe "$($testDll.FullName)" /Logger:trx /ResultsDirectory:$resultsDir

# Parse the .trx test result file
$trxPath = Get-ChildItem -Path $resultsDir -Filter *.trx | Select-Object -First 1
$xml = [xml](Get-Content $trxPath.FullName)

# Load the test assembly and detect [CriticalTest] methods via reflection
Add-Type -AssemblyName System.Reflection
$assembly = [System.Reflection.Assembly]::LoadFrom($testDll.FullName)
$criticalTestNames = @{}

foreach ($type in $assembly.GetTypes()) {
    foreach ($method in $type.GetMethods("Public, Instance, Static, DeclaredOnly")) {
        foreach ($attr in $method.GetCustomAttributes($false)) {
            if ($attr.GetType().Name -eq "CriticalTestAttribute") {
                $criticalTestNames[$method.Name] = $true
            }
        }
    }
}

# Prepare CSV output
$csvPath = "$resultsDir/TestResults.csv"
$testResults = @()
$criticalFailed = $false
$failedCriticalTests = @()

foreach ($unitTestResult in $xml.TestRun.Results.UnitTestResult) {
    $testName = $unitTestResult.testName
    # Extract method name (last part of full name)
    $methodName = $testName.Split('.')[-1]
    $isCritical = $criticalTestNames.ContainsKey($methodName)

    Write-Host "Test Case: $testName"
    Write-Host "IsCritical: $isCritical"

    if ($unitTestResult.outcome -eq "Failed" -and $isCritical) {
        Write-Host "❌ Critical test failed: $testName"
        $criticalFailed = $true
        $failedCriticalTests += $testName
    }

    $testResults += [PSCustomObject]@{
        TestName   = $testName
        Outcome    = $unitTestResult.outcome
        StartTime  = $unitTestResult.startTime
        EndTime    = $unitTestResult.endTime
        IsCritical = $isCritical
    }
}

# Export results to CSV
$testResults | Export-Csv -Path $csvPath -NoTypeInformation

# Fail the pipeline if any critical test failed
if ($criticalFailed) {
    $failedTestNames = $failedCriticalTests -join ", "
    Write-Error "🚨 Critical test failure(s) detected: $failedTestNames. Failing workflow."
    exit 1
} else {
    Write-Host "✅ No critical test failures. Proceeding..."
}
