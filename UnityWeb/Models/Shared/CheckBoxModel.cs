namespace UnityWeb.Models.Shared
{
  public class CheckBoxModel
  {
    public CheckBoxModel()
    {
    }

    public CheckBoxModel(Entities.ManCo manCo)
    {
      Id = manCo.Id;
      Value = manCo.Id.ToString();
      Text = manCo.Code + ' ' + manCo.Description;
    }

    public int Id { get; set; }

    public bool IsSelected { get; set; }

    public string Text { get; set; }

    public string Value { get; set; }

    public object Tags { get; set; }
  }
}