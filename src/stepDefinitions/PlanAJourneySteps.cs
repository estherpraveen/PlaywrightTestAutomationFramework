using Microsoft.Playwright;
using Reqnroll;
using Reqnroll.BoDi;
using System.Threading.Tasks;

[Binding]
public class PlanAJourneySteps
{
    private readonly IPage _page;
    private readonly PlanAJourneyPage _planAJourneyPage;

    public PlanAJourneySteps(ScenarioContext scenarioContext)
    {
      _page = scenarioContext["page"] as IPage
            ?? throw new InvalidOperationException(
                "Playwright page was not initialized.");
        _planAJourneyPage = new PlanAJourneyPage(_page);
    }

    [Given("I navigate to the Tfl journey planner widget")]
    public async Task NavigateToTflWebsite()
    {
        await _page.GotoAsync("https://tfl.gov.uk/plan-a-journey");
    }

    [When("I accept all cookies")]
    public async Task AcceptAllCookies()
    {
        await _planAJourneyPage.AcceptAllCookies();
    }

    [Then("I should land on the {string} page")]
    public async Task LandOnPlanAJourneyPage(string pageName)
    {
        var pageTitle = await _page.TitleAsync();
        Assert.That(pageTitle, Does.Contain(pageName));
        Console.WriteLine($"Landed on the {pageName} page with title: {pageTitle}");
    }

    [When("I enter the start location with autocomplete {string}")]
    public async Task EnterStartLocationWithAutoComplete(string fromLocation)
    {
        await _planAJourneyPage.EnterStartLocationWithAutoComplete(fromLocation);
    }

    [When("I enter the destination location with autocomplete {string}")]
    public async Task EnterDestinationLocationWithAutoComplete(string toLocation)
    {
        await _planAJourneyPage.EnterDestinationWithAutoComplete(toLocation);
    }

    [When("I click on Plan my journey button")]
    public async Task ClickPlanAJourneyButton()
    {
        await _planAJourneyPage.ClickPlanAJourneyButton();
    }   

    [Given("I have planned a journey from start location {string} to destination {string}")]
    public async Task PlanAJourney(string fromLocation, string toLocation)
    {
        await _planAJourneyPage.EnterStartLocationWithAutoComplete(fromLocation);
        await _planAJourneyPage.EnterDestinationWithAutoComplete(toLocation);
        await _planAJourneyPage.ClickPlanAJourneyButton();
    }

     [When("I enter the start location without autocomplete {string}")]
    public async Task EnterStartLocationWithoutAutoComplete(string fromLocation)
    {
        await _planAJourneyPage.EnterStartLocationWithoutAutoComplete(fromLocation);
    }

    [When("I enter the destination location without autocomplete {string}")]
    public async Task EnterDestinationLocationWithoutAutoComplete(string toLocation)
    {
        await _planAJourneyPage.EnterDestinationWithoutAutoComplete(toLocation);
    }


}


