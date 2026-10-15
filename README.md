# C# Playwright Test Automation Framework

## Overview

This project is a UI test automation framework built using **C#, Playwright for .NET, Reqnroll, and Gherkin**.

It follows a Behaviour-Driven Development (BDD) approach, allowing test scenarios to be written in a readable Given–When–Then format. The framework separates test scenarios, step definitions, and page interactions to improve readability, maintainability, and reusability.

## Technology Stack

- **C#** – Programming language
- **.NET 8** – Runtime and development platform
- **Microsoft Playwright** – Browser automation
- **Reqnroll** – BDD framework for Gherkin scenarios
- **NUnit** – Test assertions
- **Visual Studio Code** – Development environment

### Components

- **Features:** Contains Gherkin scenarios describing the expected application behaviour.
- **Step Definitions:** Maps Gherkin steps to C# methods that execute the tests.
- **Page Objects:** Encapsulates page locators and browser interactions to promote reuse and maintainability.
- **Hooks:** Initialises and closes the Playwright browser and manages the page lifecycle for each scenario.
- **Project file:** Defines the .NET project configuration and dependencies.

## Prerequisites

Before running the tests, install:

1. [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
2. [Visual Studio Code](https://code.visualstudio.com/)
3. The Playwright Chromium browser, using the installation command provided in the setup instructions below.
4. The C# and C# Dev Kit extensions for Visual Studio Code (recommended).

## Setup Instructions

### 1. Clone the repository

```bash
git clone <https://github.com/estherpraveen/PlaywrightTestAutomationFramework>
cd <TflJourneyPlannerFramework>
```

### 2. Restore dependencies

Open a terminal in the directory containing the `.csproj` file and run:

```bash
dotnet restore
```

### 3. Build the project

```bash
dotnet build
```

### 4. First-Time Setup: Install Playwright Chromium

Run the following command to install the Chromium browser required by Playwright:

pwsh bin/Debug/net8.0/playwright.ps1 install chromium

This step is required on a new machine if the required Playwright browser is not already installed.
## Running the Tests

Run all automated tests from the directory containing the project file:

```bash
dotnet test
```

The tests launch the configured browser, execute the scenarios, perform assertions, and report the results in the terminal.

To run only the positive test scenarios:

```bash
dotnet test --filter "FullyQualifiedName~ValidateTheResultsOfValidJourneys_CreateEditAndViewFeature"
```

To run only the negative test scenarios:

```bash
dotnet test --filter "FullyQualifiedName~ValidateTheResultsOfInvalidJourneysFeature"
```

## Test Reporting

The framework uses Extent Reports to generate HTML test reports, providing a clear summary of test execution results.

The report helps reviewers identify:

Overall test execution results.

Passed and failed tests.

Test execution details and logged information.

## Viewing the Extent Report

After running the tests, open the generated HTML report using the following command in the Windows Command Prompt:

start "" "TestResults\TestReport.html"

Alternatively, open TestReport.html directly in a web browser.

## Development Decisions and Rationale

### 1. BDD with Gherkin and Reqnroll

Gherkin was used to describe test scenarios in a readable Given–When–Then format. Reqnroll connects these scenarios to C# step definitions, making the tests easier to understand for both technical and non-technical stakeholders.

### 2. Page Object Model (POM)

Page objects are used to separate page locators and UI interactions from the step definitions. This avoids duplicating browser interaction code and makes maintenance easier when the application's UI changes.

### 3. Reusable Step Definitions and Methods

Common actions are implemented as reusable methods and called from multiple scenarios where appropriate. This reduces duplication while keeping the scenarios focused on the behaviour being tested.

### 4. Independent Test Scenarios

Each scenario establishes the data and application state it needs rather than relying on another scenario to run first. This makes the tests easier to execute individually and reduces dependencies on execution order.

### 5. Browser and Page Lifecycle Management

Reqnroll hooks are used to initialise Playwright and launch the browser before each scenario, then close the browser afterwards. This provides a clean browser context for each scenario and helps reduce unintended state leakage between tests.

### 6. Test Data Management

Test data is supplied through Gherkin steps and scenario parameters where appropriate. This keeps test inputs visible and makes it easier to reuse the same test logic with different values.

### 7. Headed browser execution:
Chromium is configured to run visibly, which makes it easier to observe and debug UI behavior during test runs.

### 8. 15-second default Playwright timeout:
A default timeout is set for page actions to allow time for elements to appear while preventing indefinite waits.

### 9. Assertions:

Assertions are included to verify the expected outcome of each scenario, rather than treating successful execution of browser actions as proof of success. 

### 10. ExtentReports output:
An HTML report records each scenario’s pass or fail status for review after executio

### 11. Cookie overlay timeout handling: 
After accepting cookies, the framework waits for the cookie overlay to become hidden. If that wait times out, it reloads the page and waits for the overlay to disappear again. This provides a recovery attempt when the overlay does not close as expected.

## Additional Test Scenarios

### Functional Tests:

1) Validate the journey details when the user selects the arrival date and time when planning the journey.
2) Validate that the recent journey details are displayed when the user turns on the recent journeys option 
3) Validate that the recent journey details are not displayed when the user turns off the recent journeys option 
4) Validate the location details when the user selects  <Use my location > option when planning a journey
5) Validate the details in the Favourite section, when the user adds their favourite journey
6) Validate the journey details when the user enters identical From and To locations.
7) Verify that appropriate error messages are displayed during service unavailability.
8) Validate the change in the journey details when the user edits journey.
9) Validate the change in the favourites journey details when the user edits the favourites.
10) Validate the journey results when the user selects different “Travel by” options
11) Validate the journey results when the user selects different “cycling journey” options
12) Validate the journey results when the user selects different “walking speeds”

The list above is not exhaustive. Further tests could be added to cover the following areas:
Earlier journeys, Later journeys, Save these preferences for future use etc

### Non-Functional Tests:

1. Evaluate the response time of the "plan a journey" feature. Verify whether the response time of the search meets the requirement.
2) Test that the "plan a journey" web application allows the specified number of concurrent users to search for, edit journeys etc without significant performance impact.
4) Access the "plan a journey" web application using different browsers and ensure it functions correctly on different browsers.
5) Access the "plan a journey" web application on various devices (e.g., desktop, tablet , mobile ) and ensure it adapts to different devices without loss of functionality. 
6) Test the usability of the "plan a journey" web application and ensure users can successfully complete tasks with minimal steps and effort.
7) Throughput testing- Test that the "plan a journey" web application processes the data within an acceptable time frame.
8) Perform the peak test to ensure the web application handles unpredictable load spikes.
9) Perform the soak test to ensure the stability of the web application over time under load.
10) Security testing to verify that <From> and <To> location input fields on the "plan a journey" page handles malicious input safely.

### AI Assistance Disclaimer

AI tools were used to assist with framework setup, installation, troubleshooting, some locator syntax questions, and README documentation.

The framework structure, test design, additional test scenarios, locator identification and selection, test implementation, and validation reflect my own work and problem-solving. AI assistance was used selectively where needed.


