# Find the test DLL
$testDll = Get-ChildItem -Recurse -Filter "PrimePOSTestCases.dll" | Select-Object -First 1

if (-not $testDll) {
    Write-Error "❌ Test DLL not found!"
    exit 1
}

# Create the results directory
$resultsDir = "TestResults"
mkdir $resultsDir -Force

# Run all tests and output .trx
vstest.console.exe "$($testDll.FullName)" /Logger:trx /ResultsDirectory:$resultsDir

# Parse the .trx test result file
$trxPath = Get-ChildItem -Path $resultsDir -Filter *.trx | Select-Object -First 1
if (-not $trxPath) {
    Write-Error "❌ .trx file not found after test execution!"
    exit 1
}
$xml = [xml](Get-Content $trxPath.FullName)

# Load the test assembly to get trait attributes via reflection
# Ensure the Xunit assembly is loaded if needed. For attributes, GetCustomAttributes usually handles it.
$assembly = [System.Reflection.Assembly]::LoadFrom($testDll.FullName)

# Build a map of method names to "IsCritical" flag using [Trait("Category", "Critical")]
$criticalTestMap = @{}
foreach ($type in $assembly.GetTypes()) {
    foreach ($method in $type.GetMethods()) {
        foreach ($attr in $method.GetCustomAttributes($true)) { # Use $true to include inherited attributes
            # Check if the attribute is an Xunit.TraitAttribute
            if ($attr.GetType().FullName -like "Xunit.TraitAttribute*") {
                # Xunit.TraitAttribute uses "TraitName" and "TraitValue" properties
                $traitNameProp = $attr.GetType().GetProperty("TraitName")
                $traitValueProp = $attr.GetType().GetProperty("TraitValue")

                if ($traitNameProp -and $traitValueProp) {
                    $name = $traitNameProp.GetValue($attr, $null)
                    $value = $traitValueProp.GetValue($attr, $null)

                    if ($name -eq "Category" -and $value -eq "Critical") {
                        $criticalTestMap[$method.Name] = $true
                        # Optional: For debugging, confirm it's being marked
                        # Write-Host "DEBUG: Marked $($method.Name) as critical."
                    }
                }
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
    # The Xunit testName in the TRX file often includes the full method path (e.g., "Namespace.Class.MethodName").
    # We need to extract just the method name to match our $criticalTestMap.
    # The last part after the last dot is typically the method name.
    $methodNameFromTestName = ($testName -split '\.')[-1]
    
    $isCritical = $criticalTestMap.ContainsKey($methodNameFromTestName)

    Write-Host "Test Case: $testName"
    Write-Host "IsCritical (based on method name '$methodNameFromTestName'): $isCritical"

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
