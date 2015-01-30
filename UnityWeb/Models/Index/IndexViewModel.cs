namespace UnityWeb.Models.Index
{
  public class IndexViewModel
  {
    public IndexViewModel(Entities.IndexDefinition index)
    {
      Id = index.Id;
      Name = index.Name;
      ArchiveName = index.ArchiveName;
      ArchiveColumn = index.ArchiveColumn;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public string ArchiveName { get; set; }

    public string ArchiveColumn { get; set; }
  }
}