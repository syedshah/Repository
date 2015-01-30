namespace ServiceTests
{
  using System;
  using Entities.File;
  using Exceptions;
  using FluentAssertions;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using Services;
  using UnityRepository.Interfaces;

  [TestFixture]
  public class ConFileServiceTests
  {
    private Mock<IConFileRepository> _conFileRepository;
    private IConFileService _conFileService;

    [SetUp]
    public void SetUp()
    {
      _conFileRepository = new Mock<IConFileRepository>();
      _conFileService = new ConFileService(_conFileRepository.Object);
    }

    [Test]
    public void GivenAXmlZipFile_WhenTheXmlFileIsAdded_ThenTheXmlFileIsAddedToTheRepository()
    {
      _conFileService.CreateConFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>());
      _conFileRepository.Verify(s => s.Create(It.IsAny<ConFile>()), Times.Once());
    }

    [Test]
    public void GivenAXmlZipFile_WhenTheXmlFileIsAdded_AndTheDatabaseIsUnavailable_ThenAUnityExceptionIsThrown()
    {
      _conFileService.CreateConFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>());
      _conFileRepository.Setup(c => c.Create(It.IsAny<ConFile>())).Throws<Exception>();

      Action act = () => _conFileService.CreateConFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>());

      act.ShouldThrow<UnityException>();
    }
  }
}
