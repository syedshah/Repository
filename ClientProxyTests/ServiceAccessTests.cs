namespace ClientProxyTests
{
  using System;
  using ClientProxies.ArchiveServiceReference;
  using Exceptions;
  using FluentAssertions;
  using NUnit.Framework;

  [TestFixture]
  public class ServiceAccessTests
  {
    [Test]
    public void GivenADocumentService_WhenITryToOpenTheConnection_TheConnectionIsOpened()
    {
      DocumentClient proxy = new DocumentClient();

      proxy.Open();

      Action act = proxy.Open;

      act.ShouldNotThrow<UnityException>();
    }
  }
}
