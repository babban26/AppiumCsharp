param (
    [string]$xmlFilePath = “testResult.xml",
    [string]$jsonOutputPath = "Output.json"
)

# Load the XML content from the provided file path
[xml]$xmlContent = Get-Content -Path $xmlFilePath

# Initialize the JSON template
$jsonTemplate = @'
[
    {
        "uri": "",
        "name": "",
        "elements": [
            {
                "type": "scenario",
                "keyword": "Scenario",
                "name": "",
                "line": 1,
                "steps": [
                    {
                        "keyword": "",
                        "name": "",
                        "line": 5,
                        "match": {
                            "location": ""
                        },
                        "result": {
                            "status": "",
                            "duration": 0
                        }
                    }
                ]
            }
        ]
    }
]
'@

# Convert the JSON template to a PowerShell object
$jsonObject = $jsonTemplate | ConvertFrom-Json

# Set values based on the XML content
$jsonObject[0].uri = $xmlContent.test-run.test-suite.fullname
$jsonObject[0].name = $xmlContent.test-run.test-suite.test-suite.test-case.name
$jsonObject[0].elements[0].name = $xmlContent.test-run.test-suite.test-suite.test-case.name
$jsonObject[0].elements[0].steps[0].keyword = $xmlContent.test-run.test-suite.test-suite.test-case.output.split()[0]
$jsonObject[0].elements[0].steps[0].name = $xmlContent.test-run.test-suite.test-suite.test-case.output.split()[1]
$jsonObject[0].elements[0].steps[0].match.location = $xmlContent.test-run.test-suite.test-suite.test-case.output.split()[3]
$jsonObject[0].elements[0].steps[0].result.status = $xmlContent.test-run.test-suite.test-suite.test-case.result
$jsonObject[0].elements[0].steps[0].result.duration = $xmlContent.test-run.test-suite.test-suite.test-case.duration

# Convert the PowerShell object back to JSON
$jsonResult = $jsonObject | ConvertTo-Json -Depth 10

# Save the JSON result to the specified output path
$jsonResult | Set-Content -Path $jsonOutputPath

Write-Host "JSON conversion completed. Output saved to: $jsonOutputPath"
	
				
