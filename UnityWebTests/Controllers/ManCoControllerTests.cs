namespace UnityWebTests.Controllers
{
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Web.Mvc;
  using Entities;
  using FluentAssertions;
  using Logging;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using UnityWeb.Controllers;
  using UnityWeb.Models.Applicaiton;
  using UnityWeb.Models.DocType;
  using UnityWeb.Models.ManCo;

  [TestFixture]
  public class ManCoControllerTests : ControllerTestsBase
  {
    private Mock<IManCoService> _manCoService;
    private Mock<ILogger> _logger;
    private ManCoController _controller;

    [SetUp]
    public void SetUp()
    {
      _manCoService = new Mock<IManCoService>();
      _logger = new Mock<ILogger>();

      _manCoService.Setup(x => x.GetManCos()).Returns(new List<ManCo>()
                                                        {
                                                          new ManCo { }, new ManCo { }, new ManCo { },
                                                        });

      _manCoService.Setup(r => r.GetManCo(It.IsAny<int>())).Returns(new ManCo());
      _controller = new ManCoController(_manCoService.Object, _logger.Object);
    }


    [Test]
    public void GivenAManCoController_WhenTheIndexPageIsAccessed_ThenTheIndexVieWModelIsReturned()
    {
      var result = _controller.Index();
      result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void GivenAManCoController_WhenTheIndexPageIsAccessed_ThenTheIndexViewShouldContainTheModel()
    {
      var result = (ViewResult)_controller.Index();
      result.Model.Should().BeOfType<ManCosViewModel>();
    }

    [Test]
    public void GivenAManCoController_WhenIViewTheIndexPage_ThenTheViewModelContainsTheCorrectManCos()
    {
      var result = _controller.Index() as ViewResult;

      var model = result.Model as ManCosViewModel;

      model.ManCos.Should().HaveCount(3);
    }

    [Test]
    public void GivenAValiddManCo_WhenITryAndEditAManCo_ThenIGetTheCorrectView()
    {
      ActionResult result = _controller.Edit(1);
      result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void GivenAValidManCo_WhenITryAndUpdateAManCo_ThenIGetTheCorrectView()
    {
      var result =
          _controller.Update(new EditManCoViewModel { Code = "name", ManCoId = 1 }) as
          RedirectToRouteResult;

      result.Should().NotBeNull();
      result.RouteValues["controller"].Should().Be("ManCo");
      result.RouteValues["action"].Should().Be("Index");
    }

  } 
}
