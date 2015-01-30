namespace UnityWeb.Models.DocType
{
  using System.ComponentModel.DataAnnotations;

  public class EditManCoViewModel
  {
    [Required]
    public int ManCoId { get; set; }

    [Required]
    public string Code { get; set; }

    [Required]
    public string Description { get; set; }
  }
}