namespace UnityWebTests.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Web.Mvc;

  using Entities.File;

  using FluentAssertions;
  using Logging;
  using Moq;
  using NUnit.Framework;
  using UnityWeb.Controllers;
  using ServiceInterfaces;
  using Entities;
  using UnityWeb.Models.BigZip;

  [TestFixture]
  public class BigZipControllerTests
  {
    private Mock<IZipFileService> _zipFileService;
    private Mock<IUserService> _userService;
    private Mock<ILogger> _logger;
    private BigZipController _controller;

    [SetUp]
    public void SetUp()
    {
      this._zipFileService = new Mock<IZipFileService>();
        this._userService = new Mock<IUserService>();
      _logger = new Mock<ILogger>();

      this._controller = new BigZipController(this._zipFileService.Object,
          this._userService.Object,
            this._logger.Object);
    }

    [Test]
    public void GivenAValidFileName_ICanViewTheRightPageWhenIGoToBigZipSearch()
    {
      this._userService.Setup(x => x.GetUserManCoIds()).Returns(new List<int>());

      this._zipFileService.Setup(x => x.SearchBigZip(It.IsAny<string>(), It.IsAny<List<int>>())).Returns(new List<ZipFile>());

      var result = this._controller.Search(It.IsAny<string>()) as ViewResult;

      result.Should().NotBeNull();

      result.Model.Should().BeOfType<BigZipsViewModel>();

      this._userService.Verify(x => x.GetUserManCoIds(), Times.AtLeastOnce);

      this._zipFileService.Verify(x => x.SearchBigZip(It.IsAny<string>(), It.IsAny<List<int>>()), Times.AtLeastOnce);

    }
  }
}
