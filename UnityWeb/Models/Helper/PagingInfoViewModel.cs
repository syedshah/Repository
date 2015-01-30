namespace UnityWeb.Models.Helper
{
  public class PagingInfoViewModel
  {
    public int TotalItems { get; set; }
    public int ItemsPerPage { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int StartRow { get; set; }
    public int EndRow { get; set; }
  }
}