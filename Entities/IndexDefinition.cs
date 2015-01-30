namespace Entities
{
  using System;

  public class IndexDefinition
  {
    public enum IndexTypes
    {
      Undefined,
      String,
      Int,
      XML,
      Decimal,
      Bool,
      DateTime
    };

    private int _type;

    public int Id { get; set; }

    public string Name { get; set; }

    public string ArchiveName { get; set; }

    public string ArchiveColumn { get; set; }

    public int ApplicationId { get; set; }

    public Application Application { get; set; }

    public virtual int Type
    {
      get { return _type; }
      set
      {
        if (Enum.IsDefined(typeof(IndexTypes), value))
        {
          _type = value;
        }
        else
        {
          _type = (int)IndexTypes.Undefined;
        }
      }
    }

    public void UpdateIndex(string name, string archiveName, string archiveColumn)
    {
      if (!string.IsNullOrEmpty(name))
        Name = name;
      if (!string.IsNullOrEmpty(archiveName))
        ArchiveName = archiveName;
      if (!string.IsNullOrEmpty(archiveColumn))
        ArchiveColumn = archiveColumn;
    }
  }
}
