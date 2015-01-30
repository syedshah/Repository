namespace NTGEN94
{
  using System.Collections.Generic;

  public class ExportFileData
  {
    public ExportFileData()
    {
      DocumentId = new List<string>();
    }

    public string ApplicationCode { get; set; }

    public string Grid { get; set; }

    public List<string> DocumentId { get; set; }
  }
}
