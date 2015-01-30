namespace UnityWebTests.Controllers
{
  using System.Web.Mvc;
  using Entities;
  using FluentAssertions;
  using Logging;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using UnityWeb.Controllers;
  using UnityWeb.Models.Index;

  [TestFixture]
  public class IndexControllerTests : ControllerTestsBase
  {
    [SetUp]
    public void SetUp()
    {
      _applicationDomain = new Mock<IApplicationService>();
      _indexService = new Mock<IIndexService>();
      _logger = new Mock<ILogger>();

      _controller = new IndexController(_applicationDomain.Object, _indexService.Object, _logger.Object);
      var headers = new FormCollection { { "Referer", ExpectedRefererUrl } };
      MockRequest.Setup(r => r.Headers).Returns(headers);

      _indexService.Setup(r => r.GetIndex(It.IsAny<int>())).Returns(new IndexDefinition());

      SetControllerContext(_controller);
    }
    private Mock<IApplicationService> _applicationDomain;
    private Mock<IIndexService> _indexService;
    private Mock<ILogger> _logger;
    private IndexController _controller;
    private const string ExpectedRefererUrl = "value";
    private const string _name = "Name";
    private const string ArchiveName = "ArchiveName";
    private const string ArchiveColumn = "ArchiveColumn";

    [Test]
    public void GivenAnIndexController_WhenCreateIsCalled_ThenTheIndexStateIsAddedToTempData()
    {
      _controller.ModelState.AddModelError("Name", "Name error");
      _controller.Create(new AddIndexViewModel(1) { Name = _name, ArchiveColumn = ArchiveColumn });
      Assert.That(_controller.TempData, Is.Not.Null);
      Assert.That(_controller.TempData["comment"], Is.Not.Null);
    }

    [Test]
    public void GivenAnIndexController_WhenCreateIsCalled_ThenTheBrowserIsRedirectedToTheReferer()
    {
      var result = (RedirectResult)_controller.Create(new AddIndexViewModel(1) { Name = _name, ArchiveName = ArchiveName, ArchiveColumn = ArchiveColumn });
      Assert.That(result.Url, Is.EqualTo(ExpectedRefererUrl));
      _applicationDomain.Verify(p => p.AddIndex(1, _name, ArchiveName, ArchiveColumn));
    }

    [Test]
    public void GivenAValidIndex_WhenITryAndEditAnIndex_ThenIGetTheCorrectView()
    {
      ActionResult result = _controller.Edit(1);
      result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void GivenAnIndexController_WhenCreateIsCalled_ThenTheIndexIsAdded()
    {
      _controller.Create(new AddIndexViewModel(1) { Name = _name, ArchiveName = ArchiveName, ArchiveColumn = ArchiveColumn });
      _applicationDomain.Verify(p => p.AddIndex(1, _name, ArchiveName, ArchiveColumn));
    }

    [Test]
    public void GivenAValidIndex_WhenITryAndUpdateAnIndex_ThenIGetTheCorrectView()
    {
      var result =
          _controller.Update(new EditIndexViewModel { Name = _name, IndexId = 1 }) as
          RedirectToRouteResult;

      result.Should().NotBeNull();
      result.RouteValues["controller"].Should().Be("Application");
      result.RouteValues["action"].Should().Be("Index");
    }
  }
}
