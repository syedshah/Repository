namespace UnityWebTests.Controllers
{
  using System.Collections.Generic;
  using System.Web.Mvc;
  using Entities;
  using FluentAssertions;
  using Logging;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using UnityWeb.Controllers;
  using UnityWeb.Models.Search;

  [TestFixture]
  public class SearchControllerTests : ControllerTestsBase
  {
    private Mock<ILogger> _logger;
    private Mock<IDocTypeService> _docTypeService;
    private Mock<ISubDocTypeService> _subDocTypeService;
    private Mock<IManCoService> _manCoService;
    private Mock<IUserService> _userService;
    private SearchController _controller;

    [SetUp]
    public void SetUp()
    {
      _logger = new Mock<ILogger>();
      _docTypeService = new Mock<IDocTypeService>();
      _subDocTypeService = new Mock<ISubDocTypeService>();
      _manCoService = new Mock<IManCoService>();
      _userService = new Mock<IUserService>();

      _docTypeService.Setup(x => x.GetDocTypes()).Returns(new List<DocType>
                                                                    {
                                                                      new DocType { },
                                                                      new DocType { },
                                                                      new DocType { },
                                                                    });

      _subDocTypeService.Setup(x => x.GetSubDocTypes(1)).Returns(new List<SubDocType>
                                                                    {
                                                                      new SubDocType { DocType = new DocType() },
                                                                      new SubDocType { DocType = new DocType() },
                                                                    });

      _userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser());

      _manCoService.Setup(x => x.GetManCos()).Returns(new List<ManCo>
                                                        {
                                                          new ManCo { },
                                                        });

      _manCoService.Setup(x => x.GetManCosByUserId(It.IsAny<string>())).Returns(new List<ManCo> {new ManCo()});

      _controller = new SearchController(
          _docTypeService.Object, 
          _subDocTypeService.Object, 
          _manCoService.Object, 
          _userService.Object,
          _logger.Object);

    }

    [Test]
    public void GivenASearchController_WhenTheIndexPageIsAccessed_ThenTheIndexViewShouldContainTheModel()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["SearchViewModel"]).Returns(null);

      var result = (PartialViewResult)_controller.Index();
      result.Model.Should().BeOfType<SearchViewModel>();
    }

    [Test]
    public void GivenASearchController_WhenIViewTheIndexPage_AndModelIsNotInSesion_ThenTheViewModelContainsTheCorrectNumberOfManCos()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["SearchViewModel"]).Returns(null);

      var result = _controller.Index() as PartialViewResult;

      var model = result.Model as SearchViewModel;

      model.ManCos.Should().HaveCount(1);
    }

    [Test]
    public void GivenASearchController_WhenIViewTheIndexPage_AndModelIInSesion_ThenTheViewModelContainsTheCorrectNumberOfManCos()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["SearchViewModel"]).Returns(new SearchViewModel() { SelectedDocId = "1" });
      _subDocTypeService.Setup(x => x.GetSubDocTypes(1)).Returns(new List<SubDocType>() { new SubDocType() {DocType = new DocType()} });

      var result = _controller.Index() as PartialViewResult;

      var model = result.Model as SearchViewModel;

      model.ManCos.Should().HaveCount(1);
    }

    [Test]
    public void GivenASearchController_WhenIRequestTheSubDocTypeAction_ThenIGetAJsonResult()
    {
      var result = _controller.SubDocType(1) as JsonResult;

      result.Should().BeOfType<JsonResult>();
    }
    
    [Test]
    public void GivenASearchController_WhenIGoToTheResetController_ThenIGetTheRedirectView()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["SearchViewModel"]).Returns(null);

      var result = _controller.Reset() as RedirectToRouteResult;

      result.Should().NotBeNull();
      result.RouteValues["action"].Should().Be("Index");
      result.RouteValues["controller"].Should().Be("Document");
    }
  }
}
