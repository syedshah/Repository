using System;
using TechTalk.SpecFlow;

namespace UnityScenarios
{
    [Binding]
    public class AuthenticationSteps
    {
        [Given]
        public void Given_I_am_registered()
        {
            ScenarioContext.Current.Pending();
        }

        [Given]
        public void Given_I_am_not_logged_on()
        {
            ScenarioContext.Current.Pending();
        }

        [Given]
        public void Given_I_enter_valid_credentials_into_the_logon_form()
        {
            ScenarioContext.Current.Pending();
        }

        [When]
        public void When_I_press_logon()
        {
            ScenarioContext.Current.Pending();
        }

        [Then]
        public void Then_the_dashboard_page_should_be_shown()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
