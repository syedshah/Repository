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
  public class SubDocTypeControllerTests : ControllerTestsBase
  {
    private Mock<ISubDocTypeService> _subDocTypeService;
    private Mock<IDocTypeService> _docTypeService;
    private Mock<ILogger> _logger;
    private SubDocTypeController _controller;

    [SetUp]
    public void SetUp()
    {
      _subDocTypeService = new Mock<ISubDocTypeService>();
      _docTypeService = new Mock<IDocTypeService>();
      _logger = new Mock<ILogger>();

      _subDocTypeService.Setup(x => x.GetSubDocTypes()).Returns(new List<SubDocType>()
                                                                    {
                                                                      new SubDocType { DocType = new DocType()},
                                                                      new SubDocType { DocType = new DocType()},
                                                                      new SubDocType { DocType = new DocType()}
                                                                    });

      _docTypeService.Setup(x => x.GetDocTypes()).Returns(new List<DocType>()
                                                            {
                                                              new DocType { SubDocTypes = new List<SubDocType>() { new SubDocType() }},
                                                              new DocType { SubDocTypes = new List<SubDocType>() { new SubDocType() }},
                                                              new DocType { SubDocTypes = new List<SubDocType>() { new SubDocType() }}
                                                            });

      _subDocTypeService.Setup(r => r.GetSubDocType(It.IsAny<int>())).Returns(new SubDocType());

      _controller = new SubDocTypeController(_subDocTypeService.Object, _docTypeService.Object, _logger.Object);
    }

    [Test]
    public void GivenASubDocTypeController_WhenTheIndexPageIsAccessed_ThenTheIndexVieWModelIsReturned()
    {
      var result = _controller.Index();
      result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void GivenASubDocTypeController_WhenTheIndexPageIsAccessed_ThenTheIndexViewShouldContainTheModel()
    {
      var result = (ViewResult)_controller.Index();
      result.Model.Should().BeOfType<SubDocTypesViewModel>();
    }

    [Test]
    public void GivenASubDocTypeController_WhenIViewTheIndexPage_ThenTheViewModelContainsTheCorrectNumberOfSubDocTypes()
    {
      var result = _controller.Index() as ViewResult;

      var model = result.Model as SubDocTypesViewModel;

      model.SubDocTypes.Should().HaveCount(3);
    }

    [Test]
    public void GivenASubDocTypeController_WhenIViewTheIndexPage_ThenTheViewModelContainsTheCorrectNumberOfDocTypes()
    {
      var result = _controller.Index() as ViewResult;

      var model = result.Model as SubDocTypesViewModel;

      model.AddSubDocTypeViewModel.DocTypesViewModel.DocTypes.Should().HaveCount(3) ;
    }

    [Test]
    public void GivenAValidSubDocType_WhenITryAndEditASubDocType_ThenIGetTheCorrectView()
    {
      ActionResult result = _controller.Edit(1);
      result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void GivenAValidSubDocType_WhenITryAndUpdateASubDocTypeIndex_ThenIGetTheCorrectView()
    {
      var result =
          _controller.Update(new EditSubDocTypeViewModel { Code = "code", SubDocTypeId = 1 }) as
          RedirectToRouteResult;

      result.Should().NotBeNull();
      result.RouteValues["controller"].Should().Be("SubDocType");
      result.RouteValues["action"].Should().Be("Index");
    }
  } 
}
