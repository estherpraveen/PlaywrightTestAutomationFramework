using Microsoft.Playwright;
using Reqnroll;
using Reqnroll.BoDi;
using System.Threading.Tasks;

[Binding]
public class JourneyResultsSteps
{
    private readonly IPage _page;
    private readonly JourneyResultsPage _journeyResultsPage;

    public JourneyResultsSteps(ScenarioContext scenarioContext)
    {
      _page = scenarioContext["page"] as IPage
            ?? throw new InvalidOperationException(
                "Playwright page was not initialized.");
        _journeyResultsPage = new JourneyResultsPage(_page);
    }

    [Then("Validate the result {string} for walking and {string} for cycling time")]
    public async Task ValidateWalkingAndCyclingTime(string expectedWalkingTime, string expectedCyclingTime)
    {
        string walkingTime = await _journeyResultsPage.FindWalkingTime();
        string cyclingTime = await _journeyResultsPage.FindCyclingTime();

        await _journeyResultsPage.ValidateWalkingAndCyclingTime(expectedWalkingTime, expectedCyclingTime);

    }

    [When("I click on edit preferences")]
    public async Task ClickEditPreferences()
    {
        await _journeyResultsPage.ClickEditPreferences();
    }

    [Then("more journey options are displayed")]
    public async Task MoreJourneyOptionsDisplayed()
    {
        await _journeyResultsPage.MoreJourneyOptionsDisplayed();
    }

    [When("I select least walking routes and update")]
    public async Task SelectLeastWalkingRoutesAndUpdate()
    {
        await _journeyResultsPage.SelectLeastWalkingRoutesAndUpdate();
    }

    [Then("Validate the journey time")]
    public async Task ValidateLeastWalkingJourneyTime()
    {
        await _journeyResultsPage.ValidateLeastWalkingJourneyTime();
    }

    [When("I click on View Details")]
    public async Task ClickViewDetailsButton()
    {
        await _journeyResultsPage.ClickViewDetailsButton();
        Console.WriteLine("Clicked on View Details button.");
    }

    [Then("Validate the access information {string} {string} {string} at a given {string}")]
    public async Task ValidateAccessInformationAtSpecificLocation(string value1, string value2, string value3, string location)
    {
        string[] expectedTitles = { value1, value2, value3 };
        await _journeyResultsPage.ValidateAccessInformationAtSpecificLocation(location, expectedTitles);
    }

    [Then("Validate the {string} message is displayed")]
    public async Task ValidateErrorMessage(string expectedErrorMessage)
    {
        await _journeyResultsPage.ValidateErrorMessage(expectedErrorMessage);
    }

    [Then("Validate the {string} message is displayed for empty start location")]
    public async Task ValidateEmptyStartLocationErrorMessage(string expectedErrorMessage)
    {
        await _journeyResultsPage.ValidateEmptyStartLocationErrorMessage(expectedErrorMessage);
    }

    [Then("Validate the {string} message is displayed for empty destination location")]
    public async Task ValidateEmptyDestinationLocationErrorMessage(string expectedErrorMessage)
    {
        await _journeyResultsPage.ValidateEmptyDestinationLocationErrorMessage(expectedErrorMessage);
    }   

    [Then("Validate the {string} and {string} messages are displayed for empty start and destination locations")]
    public async Task ValidateEmptyStartAndDestinationLocationErrorMessages(string expectedStartErrorMessage, string expectedDestinationErrorMessage)
    {
        await _journeyResultsPage.ValidateEmptyStartAndDestinationLocationErrorMessages(expectedStartErrorMessage, expectedDestinationErrorMessage);
    }

}


