@Business
Feature: Authentication
	In order access the system
	As a valid user
	I want to be able to logon

Scenario: Logon with identity authentication
	Given I am registered
	And I am not logged on
	And I enter valid credentials into the logon form
	When I press logon
	Then the dashboard page should be shown