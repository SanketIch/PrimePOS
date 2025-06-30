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
$assembly = [System.Reflection.Assembly]::LoadFrom($testDll.FullName)

# Build a map of method names to "IsCritical" flag using [Trait("Category", "Critical")]
$criticalTestMap = @{}
Write-Host "--- Populating criticalTestMap ---"
foreach ($type in $assembly.GetTypes()) {
    foreach ($method in $type.GetMethods()) {
        # Debugging: Show method name being processed for traits
        # Write-Host "Processing method: $($method.Name)"
        foreach ($attr in $method.GetCustomAttributes($true)) { # Use $true to include inherited attributes
            # Debugging: Show attribute type
            # Write-Host "  Found attribute: $($attr.GetType().FullName)"
            
            if ($attr.GetType().FullName -like "Xunit.TraitAttribute*") {
                $traitNameProp = $attr.GetType().GetProperty("TraitName")
                $traitValueProp = $attr.GetType().GetProperty("TraitValue")

                if ($traitNameProp -and $traitValueProp) {
                    $name = $traitNameProp.GetValue($attr, $null)
                    $value = $traitValueProp.GetValue($attr, $null)

                    # Debugging: Show trait name and value
                    # Write-Host "    Trait: Name='$name', Value='$value'"

                    if ($name -eq "Category" -and $value -eq "Critical") {
                        # Add to map using the exact method name from reflection
                        $criticalTestMap[$method.Name] = $true
                        Write-Host "✅ Marked Reflection Method: '$($method.Name)' (Length: $($method.Name.Length)) as critical."
                    }
                } else {
                    Write-Warning "  Xunit.TraitAttribute found, but TraitName or TraitValue properties not found. This is unexpected."
                }
            }
        }
    }
}
Write-Host "--- Finished populating criticalTestMap ---"
Write-Host "Critical Test Map Contents (MethodName -> IsCritical):"
$criticalTestMap.GetEnumerator() | ForEach-Object {
    Write-Host "  Key: '$($_.Key)' (Length: $($_.Key.Length)), Value: $($_.Value)"
}
Write-Host "------------------------------------------"


# Prepare CSV output
$csvPath = "$resultsDir/TestResults.csv"
$testResults = @()
$criticalFailed = $false
$failedCriticalTests = @()

foreach ($unitTestResult in $xml.TestRun.Results.UnitTestResult) {
    $testName = $unitTestResult.testName
    # Extract just the method name from the full test name (e.g., "PrimePOSTestCases.UnitTest1.IntentionalFailureTest" -> "IntentionalFailureTest")
    $methodNameFromTestName = ($testName -split '\.')[-1]
    
    # Debugging: Show what is being looked up in the map
    Write-Host "Processing TRX Test Case: '$testName'"
    Write-Host "  Extracted method name for lookup: '$methodNameFromTestName' (Length: $($methodNameFromTestName.Length))"
    
    # Explicitly check for exact match and presence in map
    $isCritical = $false
    if ($criticalTestMap.ContainsKey($methodNameFromTestName)) {
        $isCritical = $criticalTestMap[$methodNameFromTestName]
        Write-Host "  MATCH FOUND! '$methodNameFromTestName' in map with value: $isCritical"
    } else {
        Write-Host "  NO MATCH: '$methodNameFromTestName' not found in criticalTestMap."
        # Detailed comparison for debugging string mismatch
        $criticalTestMap.Keys | ForEach-Object {
            if ($_.Length -eq $methodNameFromTestName.Length -and $_ -eq $methodNameFromTestName) {
                 Write-Host "    DEBUG: Found key '$_' with same length and exact character match as '$methodNameFromTestName', but ContainsKey still failed?"
            }
        }
    }

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
