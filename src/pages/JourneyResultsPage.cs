using Microsoft.Playwright;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public class JourneyResultsPage
{
    // Locators
    private readonly IPage _page;

    private ILocator cyclingTime => _page.Locator("a.journey-box.cycling > div + div > div + div");

    private ILocator walkingTime => _page.Locator("a.journey-box.walking > div + div > div + div");

    private ILocator editPreferences => _page.Locator("div.edit-preferences > button:has-text('Edit preferences')");

    private ILocator moreJourneyOptions => _page.Locator("div#more-journey-options ");

    private ILocator leastWalkingRoutes => _page.Locator("label[for='JourneyPreference_2']");
    private ILocator showMeList => _page.Locator("li.show-me-list");

    private ILocator savePreferencesLabel => _page.Locator("label[for='SavePreferences']");
    private ILocator updateButton => _page.Locator("label[for='SavePreferences']")
    .Locator("xpath=../..")
    .Locator("xpath=following-sibling::*[1]/div")
    .Locator("input:nth-of-type(2)");

    private ILocator viewDetailsButton => _page.Locator("button.secondary-button.show-detailed-results.view-hide-details").Nth(0);

    private ILocator invalidLocationErrorMessage => _page.Locator("li.field-validation-error");

    private ILocator emptyStartLocationErrorMessage => _page.Locator("span#InputFrom-error");

    private ILocator emptyDestinationLocationErrorMessage => _page.Locator("span#InputTo-error");

    // Constructor
    public JourneyResultsPage(IPage page)
    {
        _page = page;
    }

    // Methods
    /*
    * Finds the walking time from the journey results page.
    * It retrieves the inner text of the walking time locator and returns it as a string.
    */
    public async Task<string> FindWalkingTime()
    {
        string walkingTimeText = await walkingTime.InnerTextAsync();
        return walkingTimeText;
    }


    //Finds the cycling time from the journey results page.

    public async Task<string> FindCyclingTime()
    {
        string cyclingTimeText = await cyclingTime.InnerTextAsync();
        return cyclingTimeText;
    }

    /*
    * Clicks on the "Edit Preferences" button on the journey results page.
    */
    public async Task ClickEditPreferences()
    {
        Console.WriteLine("Clicking on Edit Preferences button");
        await editPreferences.WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible,
        });
        await editPreferences.ClickAsync();
    }

    // Validates that the "More journey options" section is displayed on the journey results page.
    public async Task MoreJourneyOptionsDisplayed()
    {
        await moreJourneyOptions.WaitForAsync();
        await moreJourneyOptions.IsVisibleAsync();
        Console.WriteLine("More journey options are displayed");
    }

    /*
    * Validates the walking and cycling time on the journey results page against the expected values.
    * It retrieves the inner text of the walking and cycling time locators, trims any whitespace, and asserts that they are not null or empty.
    * Then it compares the actual walking and cycling time with the expected values provided as parameters.
    * and asserts that they match the expected values provided as parameters.
    */
    public async Task ValidateWalkingAndCyclingTime(String expectedWalkingTime, string expectedCyclingTime)
    {
        string walkingTimeText = await walkingTime.InnerTextAsync();
        string cyclingTimeText = await cyclingTime.InnerTextAsync();
        Console.WriteLine($"Walking time displayed: {walkingTimeText}, Cycling time displayed: {cyclingTimeText}");
        Assert.That(walkingTimeText, Is.Not.Null.And.Not.Empty);
        Assert.That(cyclingTimeText, Is.Not.Null.And.Not.Empty);
        Assert.That(walkingTimeText.Trim(), Is.EqualTo(expectedWalkingTime));
        Assert.That(cyclingTimeText.Trim(), Is.EqualTo(expectedCyclingTime));
        Console.WriteLine($"Walking time validated: {walkingTimeText.Trim()} matches expected: {expectedWalkingTime}");
        Console.WriteLine($"Cycling time validated: {cyclingTimeText.Trim()} matches expected: {expectedCyclingTime}");
    }

    /* 
    *This method selects the least walking routes option and updates the preferences.
    *It scrolls to the "Show Me" list, checks the "Least Walking Routes" checkbox, scrolls to the "Save Preferences" label,
    *and clicks the "Update" button to save the preferences.
     */

    public async Task SelectLeastWalkingRoutesAndUpdate()
    {
        await showMeList.ScrollIntoViewIfNeededAsync();
        await leastWalkingRoutes.CheckAsync();
        Console.WriteLine("Least Walking Routes option selected");
        await savePreferencesLabel.ScrollIntoViewIfNeededAsync();
        await updateButton.ClickAsync();
        Console.WriteLine("Preferences updated");
    }

    /* This method validates the journey time after selecting least walking routes.
    * It scrolls to the middle of the page, retrieves the journey time text, extracts the number of minutes using a regular expression,
    * and asserts that the journey time is greater than 0 minutes.
    */
    public async Task ValidateLeastWalkingJourneyTime()
    {
        await _page.EvaluateAsync(
            "window.scrollTo({ top: document.body.scrollHeight / 2, behavior: 'smooth' })"
        );
        string text = await _page
            .Locator("div.carousel-wrap > div > div > div > div > h3")
            .Nth(0)
            .InnerTextAsync();
        Console.WriteLine($"Journey time text: {text}");
        Match match = Regex.Match(text, @"(\d+)\s*mins");
        Assert.That(match.Success, Is.True, "Journey time in mins was not found");
        Console.WriteLine($"Extracted journey time in mins: {match.Groups[1].Value}");
        int minutes = int.Parse(match.Groups[1].Value);
        Assert.That(minutes, Is.GreaterThan(0));
        Console.WriteLine($"Journey time validated: {minutes} mins is greater than 0 mins");
    }

    public async Task ClickViewDetailsButton()
    {
        await viewDetailsButton.ClickAsync();
    }

    /*
    * Validates the access information for a specific location.
    * It locates the journey option by its name, retrieves the access information links at that location, and collects their "data-title" attributes.
    * It then compares the collected data titles with the expected titles provided as an array of strings.
    */
    public async Task ValidateAccessInformationAtSpecificLocation(string optionName, string[] expectedTitles)
    {
        var optionLocator = _page.Locator($"//span[contains(@class,'location-name') and normalize-space()='{optionName}']").First;
        var links = optionLocator.Locator("xpath=../../following-sibling::div[position() <= 2]//div[contains(@class,'access-information')]//a");

        var dataTitles = new List<string?>();

        for (int i = 0; i < await links.CountAsync(); i++)
        {
            dataTitles.Add(await links.Nth(i).GetAttributeAsync("data-title"));
        }

        Console.WriteLine($"Access information at {optionName}: " + string.Join(", ", dataTitles));
        for (int i = 0; i < dataTitles.Count; i++)
        {
            Assert.That(dataTitles[i], Is.EqualTo(expectedTitles[i]),
                $"data-title at index {i} does not match.");
        }
        Console.WriteLine($"Access information validated for {optionName}: " + string.Join(", ", dataTitles));
    }

    // Validates the error message displayed for an invalid location input.

    public async Task ValidateErrorMessage(string expectedErrorMessage)
    {

        await invalidLocationErrorMessage.WaitForAsync();
        string actualErrorMessage = await invalidLocationErrorMessage.InnerTextAsync();
        Console.WriteLine("Actual invalid location error message: " + actualErrorMessage);
        Assert.That(actualErrorMessage.Trim(), Is.EqualTo(expectedErrorMessage));
        Console.WriteLine($"Invalid location error message validated: {actualErrorMessage.Trim()} matches expected: {expectedErrorMessage}");
    }

    
    // Validates the error message displayed for an empty start location input.

    public async Task ValidateEmptyStartLocationErrorMessage(string expectedErrorMessage)
    {
        await emptyStartLocationErrorMessage.WaitForAsync();
        string actualErrorMessage = await emptyStartLocationErrorMessage.InnerTextAsync();
        Console.WriteLine("Actual empty start location error message: " + actualErrorMessage);

        Assert.That(actualErrorMessage.Trim(), Is.EqualTo(expectedErrorMessage));
        Console.WriteLine($"Empty start location error message validated: {actualErrorMessage.Trim()} matches expected: {expectedErrorMessage}");
    }

    // Validates the error message displayed for an empty destination location input.
    public async Task ValidateEmptyDestinationLocationErrorMessage(string expectedErrorMessage)
    {
        await emptyDestinationLocationErrorMessage.WaitForAsync();
        string actualErrorMessage = await emptyDestinationLocationErrorMessage.InnerTextAsync();
        Console.WriteLine("Actual empty destination location error message: " + actualErrorMessage);

        Assert.That(actualErrorMessage.Trim(), Is.EqualTo(expectedErrorMessage));
        Console.WriteLine($"Empty destination location error message validated: {actualErrorMessage.Trim()} matches expected: {expectedErrorMessage}");
    }

    // Validates the error messages displayed for both empty start and destination location inputs.
    public async Task ValidateEmptyStartAndDestinationLocationErrorMessages(string expectedStartErrorMessage, string expectedDestinationErrorMessage)
    {
        await emptyStartLocationErrorMessage.WaitForAsync();
        await emptyDestinationLocationErrorMessage.WaitForAsync();

        string actualStartErrorMessage = await emptyStartLocationErrorMessage.InnerTextAsync();
        string actualDestinationErrorMessage = await emptyDestinationLocationErrorMessage.InnerTextAsync();

        Console.WriteLine("Actual empty start location error message: " + actualStartErrorMessage);
        Console.WriteLine("Actual empty destination location error message: " + actualDestinationErrorMessage);

        Assert.That(actualStartErrorMessage.Trim(), Is.EqualTo(expectedStartErrorMessage));
        Assert.That(actualDestinationErrorMessage.Trim(), Is.EqualTo(expectedDestinationErrorMessage));
        Console.WriteLine($"Empty start location error message validated: {actualStartErrorMessage.Trim()} matches expected: {expectedStartErrorMessage}");
        Console.WriteLine($"Empty destination location error message validated: {actualDestinationErrorMessage.Trim()} matches expected: {expectedDestinationErrorMessage}");
    }

}