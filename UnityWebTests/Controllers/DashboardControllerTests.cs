namespace UnityWebTests.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Web.Mvc;
  using Entities;
  using Entities.File;
  using FluentAssertions;
  using Logging;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using UnityWeb.Controllers;
  using UnityWeb.Models.Dashboard;

  [TestFixture]
  public class DashboardControllerTests : ControllerTestsBase
  {
    private Mock<IGridRunService> _gridRunService;
    private Mock<ISyncService> _syncService;
    private Mock<ILogger> _logger;
    private DashboardController _controller;
    private Mock<IManCoService> _manCoService;
    private Mock<IUserService> _userService;
    private Mock<IHouseHoldingRunService> _houseHoldingRunService;

    [SetUp]
    public void SetUp()
    {
      this._gridRunService = new Mock<IGridRunService>();
      _syncService = new Mock<ISyncService>();
      _logger = new Mock<ILogger>();
      _manCoService = new Mock<IManCoService>();
      _userService = new Mock<IUserService>();
      _houseHoldingRunService = new Mock<IHouseHoldingRunService>();

      var gridRunA = new GridRun { XmlFile = new XmlFile { ManCo = new ManCo(), DocType = new DocType() } };
      var gridRunB = new GridRun { XmlFile = new XmlFile { ManCo = new ManCo(), DocType = new DocType() } };
      var gridRunC = new GridRun { XmlFile = new XmlFile { ManCo = new ManCo(), DocType = new DocType() } };
      var gridRunD = new GridRun { XmlFile = new XmlFile { ManCo = new ManCo(), DocType = new DocType() } };
      var gridRunE = new GridRun { XmlFile = new XmlFile { ManCo = new ManCo(), DocType = new DocType() } };


      this._gridRunService.Setup(x => x.GetProcessing()).Returns(new List<GridRun>() { gridRunA, gridRunB, gridRunC, gridRunD, gridRunE });
      this._gridRunService.Setup(x => x.GetTopFifteenRecentExceptions()).Returns(new List<GridRun>() { gridRunA, gridRunB, gridRunC, gridRunD, gridRunE });

      this._gridRunService.Setup(x => x.GetTopFifteenRecentExceptions(It.IsAny<List<int>>())).Returns(new List<GridRun>() { gridRunA, gridRunB, gridRunC, gridRunD, gridRunE });

      this._gridRunService.Setup(x => x.GetProcessing(It.IsAny<List<int>>())).Returns(new List<GridRun>() { gridRunA, gridRunB, gridRunC, gridRunD, gridRunE });

      var gridRun = new GridRun
                         {
                           EndDate = DateTime.Now,
                           Grid = "grid",
                           IsDebug = false,
                           StartDate = DateTime.Now.AddHours(-1),
                           Status = 2,
                           XmlFile = new XmlFile { DocType = new DocType(), ManCo = new ManCo() },
                         };
      this._gridRunService.Setup(j => j.GetTopFifteenSuccessfullyCompleted()).Returns(new List<GridRun> { gridRun, gridRun, gridRun, gridRun, gridRun });

      this._gridRunService.Setup(j => j.GetTopFifteenSuccessfullyCompleted(It.IsAny<List<int>>())).Returns(new List<GridRun> { gridRun, gridRun, gridRun, gridRun, gridRun });

      _gridRunService.Setup(x => x.GetTopFifteenRecentUnapprovedGrids()).Returns(new List<GridRun> { gridRun, gridRun, gridRun, gridRun, gridRun });

      _gridRunService.Setup(x => x.GetTopFifteenRecentUnapprovedGrids(It.IsAny<List<int>>())).Returns(new List<GridRun> { gridRun, gridRun, gridRun, gridRun, gridRun });

      _gridRunService.Setup(x => x.GetTopFifteenGridsWithRejectedDocuments()).Returns(new List<GridRun> { gridRun, gridRun, gridRun, gridRun, gridRun });

      _gridRunService.Setup(x => x.GetTopFifteenGridsWithRejectedDocuments(It.IsAny<List<int>>())).Returns(new List<GridRun> { gridRun, gridRun, gridRun, gridRun, gridRun });

      _gridRunService.Setup(x => x.GetTopFifteenGridsAwaitingHouseHolding()).Returns(new List<GridRun> { gridRun, gridRun, gridRun, gridRun, gridRun });

      _gridRunService.Setup(x => x.GetTopFifteenGridsAwaitingHouseHolding(It.IsAny<List<int>>())).Returns(new List<GridRun> { gridRun, gridRun, gridRun, gridRun, gridRun });

      _houseHoldingRunService.Setup(x => x.GetTopFifteenRecentHouseHeldGrids()).Returns(new List<HouseHoldingRun>
                                                                                      {
                                                                                        new HouseHoldingRun
                                                                                          {
                                                                                            StartDate = DateTime.Now.AddMinutes(-10),
                                                                                            EndDate = DateTime.Now.AddMinutes(-9),
                                                                                            GridRuns = new Collection<GridRun>()
                                                                                                         {
                                                                                                           new GridRun
                                                                                                             {
                                                                                                               Grid = "grid1",
                                                                                                             },
                                                                                                             new GridRun
                                                                                                               {
                                                                                                                 Grid = "grid2"
                                                                                                               }
                                                                                                         }
                                                                                          }
                                                                                      });

      _houseHoldingRunService.Setup(x => x.GetTopFifteenRecentHouseHeldGrids(It.IsAny<List<int>>())).Returns(new List<HouseHoldingRun>
                                                                                      {
                                                                                        new HouseHoldingRun
                                                                                          {
                                                                                            StartDate = DateTime.Now.AddMinutes(-10),
                                                                                            EndDate = DateTime.Now.AddMinutes(-9),
                                                                                            GridRuns = new Collection<GridRun>()
                                                                                                         {
                                                                                                           new GridRun
                                                                                                             {
                                                                                                               Grid = "grid1",
                                                                                                             },
                                                                                                             new GridRun
                                                                                                               {
                                                                                                                 Grid = "grid2"
                                                                                                               }
                                                                                                         }
                                                                                          }
                                                                                      });

      var documents = new List<Document>
                            {
                                new Document
                                    {
                                        DocumentId = "5731e1fd-ebd2-46bf-9823-6cfc56223257",
                                        ManCo = new ManCo { Code = "199447",Id=199447 },
                                        DocType = new DocType {Code = "doctype code"},
                                        SubDocType = new SubDocType {Code = "subdoctype code"},
                                        
                                    },
                                    new Document
                                    {
                                        DocumentId = "5731e1jk-ebd2-46bf-9823-6cfc56223257",
                                        ManCo = new ManCo { Code = "199447", Id=199447 },
                                        DocType = new DocType {Code = "doctype code"},
                                        SubDocType = new SubDocType {Code = "subdoctype code"}
                                    },
                                    new Document
                                    {
                                        DocumentId = "5731e1fd-ebd2-46bf-9823-6cfc78223257",
                                        ManCo = new ManCo { Code = "199778", Id=199778 },
                                        DocType = new DocType {Code = "doctype code"},
                                        SubDocType = new SubDocType {Code = "subdoctype code2"}
                                    }
                            };

      var manCos = new List<ManCo>
                    { 
                      new ManCo { Code="1", Description="Test ManCo 1", Id=1, Users= new List<ApplicationUserManCo>() },
                      new ManCo { Code="2", Description="Test ManCo 2", Id=2, Users= new List<ApplicationUserManCo>() },
                      new ManCo { Code="3", Description="Test ManCo 3", Id=3, Users= new List<ApplicationUserManCo>() },
                      new ManCo { Code="4", Description="Test ManCo 4", Id=4, Users= new List<ApplicationUserManCo>() },
                      new ManCo { }
                    };

      _manCoService.Setup(m => m.GetManCosByUserId(It.IsAny<string>())).Returns(manCos);

      _controller = new DashboardController(_gridRunService.Object, _syncService.Object, _logger.Object, _manCoService.Object, _userService.Object, _houseHoldingRunService.Object);

      _userService.Setup(x => x.GetApplicationUser()).Returns(new ApplicationUser());
    }

    [Test]
    public void GivenADashboardController_WhenIViewTheDashboardPage_AndThereAreNoFilesProcessing_ThenTheViewModelContainsNoProcessingFiles()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["LastLoggedInDate"]).Returns(null);

      _gridRunService.Setup(x => x.GetProcessing()).Returns(new List<GridRun>());
      var result = _controller.Index(new ManCoFilterViewModel()) as ViewResult;

      var model = result.Model as DashboardViewModel;
      model.DashboardProcessingViewModels.Should().HaveCount(0);
    }

    [Test]
    public void GivenADashboardController_WhenIViewTheDashboardPage_AndThereAreFilesProcessing_ThenTheViewModelContainsProcessingFiles()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["LastLoggedInDate"]).Returns(null);
      
      var result = _controller.Index(new ManCoFilterViewModel()) as ViewResult;

      var model = result.Model as DashboardViewModel;
      model.DashboardProcessingViewModels.Should().HaveCount(5);
    }

    [Test]
    public void GivenADashboardController_WhenIViewTheDashboardPage_AndThereAreFilesProcessing_AndThereIsAllManCoFilter_ThenTheViewModelContainsProcessingFiles()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["LastLoggedInDate"]).Returns(null);
      MockHttpContext.SetupGet(x => x.Session["ManCoFilter"]).Returns(new ManCoFilterViewModel { SelectedManCoId = "1"});

      var result = (ViewResult)_controller.Index(new ManCoFilterViewModel() { SelectedManCoId = "0" });

      _gridRunService.Verify(x => x.GetProcessing(), Times.AtLeastOnce());

      var model = result.Model as DashboardViewModel;
      model.DashboardProcessingViewModels.Should().HaveCount(5);
    }

    [Test]
    public void GivenADashboardController_WhenIViewTheDashboardPage_AndThereAreFilesProcessing_AndTheManCoFilterisAll_ThenTheViewModelContainsProcessingFiles()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["LastLoggedInDate"]).Returns(null);

      var result = (ViewResult)_controller.Index(new ManCoFilterViewModel() { SelectedManCoId = "1" });

      _gridRunService.Verify(x => x.GetProcessing(new List<int> { 1 }), Times.AtLeastOnce());

      var model = result.Model as DashboardViewModel;
      model.DashboardProcessingViewModels.Should().HaveCount(5);
    }

    [Test]
    public void GivenADashboardController_WhenIViewTheDashboardPage_AndThereAreNoRecentlyProcssedFiles_ThenTheViewModelContainsNoProcessingFiles()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["ManCoFilter"]).Returns(null);

      _gridRunService.Setup(x => x.GetProcessing()).Returns(new List<GridRun>());
      var result = _controller.Index(new ManCoFilterViewModel()) as ViewResult;

      _gridRunService.Verify(x => x.GetProcessing(), Times.AtLeastOnce());

      var model = result.Model as DashboardViewModel;
      model.DashboardProcessingViewModels.Should().HaveCount(0);
    }

    [Test]
    public void GivenADashboardController_WhenIViewTheDashboardPage_AndThereAreFilesRecentlyProcessed_ThenTheViewModelContainsProcessedFiles()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["LastLoggedInDate"]).Returns(null);
      
      var result = _controller.Index(new ManCoFilterViewModel()) as ViewResult;

      var model = result.Model as DashboardViewModel;
      model.DashboardProcessedViewModels.Should().HaveCount(5);
    }

    [Test]
    public void GivenADashboardController_WhenIViewTheDashboardPage_AndThereAreFilesProcessed_AndThereIsAManCoFilter_ThenTheViewModelContainsProcessedFiles()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["LastLoggedInDate"]).Returns(null);
     
      var result = (ViewResult)_controller.Index(new ManCoFilterViewModel() { SelectedManCoId = "1" });

      _gridRunService.Verify(x => x.GetTopFifteenSuccessfullyCompleted(new List<int> { 1 }), Times.AtLeastOnce());

      var model = result.Model as DashboardViewModel;
      model.DashboardProcessingViewModels.Should().HaveCount(5);
    }

    [Test]
    public void GivenADashboardController_WhenIViewTheDashboardPage_AndThereAreNoRecentProcssedFiles_ThenTheViewModelContainsNoProcessedFiles()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["ManCoFilter"]).Returns(null);

      _gridRunService.Setup(x => x.GetTopFifteenSuccessfullyCompleted()).Returns(new List<GridRun>());
      var result = _controller.Index(new ManCoFilterViewModel()) as ViewResult;

      _gridRunService.Verify(x => x.GetTopFifteenSuccessfullyCompleted(), Times.AtLeastOnce());

      var model = result.Model as DashboardViewModel;
      model.DashboardProcessedViewModels.Should().HaveCount(0);
    }

    [Test]
    public void GivenADashboardController_WhenIViewTheDashboardPage_AndThereAreRecentExceptions_ThenTheViewModelContainsExceptions()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["LastLoggedInDate"]).Returns(null);
     
      var result = _controller.Index(new ManCoFilterViewModel()) as ViewResult;

      var model = result.Model as DashboardViewModel;
      model.DashboardExceptionViewModels.Should().HaveCount(5);
    }

    [Test]
    public void GivenADashboardController_WhenIViewTheDashboardPage_AndThereAreExceptions_AndThereIsAManCoFilter_ThenTheViewModelContainsProcessedExceptions()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["LastLoggedInDate"]).Returns(null);

      var result = (ViewResult)_controller.Index(new ManCoFilterViewModel() { SelectedManCoId = "1" });

      _gridRunService.Verify(x => x.GetTopFifteenRecentExceptions(new List<int> { 1 }), Times.AtLeastOnce());

      var model = result.Model as DashboardViewModel;
      model.DashboardExceptionViewModels.Should().HaveCount(5);
    }

    [Test]
    public void GivenADashBoardController_WhenTheIndexPageIsAccess_ThenTheIndexVieWModelIsReturned()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["ManCoFilter"]).Returns(null);

      var result = _controller.Index(new ManCoFilterViewModel());
      result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void GivenADashBoardController_WhenTheManCoDropDownPageIsAccess_ThenTheManCoDropDownVieWModelIsReturned()
    {      
      var result = _controller.ManCoDropDown();
      result.Should().BeOfType<PartialViewResult>();
    }

    [Test]
    public void GivenADashBoardController_WhenTheIndexPageIsAccessed_ThenTheIndexViewShouldContainTheModel()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["ManCoFilter"]).Returns(null);

      var result = (ViewResult)_controller.Index(new ManCoFilterViewModel());
      result.Model.Should().BeOfType<DashboardViewModel>();
    }

    [Test]
    public void GivenADashBoardController_WhenTheIndexPageIsAccessed_AndThereIsAManCoFilterInSession_ThenTheIndexViewShouldContainTheModel()
    {
      ManCoFilterViewModel manCoFilterViewModel = new ManCoFilterViewModel { SelectedManCoId = "1" };

      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["ManCoFilter"]).Returns(manCoFilterViewModel);

      var result = (ViewResult)_controller.Index(new ManCoFilterViewModel());
      result.Model.Should().BeOfType<DashboardViewModel>();
    }

    [Test]
    public void GivenADashBoardController_WhenTheIndexPageIsAccessed_AndThereIsAManCoFilterValue_ThenTheIndexViewShouldContainTheModel()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["ManCoFilter"]).Returns(null);

      var result = (ViewResult)_controller.Index(new ManCoFilterViewModel() { SelectedManCoId = "1"});
      result.Model.Should().BeOfType<DashboardViewModel>();
    }

    [Test]
    public void GivenADashBoardController_WhenIViewTheIndexPage_ThenTheViewModelContainsTheCorrectNumberOfProcessingGridRuns()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["ManCoFilter"]).Returns(null);

      var result = _controller.Index(new ManCoFilterViewModel()) as ViewResult;

      var model = result.Model as DashboardViewModel;

      model.DashboardProcessingViewModels.Should().HaveCount(5);
    }

    [Test]
    public void GivenADashBoardController_WhenIViewTheIndexPage_ThenTheViewModelContainsTheCorrectNumberOfRecentlyProcessedGridRuns()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["ManCoFilter"]).Returns(null);

      var result = _controller.Index(new ManCoFilterViewModel()) as ViewResult;

      var model = result.Model as DashboardViewModel;

      model.DashboardProcessedViewModels.Should().HaveCount(5);
    }

    [Test]
    public void GivenADashBoardController_WhenIViewTheIndexPage_ThenTheViewModelContainsTheCorrectNumberOfExceptionGridRuns()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["ManCoFilter"]).Returns(null);

      var result = _controller.Index(new ManCoFilterViewModel()) as ViewResult;

      var model = result.Model as DashboardViewModel;

      model.DashboardExceptionViewModels.Should().HaveCount(5);
    }

    [Test]
    public void GivenADashboardController_WhenIViewTheDashboardPage_AndThereAreGridsAwaitingApproval_AndThereIsAManCoFilter_ThenViewModelContainsGridsAwaitingApproval()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["ManCoFilter"]).Returns(null);

      var result = (ViewResult)_controller.Index(new ManCoFilterViewModel() { SelectedManCoId = "1" });

      var model = result.Model as DashboardViewModel;

      _gridRunService.Verify(x => x.GetTopFifteenRecentUnapprovedGrids(new List<int> { 1 }), Times.AtLeastOnce());

      model.DashboardUnapporvedGridsViewModels.Should().HaveCount(5);
    }

    [Test]
    public void GivenADashboardController_WhenIViewTheDashboardPage_AndThereAreGridsAwaitingApproval_ThenViewModelContainsGridsAwaitingApproval()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["ManCoFilter"]).Returns(null);

      var result = _controller.Index(new ManCoFilterViewModel()) as ViewResult;

      var model = result.Model as DashboardViewModel;

      _gridRunService.Verify(x => x.GetTopFifteenRecentUnapprovedGrids(), Times.AtLeastOnce());

      model.DashboardUnapporvedGridsViewModels.Should().HaveCount(5);
    }

    [Test]
    public void GivenADashboardController_WhenIViewTheDashboardPage_AndThereAreNoGridsAwaitingApproval_ThenViewModelContainsNoDocumentsAwaitingApproval()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["ManCoFilter"]).Returns(null);

      _gridRunService.Setup(x => x.GetTopFifteenRecentUnapprovedGrids()).Returns(new List<GridRun>());

      var result = _controller.Index(new ManCoFilterViewModel()) as ViewResult;

      var model = result.Model as DashboardViewModel;

      _gridRunService.Verify(x => x.GetTopFifteenRecentUnapprovedGrids(), Times.AtLeastOnce());

      model.DashboardUnapporvedGridsViewModels.Should().HaveCount(0);
    }

    [Test]
    public void GivenADashboardController_WhenIViewTheDashboardPage_AndThereAreGridsRejected_ThenViewModelContainsGridsRejected()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["ManCoFilter"]).Returns(null);

      var result = _controller.Index(new ManCoFilterViewModel()) as ViewResult;

      var model = result.Model as DashboardViewModel;

      _gridRunService.Verify(x => x.GetTopFifteenGridsWithRejectedDocuments(), Times.AtLeastOnce());

      model.DashboardRejectedViewModels.Should().HaveCount(5);
    }
    
    [Test]
    public void GivenADashboardController_WhenIViewTheDashboardPage_AndThereAreGridsRejected_AndThereIsAManCoFilter_ThenViewModelContainsGridsRejected()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["ManCoFilter"]).Returns(null);

      var result = (ViewResult)_controller.Index(new ManCoFilterViewModel() { SelectedManCoId = "1" });

      var model = result.Model as DashboardViewModel;

      _gridRunService.Verify(x => x.GetTopFifteenGridsWithRejectedDocuments(new List<int> { 1 }), Times.AtLeastOnce());

      model.DashboardRejectedViewModels.Should().HaveCount(5);
    }

    [Test]
    public void GivenADashboardController_WhenIViewTheDashboardPage_AndThereAreHouseHeldGrids_ThenViewModelContainsHouseHeldGrids()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["ManCoFilter"]).Returns(null);

      var result = _controller.Index(new ManCoFilterViewModel()) as ViewResult;

      var model = result.Model as DashboardViewModel;

      _houseHoldingRunService.Verify(x => x.GetTopFifteenRecentHouseHeldGrids(), Times.AtLeastOnce());

      model.DashboardHouseHeldViewModel.Should().HaveCount(1);
    }

    [Test]
    public void GivenADashboardController_WhenIViewTheDashboardPage_AndThereAreHouseHeldGrids_AndThereIsAManCoFilter_ThenViewModelContainsHouseHeldGrids()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["ManCoFilter"]).Returns(null);

      var result = (ViewResult)_controller.Index(new ManCoFilterViewModel() { SelectedManCoId = "1" });

      var model = result.Model as DashboardViewModel;

      _houseHoldingRunService.Verify(x => x.GetTopFifteenRecentHouseHeldGrids(new List<int> { 1 }), Times.AtLeastOnce());

      model.DashboardHouseHeldViewModel.Should().HaveCount(1);
    }

    [Test]
    public void GivenADashboardController_WhenIViewTheDashboardPage_AndThereAreGridsThatHaveDocsAwaitingHouseholding_ThenViewModelContainsGridsAwaitingHouseHolding()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["ManCoFilter"]).Returns(null);

      var result = _controller.Index(new ManCoFilterViewModel()) as ViewResult;

      var model = result.Model as DashboardViewModel;

      _gridRunService.Verify(x => x.GetTopFifteenGridsAwaitingHouseHolding(), Times.AtLeastOnce());

      model.DashboardGridsAwaitingHouseHoldingViewModel.Should().HaveCount(5);
    }

    [Test]
    public void GivenADashboardController_WhenIViewTheDashboardPage_AndThereAreGridsThatHaveDocsAwaitingHouseholding_AndThereIsAManCoFilter_ThenViewModelContainsGridsAwaitingHouseHolding()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["ManCoFilter"]).Returns(null);

      var result = (ViewResult)_controller.Index(new ManCoFilterViewModel() { SelectedManCoId = "1" });

      var model = result.Model as DashboardViewModel;

      _gridRunService.Verify(x => x.GetTopFifteenGridsAwaitingHouseHolding(new List<int> { 1 }), Times.AtLeastOnce());

      model.DashboardGridsAwaitingHouseHoldingViewModel.Should().HaveCount(5);
    }
  }
}
