namespace UnityWebTests.Controllers
{
  using System.Web.Mvc;
  using AbstractConfigurationManager;
  using FluentAssertions;
  using Logging;
  using Moq;
  using NUnit.Framework;
  using UnityWeb.Controllers;

  [TestFixture]
  public class EnvironmentControllerTests : ControllerTestsBase
  {
    private Mock<IConfigurationManager> _configurationManager;
    private Mock<ILogger> _logger;

    [SetUp]
    public void SetUp()
    {
      _configurationManager = new Mock<IConfigurationManager>();
      _logger = new Mock<ILogger>();
    }

    [Test]
    public void GivenAnEnvirnomentController_WhenICallItsIndexMethod_AndInTheDebugEnvironment_ThenIGetTheDebugPartialView()
    {
      _configurationManager.Setup(c => c.AppSetting(It.IsAny<string>())).Returns("DEBUG");

      var controller = new EnvironmentController(_configurationManager.Object, _logger.Object);
      var result = (PartialViewResult)controller.Index();
      result.ViewName.Should().Be("_debug");
    }

    [Test]
    public void GivenAnEnvirnomentController_WhenICallItsIndexMethod_AndInTheTestEnvironment_ThenIGetTheTestPartialView()
    {
      _configurationManager.Setup(c => c.AppSetting(It.IsAny<string>())).Returns("TEST");

      var controller = new EnvironmentController(_configurationManager.Object, _logger.Object);
      var result = (PartialViewResult)controller.Index();
      result.ViewName.Should().Be("_test");
    }

    [Test]
    public void GivenAnEnvirnomentController_WhenICallItsIndexMethod_AndInTheUatEnvironment_ThenIGetTheUatPartialView()
    {
      _configurationManager.Setup(c => c.AppSetting(It.IsAny<string>())).Returns("UAT");

      var controller = new EnvironmentController(_configurationManager.Object, _logger.Object);
      var result = (PartialViewResult)controller.Index();
      result.ViewName.Should().Be("_uat");
    }

    [Test]
    public void GivenAnEnvirnomentController_WhenICallItsIndexMethod_AndInTheProdEnvironment_ThenIGetTheProdPartialView()
    {
      _configurationManager.Setup(c => c.AppSetting(It.IsAny<string>())).Returns("PROD");

      var controller = new EnvironmentController(_configurationManager.Object, _logger.Object);
      var result = (PartialViewResult)controller.Index();
      result.ViewName.Should().Be("_prod");
    }
  }
}
