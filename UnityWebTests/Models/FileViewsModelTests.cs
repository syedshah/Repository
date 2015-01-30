namespace UnityWebTests.Models
{
  using System.Collections.Generic;
  using FluentAssertions;
  using NUnit.Framework;
  using UnityWeb.Models.File;
  

  [TestFixture]
  public class FileViewsModelTests
  {
    [Test]
    public void GivenAFileSearchesViewModel_WhenIGetACollectionOfFileSearchesViewModels_ThenItIsInitialized()
    {
      var model = new FilesViewModel();
      List<FileViewModel> gridRuns = model.Files;
      gridRuns.Should().NotBeNull();
    }
  }
}
