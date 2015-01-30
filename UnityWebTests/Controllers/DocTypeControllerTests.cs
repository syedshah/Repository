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
  using UnityWeb.Models.DocType;

  [TestFixture]
  public class DocTypeControllerTests : ControllerTestsBase
  {
    private Mock<IDocTypeService> _docTypeService;
    private Mock<ILogger> _logger;
    private DocTypeController _controller;

    [SetUp]
    public void SetUp()
    {
      _docTypeService = new Mock<IDocTypeService>();
      _logger = new Mock<ILogger>();

      _docTypeService.Setup(x => x.GetDocTypes()).Returns(new List<DocType>()
                                                                    {
                                                                      new DocType { },
                                                                      new DocType { },
                                                                      new DocType { },
                                                                    });

      _docTypeService.Setup(r => r.GetDocType(It.IsAny<int>())).Returns(new DocType());

      _controller = new DocTypeController(_docTypeService.Object, _logger.Object);
    }


    [Test]
    public void GivenADocTypeController_WhenTheIndexPageIsAccessed_ThenTheIndexVieWModelIsReturned()
    {
      var result = _controller.Index();
      result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void GivenADocTypeController_WhenTheIndexPageIsAccessed_ThenTheIndexViewShouldContainTheModel()
    {
      var result = (ViewResult)_controller.Index();
      result.Model.Should().BeOfType<DocTypesViewModel>();
    }

    [Test]
    public void GivenADocTypeController_WhenIViewTheIndexPage_ThenTheViewModelContainsTheCorrectNumberOfDocTypes()
    {
      var result = _controller.Index() as ViewResult;

      var model = result.Model as DocTypesViewModel;

      model.DocTypes.Should().HaveCount(3);
    }

    [Test]
    public void GivenAValiddDocType_WhenITryAndEditADocType_ThenIGetTheCorrectView()
    {
      ActionResult result = _controller.Edit(1);
      result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void GivenAValidDocType_WhenITryAndUpdateAnDocTypeIndex_ThenIGetTheCorrectView()
    {
      var result =
          _controller.Update(new EditDocTypeViewModel { Code = "code", DocTypeId = 1 }) as
          RedirectToRouteResult;

      result.Should().NotBeNull();
      result.RouteValues["controller"].Should().Be("DocType");
      result.RouteValues["action"].Should().Be("Index");
    }
  } 
}
