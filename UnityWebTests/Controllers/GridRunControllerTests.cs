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
  using UnityWeb.Models.GridRun;

  [TestFixture]
  public class GridRunControllerTests : ControllerTestsBase
  {
    private Mock<IGridRunService> _gridRunService;
    private Mock<IUserService> _userService;
    private GridRunController _controller;
    private Mock<ILogger> _logger;

    [SetUp]
    public void SetUp()
    {
      _gridRunService = new Mock<IGridRunService>();
      _userService = new Mock<IUserService>();
      _logger = new Mock<ILogger>();
      _controller = new GridRunController(_gridRunService.Object,
          _userService.Object,
          _logger.Object);

      this._userService.Setup(x => x.GetUserManCoIds()).Returns(new List<int>());

      _gridRunService.Setup(j => j.Search(string.Empty, It.IsAny<List<int>>())).Returns(_gridRuns);

      _gridRunService.Setup(g => g.GetUnapproved(It.IsAny<int>(), It.IsAny<int>())).Returns(pagedGridRuns);

      _gridRunService.Setup(g => g.GetUnapproved(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>())).Returns(pagedGridRuns);
      
    }

    private static readonly List<GridRun> _gridRuns = new List<GridRun>
                                                {
                                                    new GridRun { XmlFile = new XmlFile { DocType  = new DocType(), ManCo = new ManCo() }, Application = new Application() },
                                                    new GridRun { XmlFile = new XmlFile { DocType = new DocType(), ManCo = new ManCo() }, Application = new Application() },
                                                    new GridRun { XmlFile = new XmlFile { DocType = new DocType(), ManCo = new ManCo() }, Application = new Application() },
                                                    new GridRun { XmlFile = new XmlFile { DocType = new DocType(), ManCo = new ManCo() }, Application = new Application() }
                                                };

    private readonly PagedResult<GridRun> pagedGridRuns = new PagedResult<GridRun>
    {
      CurrentPage = 1,
      ItemsPerPage = 2,
      Results = _gridRuns,
      TotalItems = _gridRuns.Count
    };
   

    [Test]
    public void GivenAGridRunController_WhenICallItsSearchMethod_ThenItRetrunsTheCorrectNumberOfGridRuns()
    {
      var result = (ViewResult)_controller.Search(string.Empty);

      var model = (GridRunSearchesViewModel)result.Model;

      model.GridRuns.Should().HaveCount(_gridRuns.Count);

      this._userService.Verify(x => x.GetUserManCoIds(), Times.AtLeastOnce);
    }

    [Test]
    public void GivenAGridRunController_WhenICallItsSearchMethod_ThenItReturnsTheCorrectView()
    {
      var result = (ViewResult)_controller.Search(string.Empty);
      result.Model.Should().BeOfType<GridRunSearchesViewModel>();

      this._userService.Verify(x => x.GetUserManCoIds(), Times.AtLeastOnce);
    }

    [Test]
    public void GivenAGridRunController_WhenICallItsUnapprovedAction_ThenItReturnsTheCorrectView()
    {
      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["ManCoFilter"]).Returns(null);

      var result = (ViewResult)_controller.Unapproved();

      var model = (GridRunsDetailViewModel)result.Model;

      model.GridRuns.Should().HaveCount(_gridRuns.Count);
      model.PagingInfo.CurrentPage.Should().Be(1);
      model.PagingInfo.ItemsPerPage.Should().Be(2);
      model.PagingInfo.TotalItems.Should().Be(4);
      model.PagingInfo.TotalPages.Should().Be(2);
    }

    [Test]
    public void GivenAGridRunController_WhenICallItsUnapprovedAction_AndThereIsAManCoFilter_ThenItReturnsTheCorrectView()
    {
      var manCoFilterViewModel = new ManCoFilterViewModel { SelectedManCoId = "1" };

      SetControllerContext(_controller);
      MockHttpContext.SetupGet(x => x.Session["ManCoFilter"]).Returns(manCoFilterViewModel);

      var result = (ViewResult)_controller.Unapproved();

      var model = (GridRunsDetailViewModel)result.Model;

      model.GridRuns.Should().HaveCount(_gridRuns.Count);
      model.PagingInfo.CurrentPage.Should().Be(1);
      model.PagingInfo.ItemsPerPage.Should().Be(2);
      model.PagingInfo.TotalItems.Should().Be(4);
      model.PagingInfo.TotalPages.Should().Be(2);
    }

    [Test]
    public void GivenValidData_WhenIAskForGridStatus_ThenIGetTheCorrectView()
    {
      DateTime recieved = DateTime.Now.AddDays(-1);
      DateTime endDate = DateTime.Now.AddHours(-1);

      _gridRunService.Setup(g => g.GetGridRun(1)).Returns(new GridRun
                                                            {
                                                              XmlFile = new XmlFile
                                                                          {
                                                                            Received = recieved
                                                                          },
                                                                          EndDate = endDate
                                                            });

      var result = (PartialViewResult)_controller.Status(1);
      var model = (GridRunStatusViewModel)result.Model;

      model.Arrived.Should().Be(recieved.ToString("ddd d MMM yyyy HH:mm"));
      model.Proccessed.Should().Be(endDate.ToString("ddd d MMM yyyy HH:mm"));
      result.Should().BeOfType<PartialViewResult>();
      result.ViewName.Should().Be("_GridRunStatus");
    }

    [Test]
    public void GivenAFullyApprovedGrid_WhenIAskforGridStatus_ThenIGetThecorrectView()
    {
      DateTime recieved = DateTime.Now.AddDays(-1);
      DateTime endDate = DateTime.Now.AddHours(-1);

      _gridRunService.Setup(g => g.GetGridRun(1)).Returns(new GridRun
                                                            {
                                                              XmlFile = new XmlFile
                                                              {
                                                                Received = recieved
                                                              },
                                                              EndDate = endDate,
                                                              Documents = new Collection<Document>()
                                                                            {
                                                                              new Document
                                                                                {
                                                                                  Approval = new Approval
                                                                                               {
                                                                                                 ApprovedDate = DateTime.Now.AddSeconds(-3),
                                                                                                 ApprovedBy = "a"
                                                                                               }
                                                                                },
                                                                                new Document
                                                                                  {
                                                                                    Approval = new Approval
                                                                                              {
                                                                                                 ApprovedDate = DateTime.Now.AddSeconds(-2),
                                                                                                 ApprovedBy = "b"
                                                                                               }
                                                                                  },
                                                                                  new Document
                                                                                    {
                                                                                      Approval = new Approval
                                                                                      {
                                                                                                 ApprovedDate = DateTime.Now.AddSeconds(-1),
                                                                                                 ApprovedBy = "c"
                                                                                               }
                                                                                    }
                                                                            }
                                                            });

      var result = (PartialViewResult)_controller.Status(1);
      var model = (GridRunStatusViewModel)result.Model;

      model.ApprovalStatus.Should().Be((int)GridRunStatusViewModel.ApprovalStatuses.FullyApproved);
      model.ApprovedBy.Should().Be("c");
      model.HouseHoldStatus.Should().Be((int)GridRunStatusViewModel.HouseHoldingStatuses.NonHouseHeld);
      result.ViewName.Should().Be("_GridRunStatus");
    }

    [Test]
    public void GivenAFullyUnApprovedGrid_WhenIAskforGridStatus_ThenIGetThecorrectView()
    {
      DateTime recieved = DateTime.Now.AddDays(-1);
      DateTime endDate = DateTime.Now.AddHours(-1);

      _gridRunService.Setup(g => g.GetGridRun(1)).Returns(new GridRun
                                                            {
                                                              XmlFile = new XmlFile
                                                              {
                                                                Received = recieved
                                                              },
                                                              EndDate = endDate,
                                                              Documents = new Collection<Document>()
                                                                            {
                                                                              new Document (),
                                                                              new Document (),
                                                                              new Document ()
                                                                            }
      });

      var result = (PartialViewResult)_controller.Status(1);
      var model = (GridRunStatusViewModel)result.Model;

      model.ApprovalStatus.Should().Be((int)GridRunStatusViewModel.ApprovalStatuses.Unapproved);
      model.HouseHoldStatus.Should().Be((int)GridRunStatusViewModel.HouseHoldingStatuses.NonHouseHeld);
      result.ViewName.Should().Be("_GridRunStatus");
    }

    [Test]
    public void GivenAPartiallyApprovedGrid_WhenIAskforGridStatus_ThenIGetThecorrectView()
    {
      DateTime recieved = DateTime.Now.AddDays(-1);
      DateTime endDate = DateTime.Now.AddHours(-1);
      DateTime approvalDate = DateTime.Now;

      _gridRunService.Setup(g => g.GetGridRun(1)).Returns(new GridRun
                                                        {
                                                          XmlFile = new XmlFile
                                                          {
                                                            Received = recieved
                                                          },
                                                          EndDate = endDate,
                                                          Documents = new Collection<Document>()
                                                                            {
                                                                              new Document 
                                                                                {
                                                                                  Approval = new Approval
                                                                                               {
                                                                                                 ApprovedDate = approvalDate,
                                                                                                 ApprovedBy = "d"
                                                                                               }
                                                                                },
                                                                              new Document
                                                                                {
                                                                                  Rejection = new Rejection()
                                                                                },
                                                                              new Document ()
                                                                            }
                                                           });

      var result = (PartialViewResult)_controller.Status(1);
      var model = (GridRunStatusViewModel)result.Model;

      model.ApprovalStatus.Should().Be((int)GridRunStatusViewModel.ApprovalStatuses.PartiallyApproved);
      model.ApprovedBy.Should().Be("d");
      model.ApprovalDate.Should().Be(approvalDate.ToString("ddd d MMM yyyy HH:mm"));
      model.HouseHoldStatus.Should().Be((int)GridRunStatusViewModel.HouseHoldingStatuses.NonHouseHeld);
      result.ViewName.Should().Be("_GridRunStatus");
    }

    [Test]
    public void GivenAFullyRejectedGrid_WhenIAskforGridStatus_ThenIGetThecorrectView()
    {
      DateTime recieved = DateTime.Now.AddDays(-1);
      DateTime endDate = DateTime.Now.AddHours(-1);
      DateTime rejectionDate = DateTime.Now.AddSeconds(-10);
      DateTime rejectionDate1 = DateTime.Now.AddSeconds(-5);
      DateTime rejectionDate2 = DateTime.Now.AddSeconds(-1);

      _gridRunService.Setup(g => g.GetGridRun(1)).Returns(new GridRun
                                                           {
                                                              XmlFile = new XmlFile
                                                              {
                                                                Received = recieved
                                                              },
                                                              EndDate = endDate,
                                                              Documents = new Collection<Document>()
                                                                            {
                                                                              new Document 
                                                                                {
                                                                                  Rejection = new Rejection
                                                                                                {
                                                                                                  RejectedBy = "x",
                                                                                                  RejectionDate = rejectionDate
                                                                                                }
                                                                                },
                                                                              new Document
                                                                                {
                                                                                  Rejection = new Rejection
                                                                                                {
                                                                                                  RejectedBy = "y",
                                                                                                  RejectionDate = rejectionDate1
                                                                                                }
                                                                                },
                                                                              new Document 
                                                                                {
                                                                                  Rejection = new Rejection
                                                                                                {
                                                                                                  RejectedBy = "z",
                                                                                                  RejectionDate = rejectionDate2
                                                                                                }
                                                                                }
                                                                            }
                                                                });

      var result = (PartialViewResult)_controller.Status(1);
      var model = (GridRunStatusViewModel)result.Model;

      model.ApprovalStatus.Should().Be((int)GridRunStatusViewModel.ApprovalStatuses.FullyRejected);
      model.RejectedBy.Should().Be("z");
      model.RejectedDate.Should().Be(rejectionDate2.ToString("ddd d MMM yyyy HH:mm"));
      model.HouseHoldStatus.Should().Be((int)GridRunStatusViewModel.HouseHoldingStatuses.NonHouseHeld);
      result.ViewName.Should().Be("_GridRunStatus");
    }

    [Test]
    public void GivenAFullyHouseHeldGrid_WhenIAskforGridStatus_ThenIGetThecorrectView()
    {
      DateTime recieved = DateTime.Now.AddDays(-1);
      DateTime endDate = DateTime.Now.AddHours(-1);
      DateTime approvalnDate = DateTime.Now;
      DateTime approvalnDate1 = DateTime.Now.AddMinutes(-10);
      DateTime approvalnDate2 = DateTime.Now.AddMinutes(-20);

      _gridRunService.Setup(g => g.GetGridRun(1)).Returns(new GridRun
      {
        XmlFile = new XmlFile
        {
          Received = recieved
        },
        EndDate = endDate,
        Documents = new Collection<Document>()
                                                                            {
                                                                              new Document
                                                                                {
                                                                                  HouseHold = new HouseHold(),
                                                                                  Approval = new Approval
                                                                                               {
                                                                                                 ApprovedBy = "p",
                                                                                                 ApprovedDate = approvalnDate
                                                                                               }
                                                                                },
                                                                                new Document
                                                                                  {
                                                                                    HouseHold = new HouseHold(),
                                                                                    Approval = new Approval
                                                                                                 {
                                                                                                   ApprovedBy = "q",
                                                                                                   ApprovedDate = approvalnDate1
                                                                                                 }
                                                                                  },
                                                                                  new Document
                                                                                    {
                                                                                      HouseHold = new HouseHold(),
                                                                                      Approval = new Approval
                                                                                                   {
                                                                                                     ApprovedBy = "r",
                                                                                                     ApprovedDate = approvalnDate2
                                                                                                   }
                                                                                    }
                                                                            }
      });

      var result = (PartialViewResult)_controller.Status(1);
      var model = (GridRunStatusViewModel)result.Model;

      model.ApprovalStatus.Should().Be((int)GridRunStatusViewModel.ApprovalStatuses.FullyApproved);
      model.ApprovedBy.Should().Be("p");
      model.ApprovalDate.Should().Be(approvalnDate.ToString("ddd d MMM yyyy HH:mm"));
      model.HouseHoldStatus.Should().Be((int)GridRunStatusViewModel.HouseHoldingStatuses.FullyHouseHeld);
      result.ViewName.Should().Be("_GridRunStatus");
    }

    [Test]
    public void GivenAPartiallyHouseHeld_WhenIAskforGridStatus_ThenIGetThecorrectView()
    {
      DateTime recieved = DateTime.Now.AddDays(-1);
      DateTime endDate = DateTime.Now.AddHours(-1);
      DateTime approvalnDate = DateTime.Now.AddSeconds(-10);
      DateTime approvalnDate1 = DateTime.Now.AddSeconds(-1);

      _gridRunService.Setup(g => g.GetGridRun(1)).Returns(new GridRun
      {
        XmlFile = new XmlFile
        {
          Received = recieved
        },
        EndDate = endDate,
        Documents = new Collection<Document>()
                                                                            {
                                                                              new Document 
                                                                                {
                                                                                  HouseHold = new HouseHold(),
                                                                                  Approval = new Approval
                                                                                               {
                                                                                                 ApprovedBy = "m",
                                                                                                 ApprovedDate = approvalnDate
                                                                                               }
                                                                                },
                                                                              new Document
                                                                                {
                                                                                  HouseHold = new HouseHold(),
                                                                                  Approval = new Approval
                                                                                                {
                                                                                                 ApprovedBy = "n",
                                                                                                 ApprovedDate = approvalnDate1
                                                                                               }
                                                                                },
                                                                              new Document ()
                                                                            }
      });

      var result = (PartialViewResult)_controller.Status(1);
      var model = (GridRunStatusViewModel)result.Model;

      model.ApprovalStatus.Should().Be((int)GridRunStatusViewModel.ApprovalStatuses.PartiallyApproved);
      model.ApprovedBy.Should().Be("n");
      model.ApprovalDate.Should().Be(approvalnDate1.ToString("ddd d MMM yyyy HH:mm"));
      model.HouseHoldStatus.Should().Be((int)GridRunStatusViewModel.HouseHoldingStatuses.PartiallyHouseHeld);
      result.ViewName.Should().Be("_GridRunStatus");
    }
  }
}
