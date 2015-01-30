namespace BusinessEngines
{
  using System;
  using BusinessEngineInterfaces;
  using Entities;
  using FileRepository.Interfaces;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  public class HouseHoldingRunEngine : IHouseHoldingRunEngine
  {
    private readonly IHouseHoldingFileRepository _houseHoldingFileRepository;
    private readonly IHouseHoldingRunRepository _houseHoldingRunRepository;
    private readonly IHouseHoldRepository _houseHoldRepository;
    private readonly IDocumentService _documentService;
    private readonly IGridRunService _gridRunService;

    public HouseHoldingRunEngine(
      IHouseHoldingFileRepository houseHoldingFileRepository,
      IHouseHoldingRunRepository houseHoldingRunRepository,
      IHouseHoldRepository houseHoldRepository,
      IDocumentService documentService,
      IGridRunService gridRunService)
    {
      _houseHoldingFileRepository = houseHoldingFileRepository;
      _houseHoldingRunRepository = houseHoldingRunRepository;
      _houseHoldRepository = houseHoldRepository;
      _documentService = documentService;
      _gridRunService = gridRunService;
    }

    public void ProcessHouseHoldingRun()
    {
      HouseHoldingRunData houseHoldingRunData = _houseHoldingFileRepository.GetHouseHoldingData();

      var houseHoldingRun = new HouseHoldingRun
                              {
                                Grid = houseHoldingRunData.Grid,
                                StartDate = houseHoldingRunData.StartDate,
                                EndDate = houseHoldingRunData.EndDate
                              };

      _houseHoldingRunRepository.Create(houseHoldingRun);

      foreach (var grid in houseHoldingRunData.ProcessingGridRun)
      {
        var gridRun = _gridRunService.GetGridRun(grid);
        _gridRunService.Update(gridRun.Id, gridRun.StartDate, gridRun.EndDate, gridRun.Status, gridRun.XmlFileId, houseHoldingRun.Id);
      }

      foreach (var documentData in houseHoldingRunData.DocumentRunData)
      {
        var document = _documentService.GetDocument(documentData.DocumentId);

        var houseHold = new HouseHold()
        {
          HouseHoldDate = DateTime.Now,
          DocumentId = document.Id
        };

        _houseHoldRepository.Create(houseHold);

        _documentService.Update(document.Id, houseHoldingRun.Id);
      }
    }
  }
}
