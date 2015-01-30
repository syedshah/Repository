namespace ArchiveServiceFactory.Interfaces
{
  using ArchiveServiceFactory.ArchiveService;

  public interface IDocumentServiceFactory
  {
    IDocumentChannel CreateChannel();
  }
}
