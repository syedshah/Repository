namespace UnityWebTests.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

  using Entities.File;

  using FluentAssertions;
  using Logging;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using UnityWeb.Controllers;
  using UnityWeb.Models.QuickSearch;

  [TestFixture]
  public class QuickSearchControllerTests : ControllerTestsBase
  {
    private Mock<ILogger> _logger;
    private Mock<IGridRunService> _gridRunService;
    private Mock<IXmlFileService> _xmlFileService;
    private Mock<IZipFileService> _zipFileService;
    private Mock<IUserService> _userService;
    private QuickSearchController _controller;

    [SetUp]
    public void SetUp()
    {
      _logger = new Mock<ILogger>();
      _gridRunService = new Mock<IGridRunService>();
      _xmlFileService = new Mock<IXmlFileService>();
      _zipFileService = new Mock<IZipFileService>();
      _userService = new Mock<IUserService>();
      _controller = new QuickSearchController(
        _gridRunService.Object,
        _xmlFileService.Object,
        _zipFileService.Object,
        _userService.Object,
        _logger.Object);
    }

    [Test]
    public void GivenAQuickSearchController_WhenICallItsIndexMethod_ThenIGetTheIndexPartialView()
    {
      var result = (PartialViewResult)_controller.Index();
      result.ViewName.Should().Be("_Index");
    }

    [Test]
    public void GivenAQuickSearchController_WhenICallItsIndexMethod_ThenCorrectViewModelIsReturned()
    {
      var result = _controller.Index() as PartialViewResult;
      var model = result.Model as QuickSearchViewModel;

      model.Should().NotBeNull();
    }

    [Test]
    public void GivenAQuickSearchController_WhenICallItsBigZipMethodIsCalled_ThenCorrectViewModelIsReturned()
    {
        this._userService.Setup(x => x.GetUserManCoIds()).Returns(new List<int>());

        this._zipFileService.Setup(x => x.SearchBigZip(It.IsAny<string>(), It.IsAny<List<int>>())).Returns(new List<ZipFile>());

        var result = this._controller.BigZip(It.IsAny<string>());

        result.Should().NotBeNull();
        result.Should().BeOfType<JsonResult>();
        this._zipFileService.Verify(x => x.SearchBigZip(It.IsAny<string>(), It.IsAny<List<int>>()), Times.AtLeastOnce);
        this._userService.Verify(x => x.GetUserManCoIds(), Times.AtLeastOnce);
    }
  }
}
