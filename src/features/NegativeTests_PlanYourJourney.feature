Feature: Validate the results of invalid journeys

Background:
    Given I navigate to the Tfl journey planner widget
    When I accept all cookies
    Then I should land on the "Plan a journey" page


Scenario Outline: Validate the results when invalid start location is entered
    When I enter the start location without autocomplete <From>
    And I enter the destination location with autocomplete <To> 
    And I click on Plan my journey button
    Then Validate the <error> message is displayed

    Examples:
    | From  | To | error|
    | "79798675" | "Covent Garden Underground Station" | "Journey planner could not find any results to your search. Please try again" |

Scenario Outline:  Validate the results when invalid destination location is entered

    When I enter the start location with autocomplete <From>
    And I enter the destination location without autocomplete <To> 
    And I click on Plan my journey button
    Then Validate the <error> message is displayed

    Examples:
    | From  | To | error|
    | "Leicester Square Underground Station" | "4567676" | "Journey planner could not find any results to your search. Please try again" |

Scenario Outline:  Validate the results when invalid start and destination locations are entered
    
    When I enter the start location without autocomplete <From>
    And I enter the destination location without autocomplete <To> 
    And I click on Plan my journey button
    Then Validate the <error> message is displayed

    Examples:
    | From  | To | error|
    | "3223009" | "4567676" | "Journey planner could not find any results to your search. Please try again" |

Scenario Outline:  Validate the error message displayed for empty start location
    
    When I enter the destination location without autocomplete <To> 
    And I click on Plan my journey button
    Then Validate the <error> message is displayed for empty start location

     Examples:
    | To | error|
    |"Covent Garden Underground Station"  | "The From field is required." |

Scenario Outline:  Validate the error message displayed for empty destination location
    
    When I enter the start location without autocomplete <From>
    And I click on Plan my journey button
    Then Validate the <error> message is displayed for empty destination location

    Examples:
    | From | error|
    |"Leicester Square Underground Station"  | "The To field is required." |

Scenario Outline:  Validate the results when no start and destination locations are entered

    When I click on Plan my journey button
    Then Validate the <error1> and <error2> messages are displayed for empty start and destination locations

    Examples:
    | error1|error2| 
    | "The From field is required."|"The To field is required."| 