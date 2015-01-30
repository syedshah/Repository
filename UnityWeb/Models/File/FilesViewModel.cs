namespace UnityWeb.Models.File
{
  using System.Collections.Generic;

  public class FilesViewModel
  {
    private List<FileViewModel> _files = new List<FileViewModel>();

    public FilesViewModel()
    {
    }

    public FilesViewModel(IList<Entities.File.XmlFile> files)
    {
      AddFiles(files);
    }

    public List<FileViewModel> Files
    {
      get { return _files; }
      set { _files = value; }
    }

    public void AddFiles(IList<Entities.File.XmlFile> files)
    {
      foreach (Entities.File.XmlFile file in files)
      {
        foreach (var gridRuns in file.GridRuns)
        {
          var fvm = new FileViewModel(gridRuns);
          Files.Add(fvm);  
        }
      }
    }
  }
}