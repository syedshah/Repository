namespace ModelTests.File
{
  using System;
  using Entities.File;
  using FluentAssertions;
  using NUnit.Framework;

  [TestFixture]
  public class XmlFileTests
  {
    [Test]
    public void GivenAXmlFile_WhenICreateAXmlFile_TheXmlFileIsCreatedProperly()
    {
      var xmlFile = new XmlFile(
        "documentSetId",
        "fileName",
        "parentFileName",
        false,
        1,
        2,
        "3",
        "bigZip",
        new DateTime(2013, 1, 1),
        "allocatorGrid",
        new DateTime(2013, 1, 1));

      xmlFile.DocumentSetId.Should().Be("documentSetId");
      xmlFile.FileName.Should().Be("fileName");
      xmlFile.ParentFileName.Should().Be("parentFileName");
      xmlFile.DocTypeId.Should().Be(1);
      xmlFile.ManCoId.Should().Be(2);
      xmlFile.DomicileId.Should().Be("3");
    }
  }
}
