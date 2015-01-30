namespace BusinessEngineTests
{
  using System;
  using System.Collections.ObjectModel;
  using BusinessEngineInterfaces;
  using BusinessEngines;
  using Entities;
  using FileRepository.Interfaces;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using UnityRepository.Interfaces;

  [TestFixture]
  public class HouseHoldingRunEngineTests
  {
    private Mock<IHouseHoldingFileRepository> _houseHoldingFileRepository;
    private Mock<IHouseHoldingRunRepository> _houseHoldingRunRepository;
    private Mock<IHouseHoldRepository> _houseHoldRepository;
    private Mock<IDocumentService> _documentService;
    private Mock<IGridRunService> _gridRunService;
    private IHouseHoldingRunEngine _houseHoldingRunEngine;

    [SetUp]
    public void SetUp()
    {
      _houseHoldingFileRepository = new Mock<IHouseHoldingFileRepository>();
      _houseHoldingRunRepository = new Mock<IHouseHoldingRunRepository>();
      _houseHoldRepository = new Mock<IHouseHoldRepository>();
      _documentService = new Mock<IDocumentService>();
      _gridRunService = new Mock<IGridRunService>();
      _houseHoldingRunEngine = new HouseHoldingRunEngine(
        _houseHoldingFileRepository.Object,
        _houseHoldingRunRepository.Object,
        _houseHoldRepository.Object,
        _documentService.Object,
        _gridRunService.Object);
    }

    [Test]
    public void GivenValidData_WhenITryToHouseHold_HouseHoldInformationIsSaved()
    {
      _houseHoldingFileRepository.Setup(h => h.GetHouseHoldingData()).Returns(new HouseHoldingRunData
                                                                                {
                                                                                  Grid = "grid",
                                                                                  ProcessingGridRun = new Collection<string>()
                                                                                                        {
                                                                                                          "grid1",
                                                                                                          "grid2"
                                                                                                        },
                                                                                  DocumentRunData = new Collection<DocumentRunData>
                                                                                                {
                                                                                                  new DocumentRunData
                                                                                                    {
                                                                                                      DocumentId = Guid.NewGuid().ToString(),
                                                                                                      HouseHoldDate = DateTime.Now
                                                                                                    },
                                                                                                    new DocumentRunData
                                                                                                    {
                                                                                                      DocumentId = Guid.NewGuid().ToString(),
                                                                                                      HouseHoldDate = DateTime.Now
                                                                                                    }
                                                                                                }
                                                                                });

      _documentService.Setup(c => c.GetDocument(It.IsAny<string>())).Returns(new Document());
      _gridRunService.Setup(g => g.GetGridRun(It.IsAny<string>())).Returns(new GridRun());

      _houseHoldingRunEngine.ProcessHouseHoldingRun();

      _houseHoldingRunRepository.Verify(x => x.Create(It.IsAny<HouseHoldingRun>()), Times.Once());
      _houseHoldRepository.Verify(x => x.Create(It.IsAny<HouseHold>()), Times.Exactly(2));
      _documentService.Verify(x => x.Update(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
      _gridRunService.Verify(x => x.Update(It.IsAny<int>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<int>()), Times.Exactly(2));
    }
  }
}
