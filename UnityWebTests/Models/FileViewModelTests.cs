namespace UnityWebTests.Models
{
  using System.Collections.Generic;
  using FluentAssertions;
  using NUnit.Framework;
  using UnityWeb.Models.File;

  [TestFixture]
  public class FileViewModelTests
  {
    [Test]
    public void FileSearchCollection_WhenIAccessTheCollection_ThenItIsTheSameAsTheSetValue()
    {
      var files = new List<FileViewModel>();
      var model = new FilesViewModel();
      model.Files = files;

      model.Files.Should().BeEquivalentTo(files);
    }
  }
}
