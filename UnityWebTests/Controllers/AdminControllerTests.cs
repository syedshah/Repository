namespace UnityWebTests.Controllers
{
  using System.Web.Mvc;
  using FluentAssertions;
  using Logging;
  using Moq;
  using NUnit.Framework;
  using UnityWeb.Controllers;

  [TestFixture]
  public class AdminControllerTests : ControllerTestsBase
  {
    private Mock<ILogger> _logger;
    private AdminController _controller;

    [SetUp]
    public void SetUp()
    {
      _logger = new Mock<ILogger>();

      _controller = new AdminController(_logger.Object);
    }

    [Test]
    public void GivenAAdminController_WhenTheIndexPageIsAccessed_ThenTheIndexVieWModelIsReturned()
    {
      var result = _controller.Index();
      result.Should().BeOfType<ViewResult>();
    }
  } 
}
