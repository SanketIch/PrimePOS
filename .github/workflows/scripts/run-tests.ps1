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

# Prepare CSV output
$csvPath = "$resultsDir/TestResults.csv"
$testResults = @()
$criticalFailed = $false
$failedCriticalTests = @()  # For logging failed critical tests

foreach ($unitTestResult in $xml.TestRun.Results.UnitTestResult) {
    $testId = $unitTestResult.testId
    $testDefinition = $xml.TestRun.TestDefinitions.UnitTest | Where-Object { $_.id -eq $testId }

    # Check if test has the "Critical" category
    $categories = @()
    if ($testDefinition.TestCategory.TestCategoryItem -is [System.Array]) {
        $categories = @($testDefinition.TestCategory.TestCategoryItem | ForEach-Object { $_.TestCategory })
    } elseif ($testDefinition.TestCategory.TestCategoryItem) {
        $categories = @($testDefinition.TestCategory.TestCategoryItem.TestCategory)
    }

    $isCritical = "Critical" -in $categories

    Write-Host "Test Case: $($unitTestResult.testName)"
    Write-Host "IsCritical: $isCritical"

    if ($unitTestResult.outcome -eq "Failed" -and $isCritical) {
        Write-Host "❌ Critical test failed: $($unitTestResult.testName)"
        $criticalFailed = $true
        $failedCriticalTests += $unitTestResult.testName
    }

    $testResults += [PSCustomObject]@{
        TestName   = $unitTestResult.testName
        Outcome    = $unitTestResult.outcome
        StartTime  = $unitTestResult.startTime
        EndTime    = $unitTestResult.endTime
        IsCritical = $isCritical
    }
}

# Export results to CSV
$testResults | Export-Csv -Path $csvPath -NoTypeInformation

# Fail the pipeline only if a critical test failed
if ($criticalFailed) {
    $failedTestNames = $failedCriticalTests -join ", "
    Write-Error "🚨 Critical test failure(s) detected: $failedTestNames. Failing workflow."
    exit 1
} else {
    Write-Host "✅ No critical test failures. Proceeding..."
}
