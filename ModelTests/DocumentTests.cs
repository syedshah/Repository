namespace ModelTests
{
  using Entities;

  using FluentAssertions;

  using NUnit.Framework;

  [Category("Unit")]
  [TestFixture]
  public class DocumentTests
  {
    [Test]
    public void GivenADocumentObjectWithAnEmptyDoNotPrintFlag_ThenTheDoNotPrintFlagIsSetToTrue()
    {
      var document = new Document(string.Empty);
      document.MailPrintFlag.Should().Be("Y");
    }

    [Test]
    public void GivenADocumentObjectWithAnNonEmptyDoNotPrintFlag_ThenTheDoNotPrintFlagIsSetToTheValueSuppied()
    {
      var document = new Document("N");
      document.MailPrintFlag.Should().Be("N");
    }
  }
}
