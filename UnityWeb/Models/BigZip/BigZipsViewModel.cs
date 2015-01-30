namespace UnityWeb.Models.BigZip
{
  using System.Collections.Generic;

  public class BigZipsViewModel
  {
    private List<BigZipViewModel> _files = new List<BigZipViewModel>();

    public BigZipsViewModel()
    {
          
    }

    public BigZipsViewModel(IList<Entities.File.ZipFile> files)
    {

    }

    public List<BigZipViewModel> Files
    {
      get { return _files; }
      set { _files = value; }   
    }

    public void AddFiles(IList<Entities.File.ZipFile> files)
    {
      foreach (Entities.File.ZipFile file in files)
      {
         var bzvm = new BigZipViewModel(file);
         Files.Add(bzvm);
      }
    }
  }
}