namespace FileRepository.Repositories
{
  using System;
  using System.IO;
  using System.Linq;
  using SystemFileAdapter;
  using Entities;
  using FileRepository.Interfaces;
  using FileSystemInterfaces;

  public class HouseHoldingFileRepository : BaseFileRepository, IHouseHoldingFileRepository
  {
    public HouseHoldingFileRepository(string path, IFileInfoFactory fileInfo, IDirectoryInfo directoryInfo)
      : base(path, fileInfo, directoryInfo)
    {
    }

    protected override string GenerateFileName()
    {
      return GenerateFileName("filename");
    }

    private string GenerateFileName(string id)
    {
      return string.Format("{0}/{1}.csv", Path, id);
    }
    
    public HouseHoldingRunData GetHouseHoldingData()
    {
      var houseHoldingRun = new HouseHoldingRunData();

      int count = 1;
      foreach (var file in Entities)
      {
        string[] csvLines = File.ReadAllLines(file.FullName);

        var lines = (from l in csvLines 
                    let data = l.Split(',') 
                    select new Row
                             {
                               ColumnOne = data[0],
                               ColumnTwo = data[1],
                               ColumnThree = data[2]
                             }).ToList();
        

        foreach (var row in lines)
        {
          if (count == 1)
          {
            houseHoldingRun.Grid = row.ColumnOne;
            houseHoldingRun.StartDate = Convert.ToDateTime(row.ColumnTwo);
            houseHoldingRun.EndDate = Convert.ToDateTime(row.ColumnThree);
          }
          else
          {
            houseHoldingRun.DocumentRunData.Add(new DocumentRunData
                                            {
                                              DocumentId = row.ColumnTwo,
                                              HouseHoldDate = Convert.ToDateTime(row.ColumnThree)
                                            });

            if (!houseHoldingRun.ProcessingGridRun.Contains(row.ColumnOne))
            {
              houseHoldingRun.ProcessingGridRun.Add(row.ColumnOne);
            }
          }
          count++;
        }
      }
      return houseHoldingRun;
    }

    public class Row
    {
      public string ColumnOne { get; set; }

      public string ColumnTwo { get; set; }

      public string ColumnThree { get; set; }
    }
  }
}
