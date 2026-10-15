
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System.IO;

public static class ExtentReportManager
{
    private static ExtentReports? _extent;
    private static ExtentSparkReporter? _sparkReporter;

    public static ExtentReports Instance
    {
        get
        {
            if (_extent == null)
            {
              string? projectDirectory = AppContext.BaseDirectory;

while (projectDirectory != null &&
       !File.Exists(Path.Combine(
           projectDirectory, "TflJourneyPlannerFramework.csproj")))
{
    projectDirectory = Directory.GetParent(projectDirectory)?.FullName;
}

if (projectDirectory == null)
{
    throw new DirectoryNotFoundException(
        "Could not find TflJourneyPlannerFramework.csproj.");
}

string reportDirectory = Path.Combine(
    projectDirectory,
    "TestResults");

Directory.CreateDirectory(reportDirectory);

string reportPath = Path.Combine(
    reportDirectory,
    "TestReport.html");

                _sparkReporter = new ExtentSparkReporter(reportPath);
                _sparkReporter.Config.DocumentTitle =
                    "Automation Test Report";
                _sparkReporter.Config.ReportName =
                    "Playwright + Reqnroll Test Results";

                _extent = new ExtentReports();
                _extent.AttachReporter(_sparkReporter);
                _extent.AddSystemInfo("Framework", "C# Playwright + Reqnroll");
                _extent.AddSystemInfo("Test Runner", "NUnit");
            }

            return _extent;
        }
    }

    public static void Flush()
    {
        _extent?.Flush();
    }
}
