using AventStack.ExtentReports;
using Reqnroll;
using Microsoft.Playwright;


[Binding]
public class Hooks
{
    private readonly ScenarioContext _scenarioContext;
    private IPlaywright? _playwright;
    private IBrowser? _browser;

    private ExtentTest? _scenarioTest;

    public Hooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeScenario]
    public async Task BeforeScenario()
    {
        Console.WriteLine($"Running scenario: {_scenarioContext.ScenarioInfo.Title}");
        _playwright = await Playwright.CreateAsync();

        _browser = await _playwright.Chromium.LaunchAsync(
            new BrowserTypeLaunchOptions
            {
                Headless = false
            });
        _scenarioTest = ExtentReportManager.Instance.CreateTest(
        _scenarioContext.ScenarioInfo.Title);

        var page = await _browser.NewPageAsync();

        page.SetDefaultTimeout(15_000);

        _scenarioContext["page"] = page;
    }

    [AfterScenario]
    public async Task AfterScenario()
    {
        try
        {
            if (_scenarioContext.TestError != null)
            {
                _scenarioTest?.Fail(_scenarioContext.TestError);
            }
            else
            {
                _scenarioTest?.Pass("Scenario passed");
            }

            ExtentReportManager.Flush();
        }
        finally
        {
            if (_browser != null)
                await _browser.CloseAsync();

            _playwright?.Dispose();
        }

    }
}