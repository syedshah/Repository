namespace UnityWebTests.Models
{
  using System.Collections.Generic;
  using FluentAssertions;
  using NUnit.Framework;
  using UnityWeb.Models.Document;
  using UnityWeb.Models.Helper;

  [TestFixture]
  public class DocumentsViewModelTests
  {
    [Test]
    public void GivenADocumentsViewModel_WhenIGetACollectionOfDocumentViewModels_ThenItIsInitialized()
    {
      var model = new DocumentsViewModel();
      List<DocumentViewModel> documents = model.Documents;
      PagingInfoViewModel pagingInfoViewModel = model.PagingInfo;
      documents.Should().NotBeNull();
      pagingInfoViewModel.Should().NotBeNull();
    }
  }
}
