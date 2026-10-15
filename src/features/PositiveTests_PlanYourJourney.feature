Feature: Validate the results of valid journeys - Create, Edit and View

Background:
    Given I navigate to the Tfl journey planner widget
    When I accept all cookies
    Then I should land on the "Plan a journey" page

Scenario Outline: Plan a valid journey and validate the details

    When I enter the start location with autocomplete <From>
    And I enter the destination location with autocomplete <To> 
    And I click on Plan my journey button
    Then I should land on the "Journey results - Transport for London" page
    Then Validate the result <walking> for walking and <cycling> for cycling time

    Examples:
    | From  | To | walking | cycling |
    | "Leicester Square Underground Station" | "Covent Garden Underground Station" | "6mins" | "1mins" | 


Scenario Outline: Edit preferences and validate the journey time

    Given I have planned a journey from start location <From> to destination <To>
    When I click on edit preferences 
    Then more journey options are displayed
    When I select least walking routes and update
    Then Validate the journey time

    Examples:
    | From  | To |
    | "Leicester Square Underground Station" | "Covent Garden Underground Station" | 


Scenario Outline: View details and verify the access information
    
    Given I have planned a journey from start location <From> to destination <To>
    When I click on edit preferences 
    And I select least walking routes and update
    When I click on View Details
    Then Validate the access information <value1> <value2> <value3> at a given <location>

    Examples:
      | From  | To | value1 | value2 | value3 | location |
      | "Leicester Square Underground Station" | "Covent Garden Underground Station" | "Up stairs"| "Up lift" | "Level walkway" | "Covent Garden Underground Station"|