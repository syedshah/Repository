namespace UnityWeb.Models.Otf
{
  public class OtfParameterModel
  {
    public int AppId { get; set; }

    public int ManCoId { get; set; }

    public string AccountNumber { get; set; }

    public int Page { get; set; }

    public bool IsAjaxCall { get; set; }
  }
}