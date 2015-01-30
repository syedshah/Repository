namespace UnityRepository.Initializers
{
  using System;
  using System.Collections.Generic;
  using System.Data.Entity;
  using System.Data.Entity.Migrations;
  using Entities;
  using Entities.File;
  using UnityRepository.Contexts;

  public class UnityTestInitializer : CreateDatabaseIfNotExists<UnityDbContext>
  {
    protected override void Seed(UnityDbContext context)
    {
      /*var application = new Application { Code = "code", Description = "description" };

      var docTypeContractNotes = new DocType("CNT", "CNT (Contract Note)");
      var domicile = new Domicile("XXX", "DomDescription");

      var manCoSarsin = new ManCo("Sarasin", "Sarasin management company");
      var manFulcrum = new ManCo("Fulcrum", "Fulcrum management company");

      var gridRunException = new GridRun(Guid.NewGuid().ToString(), false, DateTime.Now.AddHours(-1), DateTime.Now, 4)
                               {
                                 Application = application,
                                 StartDate = DateTime.Now
                               };

      var gridRunInProgressOne = new GridRun(Guid.NewGuid().ToString(), false, DateTime.Now, null, 1)
                                   {
                                     Application = application,
                                     StartDate = DateTime.Now
                                   };
      var gridRunInProgressTwo = new GridRun(Guid.NewGuid().ToString(), false, DateTime.Now, null, 1)
                                   {
                                     Application = application,
                                     StartDate = DateTime.Now
                                   };

      context.XmlFiles.AddOrUpdate(
        x => x.FileName,
        new XmlFile
        {
          FileName = "NTSFMC.VBUCK1C.FMC7649C.XDDSTM.S030151.XML",
          DocType = docTypeContractNotes,
          DocumentSetId = Guid.NewGuid().ToString(),
          Domicile = domicile,
          ParentFileName = "NTSFMC.VBUCK1C.FMC7649C.XDDSTM.S030151.zip",
          ManCo = manCoSarsin,
          OffShore = false,
          GridRuns = new List<GridRun> { gridRunInProgressOne },
          BigZip = "NTSFMC.VBUCK1C.FMC7649C.D121001.T014821.ZIP",
          Allocated = DateTime.Now,
          Received = DateTime.Now
        });
      
      context.XmlFiles.AddOrUpdate(
        x => x.FileName,
        new XmlFile
        {
          FileName = "NTSFMC.VBUCK1C.FMC7649C.XDDSTM.S030131.XML",
          DocType = docTypeContractNotes,
          DocumentSetId = Guid.NewGuid().ToString(),
          Domicile = domicile,
          ParentFileName = "NTSFMC.VBUCK1C.FMC7649C.XDDSTM.S030131.zip",
          ManCo = manCoSarsin,
          OffShore = false,
          GridRuns = new List<GridRun> { gridRunInProgressOne },
          BigZip = "NTSFMC.VBUCK1C.FMC7649C.D121001.T014820.ZIP",
          Allocated = DateTime.Now,
          Received = DateTime.Now
        },
        new XmlFile
        {
          FileName = "NTSFMC.VBUCK1C.FMC7649C.XDDSTM.S030132.XML",
          DocType = docTypeContractNotes,
          DocumentSetId = Guid.NewGuid().ToString(),
          Domicile = domicile,
          ParentFileName = "NTSFMC.VBUCK1C.FMC7649C.XDDSTM.S030132.zip",
          ManCo = manFulcrum,
          OffShore = true,
          GridRuns = new List<GridRun> { gridRunInProgressTwo },
          BigZip = "NTSFMC.VBUCK1C.FMC7649C.D121001.T014820.ZIP",
          Allocated = DateTime.Now,
          Received = DateTime.Now
        },
        new XmlFile
        {
          FileName = "NTSFMC.VBUCK1C.FMC7649C.XDDSTM.S030133.XML",
          DocType = docTypeContractNotes,
          DocumentSetId = Guid.NewGuid().ToString(),
          Domicile = domicile,
          ParentFileName = "NTSFMC.VBUCK1C.FMC7649C.XDDSTM.S030132.zip",
          ManCo = manFulcrum,
          OffShore = true,
          GridRuns = new List<GridRun> { gridRunInProgressOne },
          BigZip = "NTSFMC.VBUCK1C.FMC7649C.D121001.T014820.ZIP",
          Allocated = DateTime.Now,
          Received = DateTime.Now
        });

      context.XmlFiles.Add(
        new XmlFile
        {
          FileName = "NTSFMC.VBUCK1C.FMC7649C.XDDSTM.S030152.XML",
          DocType = docTypeContractNotes,
          DocumentSetId = Guid.NewGuid().ToString(),
          Domicile = domicile,
          ParentFileName = "NTSFMC.VBUCK1C.FMC7649C.XDDSTM.S030152.zip",
          ManCo = manFulcrum,
          OffShore = true,
          GridRuns = new List<GridRun> { gridRunException },
          BigZip = "NTSFMC.VBUCK1C.FMC7649C.D121001.T014821.ZIP",
          Allocated = DateTime.Now,
          Received = DateTime.Now
        });*/
    }
  }
}
