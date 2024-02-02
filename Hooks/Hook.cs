using System;
using TechTalk.SpecFlow;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Frais.Hooks
{
    [Binding]
    public class Hooks
    {
        private JiraClient _jiraClient;
        //string allureResultsPath = "**/bin/Debug/net8.0/allure-results"; // Replace with your Allure results directory
        //string zephyrScaleJsonPath = "**/target/zephyr-scale.json"; // Replace with the desired output path



        [AfterScenario]
        public void AfterScenario()
        {
            //Allure to Zephyr
            // Console.WriteLine("Inside After Scenario ###");
            // ExtractAllureAndConvertToZephyrScale(allureResultsPath, zephyrScaleJsonPath);


            // JIRA ticket
            _jiraClient = new JiraClient();
            var testContext = TestContext.CurrentContext;


            if (TestContext.CurrentContext.Result.Outcome == ResultState.Failure)
            {
                // Check if the failure reason contains a specific message (e.g., server error)
                string failureMessage = TestContext.CurrentContext.Result.Message;

                if (failureMessage.Contains("Server error"))
                {
                    // Your handling logic for server error
                    Console.WriteLine("Test failed due to a server error.");
                }
            }

            // Check if the test failed
            if (testContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                // Check if the failure reason contains a specific message (e.g., server error)
                string failureMessage = TestContext.CurrentContext.Result.Message;

                if (failureMessage.Contains("Server error"))
                {
                    Console.WriteLine("Test failed due to a server error.");
                }
                else
                {
                    var summary = $"Test failed: {testContext.Test.MethodName}";
                    var customfield_10083 = $"Acceptance criteria of the failure: {testContext.Result.Message}";
                    var description = $"Detailed description of the failure: {testContext.Result.Message}";

                    // Create Jira bug for the failed test
                    _jiraClient.CreateBug(summary, customfield_10083, description);
                }
            }
        }



        static void ExtractAllureAndConvertToZephyrScale(string allureResultsPath, string zephyrScaleJsonPath)
        {
            // Run the Allure command to export results to a JSON file
            string jsonContent = RunAllureExport(allureResultsPath);

            // Parse JSON content
            dynamic allureData = JObject.Parse(jsonContent);

            // Convert to Zephyr Scale format
            var zephyrScaleData = ConvertToZephyrScale(allureData);

            // Serialize to JSON
            string zephyrScaleJson = JsonConvert.SerializeObject(zephyrScaleData, Formatting.Indented);

            // Save the Zephyr Scale JSON to a file
            File.WriteAllText(zephyrScaleJsonPath, zephyrScaleJson);

            Console.WriteLine($"Zephyr Scale JSON conversion completed. Output saved to: {zephyrScaleJsonPath}");
        }

        static string RunAllureExport(string allureResultsPath)
        {
            // Run the Allure command to export results to a JSON file
            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = "allure";
                process.StartInfo.Arguments = $"export --output allure-results-json {allureResultsPath}";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                return process.StandardOutput.ReadToEnd();
            }
        }

        static dynamic ConvertToZephyrScale(dynamic allureData)
        {
            // Extract relevant information and convert to Zephyr Scale format
            var zephyrScaleData = new
            {
                uri = allureData[0]?.name ?? "",
                name = allureData[0]?.name ?? "",
                elements = new[]
                {
                new
                {
                    type = "scenario",
                    keyword = "Scenario",
                    name = allureData[0]?.name ?? "",
                    line = 1,
                    steps = new[]
                    {
                        new
                        {
                            keyword = allureData[0]?.steps[0]?.name?.Value ?? "",
                            name = allureData[0]?.steps[0]?.name?.Value ?? "",
                            line = 5,
                            match = new
                            {
                                location = allureData[0]?.steps[0]?.start ?? "",
                            },
                            result = new
                            {
                                status = allureData[0]?.status ?? "",
                                duration = allureData[0]?.stop - allureData[0]?.start ?? 0,
                            }
                        }
                    }
                }
            }
            };

            return zephyrScaleData;
        }
    }
}
