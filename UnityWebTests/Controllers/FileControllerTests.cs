namespace UnityWebTests.Controllers
{
  using System.Collections.Generic;
  using System.Web.Mvc;
  using Entities;
  using Entities.File;
  using FluentAssertions;
  using Logging;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using UnityWeb.Controllers;
  using UnityWeb.Models.File;

  [TestFixture]
  public class FileControllerTests : ControllerTestsBase
  {
    private Mock<IXmlFileService> _xmlFileService;
    private Mock<IUserService> _userService;
    private Mock<ILogger> _logger;

    [SetUp]
    public void SetUp()
    {
      _xmlFileService = new Mock<IXmlFileService>();
      _userService = new Mock<IUserService>();
      _logger = new Mock<ILogger>();

      var manCos = new List<ApplicationUserManCo>();

      manCos.Add(new ApplicationUserManCo() { ManCoId = 2 });
      manCos.Add(new ApplicationUserManCo() { ManCoId = 3 });

      this._userService.Setup(x => x.GetUserManCoIds()).Returns(new List<int>());

      _xmlFileService.Setup(j => j.Search(string.Empty, It.IsAny<List<int>>())).Returns(xmlFiles);
    }

    private readonly List<XmlFile> xmlFiles = new List<XmlFile>
                                                {
                                                    new XmlFile { GridRuns = new List<GridRun> { new GridRun() { XmlFile = new XmlFile {}} } },
                                                    new XmlFile { GridRuns = new List<GridRun> { new GridRun() { XmlFile = new XmlFile {}} } },
                                                    new XmlFile { GridRuns = new List<GridRun> { new GridRun() { XmlFile = new XmlFile {}} } }
                                                };
    

    [Test]
    public void GivenAFileController_WhenICallItsSearchMethod_ThenItRetrunsTheCorrectNumberXmlFiles()
    {
      var controller = new FileController(_xmlFileService.Object, _userService.Object, _logger.Object);
      var result = (ViewResult)controller.Search(string.Empty);

      var model = (FilesViewModel)result.Model;

      model.Files.Should().HaveCount(xmlFiles.Count);
      this._userService.Verify(x => x.GetUserManCoIds(), Times.AtLeastOnce);
    }

    [Test]
    public void GivenAFileController_WhenICallItsSearchMethod_ThenItReturnsTheCorrectView()
    {
      var controller = new FileController(_xmlFileService.Object, _userService.Object, _logger.Object);
      var result = (ViewResult)controller.Search(string.Empty);
      result.Model.Should().BeOfType<FilesViewModel>();
      this._userService.Verify(x => x.GetUserManCoIds(), Times.AtLeastOnce);
    }
  }
}