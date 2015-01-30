namespace UnityWeb.Models.Index
{
  using System.ComponentModel.DataAnnotations;

  public class EditIndexViewModel
  {
    [Required]
    public int IndexId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string ArchiveName { get; set; }

    [Required]
    public string ArchiveColumn { get; set; }
  }
}