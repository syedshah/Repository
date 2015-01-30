namespace UnityWebTests.Controllers
{
  using System;
  using System.Web.Mvc;
  using AbstractConfigurationManager;
  using FluentAssertions;
  using Logging;
  using Moq;
  using NUnit.Framework;
  using UnityWeb.Controllers;
  using ServiceInterfaces;
  using Entities;
  using UnityWeb.Models.NewsTicker;

  [TestFixture]
  public class NewsTickerControllerTests : ControllerTestsBase
  {
    private Mock<INewsTickerService> _newsTickerService;
    private Mock<ILogger> _logger;
    private NewsTickerController _controller;

    [SetUp]
    public void SetUp()
    {
      _newsTickerService = new Mock<INewsTickerService>();
      _logger = new Mock<ILogger>();
      _newsTickerService.Setup(x => x.GetNewsTicker()).Returns(new NewsTicker("Test news", DateTime.Now.AddDays(-2)));
      _controller = new NewsTickerController(_newsTickerService.Object, _logger.Object);
    }

    [Test]
    public void GivenANewsTickerToController_ThenTheNewsTickerHTMLTextReturned()
    {
      var result = _controller.GetNewsTicker();
      result.Should().BeOfType<PartialViewResult>();
    }

    [Test]
    public void GivenABlankNewsTickerToCoController_ThenTheNullResultShouldReturn()
    {
      _newsTickerService.Setup(x => x.GetNewsTicker()).Returns(new NewsTicker());
      var result = (PartialViewResult)_controller.GetNewsTicker();
      Assert.AreEqual(result.Model, null);
    }

    [Test]
    public void GivenNoNewsTicker_ThenIGetTheNullView()
    {
      _newsTickerService.Setup(x => x.GetNewsTicker()).Returns((NewsTicker)null);
      var result = (PartialViewResult)_controller.GetNewsTicker();
      Assert.AreEqual(result.Model, null);
    }


    [Test]
    public void CallingNewsTicker_WhenITryEditNewsTicker_ThenIGetTheCorrectView()
    {
      ActionResult result = _controller.Edit();
      result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void GivenAValidNewsTickerAndUpdate_ThenIGetTheCorrectView()
    {
      var result =
          _controller.Update(new EditNewsTickerViewModel { Id = 5, News = "Test news", Date = DateTime.Now }) as
          RedirectToRouteResult;

      result.Should().NotBeNull();
      result.RouteValues["controller"].Should().Be("Dashboard");
      result.RouteValues["action"].Should().Be("Index");
    }



  }
}
