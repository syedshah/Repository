namespace UnityWeb.Models.DocType
{
  using System.ComponentModel.DataAnnotations;

  public class EditDocTypeViewModel
  {
    [Required]
    public int DocTypeId { get; set; }

    [Required]
    public string Code { get; set; }

    [Required]
    public string Description { get; set; }
  }
}