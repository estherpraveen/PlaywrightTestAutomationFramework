using Microsoft.Playwright;
using System.Threading.Tasks;

public class PlanAJourneyPage
{
    // Locators
    private readonly IPage _page;
    private ILocator acceptAllCookiesButton => _page.Locator("button#CybotCookiebotDialogBodyLevelButtonLevelOptinAllowAll");
    private ILocator cookieOverlay => _page.Locator("div#cb-cookieoverlay");
    private ILocator fromTextBox => _page.GetByPlaceholder("From");
    private ILocator toTextBox => _page.Locator("#InputTo");
    private ILocator planAJourneyButton => _page.Locator("#plan-journey-button");
    private ILocator LocationAutoComplete(string location) =>
        _page.GetByText(location, new() { Exact = true });

    // Constructor
    public PlanAJourneyPage(IPage page)
    {
        _page = page;
    }

    // Methods

    /*
     * Accepts all cookies by clicking the "Accept All Cookies" button and waits for the cookie overlay to disappear.
     * If the overlay does not disappear within the timeout, it reloads the page and waits again.
     * This is handled within the try-catch block to ensure that the test can proceed even if the overlay does not disappear as expected.
     */
    public async Task AcceptAllCookies()
    {
        await acceptAllCookiesButton.WaitForAsync(
                 new() { State = WaitForSelectorState.Visible }
            );
        await acceptAllCookiesButton.ClickAsync();
        Console.WriteLine("Clicked on Accept All Cookies button.");
        try
        {
            await cookieOverlay.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Hidden,
            });
        }
        catch (TimeoutException)
        {
            Console.WriteLine("Cookie overlay did not disappear within the timeout. Reloading the page and waiting again.");
            await _page.ReloadAsync();
            await cookieOverlay.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Hidden,
            });
            Console.WriteLine("Cookie overlay disappeared after reloading the page.");

        }
    }

    /*    
    * Enters the starting location in the "From" text box and selects the location from the auto-complete suggestions.
    * It first fills the text box with the first 3 characters of the location to trigger the auto-complete suggestions,
    * waits for the specific location to appear in the suggestions, and then clicks on it.
    */

    public async Task EnterStartLocationWithAutoComplete(string fromLocation)
    {   
        string partialLocation = fromLocation.Substring(0, 3); // Get the first 3 characters of the location
        await fromTextBox.FillAsync(partialLocation);
        Console.WriteLine($"Entered partial start location: {partialLocation}");
        await LocationAutoComplete(fromLocation).WaitForAsync();
        await LocationAutoComplete(fromLocation).ClickAsync();
        Console.WriteLine($"Selected start location: {fromLocation}");
    }

    
    // Enters the destination location in the "To" text box and selects the location from the auto-complete suggestions.
    
    public async Task EnterDestinationWithAutoComplete(string toLocation)
    {
        string partialLocation = toLocation.Substring(0, 3); // Get the first 3 characters of the location
        await toTextBox.FillAsync(partialLocation);
        Console.WriteLine($"Entered partial destination location: {partialLocation}");
        await LocationAutoComplete(toLocation).WaitForAsync();
        await LocationAutoComplete(toLocation).ClickAsync();
        Console.WriteLine($"Selected destination location: {toLocation}");
    }

    public async Task ClickPlanAJourneyButton()
    {
        await planAJourneyButton.ClickAsync();
        Console.WriteLine("Clicked on Plan a Journey button.");
    }

    
   // Enters the starting location in the "From" text box without using auto-complete suggestions.
    
    public async Task EnterStartLocationWithoutAutoComplete(string fromLocation)
    {
        Console.WriteLine($"Entering start location: {fromLocation}");
        await fromTextBox.FillAsync(fromLocation);
    }

    // Enters the destination location in the "To" text box without using auto-complete suggestions.
    public async Task EnterDestinationWithoutAutoComplete(string toLocation)
    {
        Console.WriteLine($"Entering destination location: {toLocation}");
        await toTextBox.FillAsync(toLocation);
    }
}