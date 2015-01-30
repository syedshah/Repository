namespace UnityWeb.Models.AutoApproval
{
  
  public class ManCoApprovalViewModel
  {
    public ManCoApprovalViewModel(Entities.ManCo manCos)
    {
      Id = manCos.Id;
      Code = manCos.Code;
      Description = manCos.Description;
      ManCoDisplay = string.Format("{0} - {1}", Code, Description);
    }

    public int Id { get; set; }

    public string Code { get; set; }

    public string Description { get; set; }

    public string ManCoDisplay { get; set; }
  }
}