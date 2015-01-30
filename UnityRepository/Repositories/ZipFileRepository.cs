namespace UnityRepository.Repositories
{
    using System;
    using System.Collections.Generic;
  using System.Data.Entity;
  using System.Linq;
  using EFRepository;
  using Entities.File;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class ZipFileRepository : BaseEfRepository<ZipFile>, IZipFileRepository
  {
    public ZipFileRepository(string connectionString)
      : base(new UnityDbContext(connectionString))
    {
    }

    public IList<ZipFile> SearchBigZip(string fileName)
    {
      return (from f in Entities
              where f.FileName.Contains(fileName)
              && f.BigZip == true
              select f).ToList();
    }

    public IList<ZipFile> SearchBigZip(string fileName, List<int> manCoIds)
    {
        //return (from f in Entities
        //        where f.FileName.Contains(fileName)
        //        && manCoIds.Contains(f.ManCoId)
        //        && f.BigZip == true
        //        select f).ToList();

        throw new NotImplementedException();
    }
  }
}
