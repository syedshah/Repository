namespace Entities
{
  using System;
  using System.Collections.Generic;

  public class PagedResult<T>
  {
    public IList<T> Results { get; set; }
    public int TotalItems { get; set; }
    public int ItemsPerPage { get; set; }
    public int CurrentPage { get; set; }
    public int StartRow { get; set; }
    public int EndRow { get; set; }
    public int TotalPages
    {
      get
      {
        return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
      }
    }
  }
}
