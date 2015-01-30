namespace UnityWebTests.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Web.Mvc;
  using Entities;
  using FluentAssertions;
  using Logging;
  using Microsoft.AspNet.Identity.EntityFramework;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using UnityWeb.Controllers;
  using UnityWeb.Models.KpiReport;
  using UnityWeb.Models.UserReport;

  [TestFixture]
  public class KpiReportsControllerTests
  {
    private KpiReportsController _controller;
    private Mock<ILogger> _logger;
    private Mock<IDocumentService> _documentService;
    private Mock<IManCoService> _manCoService;
    private Mock<IUserService> _userService;
    private Mock<ApplicationUser> user1 = new Mock<ApplicationUser>();
    private Mock<ApplicationUser> user2 = new Mock<ApplicationUser>();
    private Mock<ApplicationUser> user3 = new Mock<ApplicationUser>();
    private PagedResult<ApplicationUser> users;
    private KpiReportViewModel _kpiReportViewModel;

    [SetUp]
    public void SetUp()
    {
      _logger = new Mock<ILogger>();
      _documentService = new Mock<IDocumentService>();
      _manCoService = new Mock<IManCoService>();
      _userService = new Mock<IUserService>();
      _controller = new KpiReportsController(_documentService.Object, _manCoService.Object, _userService.Object, _logger.Object);

      _userService.Setup(m => m.GetApplicationUser()).Returns(new ApplicationUser() { Id = "1" });
      _manCoService.Setup(m => m.GetManCosByUserId("1")).Returns(new List<ManCo> { new ManCo { } });

      _kpiReportViewModel = new KpiReportViewModel
                              {
                                DateFrom = DateTime.Now.AddMonths(-1),
                                DateTo = DateTime.Now,
                                SelectedManCoId = "1"
                              };
    }

    [Test]
    public void GivenAKpiReportsController_WhenICallItsIndexMethod_ThenItReturnsTheCorrectView()
    {
      var result = (ViewResult)_controller.Index();

      var model = (KpiReportViewModel)result.Model;

      model.ManCos.Count.Should().Be(1);
    }

    [Test]
    public void GivenKpiReportsController_WhenICallItsRunMethod_AndThereAreModelErrors_ThenTheUserIsNotified()
    {
      _controller.ModelState.AddModelError("error", "message");

      var result = (ViewResult)_controller.Run("Run", _kpiReportViewModel);

      _controller.TempData.Should().NotBeNull();
      var errorList = _controller.ModelState.Values.SelectMany(x => x.Errors).ToList();
      _controller.TempData["kpierror"].Should().Be(errorList[0].ErrorMessage);
    }

    [Test]
    public void GivenKpiReportsController_WhenICallItsRunMethod_ThenItReturnsTheCorrectView()
    {
      _documentService.Setup(d => d.GetDocuments(Convert.ToInt32(_kpiReportViewModel.SelectedManCoId), _kpiReportViewModel.DateFrom.Value, _kpiReportViewModel.DateTo.Value)).Returns(new List<KpiReportData>(){ new KpiReportData()
                                                                                                                                                                                                                   {
                                                                                                                                                                                                                     ManCo = new ManCo(),
                                                                                                                                                                                                                     DocType = "DocType",
                                                                                                                                                                                                                     SubDocType = "SubDocType",
                                                                                                                                                                                                                     NumberOfDocs = 23
                                                                                                                                                                                                                   } });

      var result = (ViewResult)_controller.Run("Run", _kpiReportViewModel);

      var model = (KpiReportsDataViewModel)result.Model;

      model.KpiReport.Count.Should().Be(1);
    }

    [Test]
    public void GivenAKpiReportsController_WhenICallItsExportMethod_ThenItReturnsTheCorrectView()
    {
      _documentService.Setup(d => d.GetDocuments(Convert.ToInt32(_kpiReportViewModel.SelectedManCoId), _kpiReportViewModel.DateFrom.Value, _kpiReportViewModel.DateTo.Value)).Returns(new List<KpiReportData>(){ new KpiReportData()
                                                                                                                                                                                                                   {
                                                                                                                                                                                                                     ManCo = new ManCo(),
                                                                                                                                                                                                                     DocType = "DocType",
                                                                                                                                                                                                                     SubDocType = "SubDocType",
                                                                                                                                                                                                                     NumberOfDocs = 23
                                                                                                                                                                                                                   } });

      var result = (FileContentResult)_controller.Run("Export", _kpiReportViewModel);

      result.Should().BeOfType<FileContentResult>();
    }

    [Test]
    public void GivenAKpiReportsController_WhenICallItsExportMethod_AndThereAreNoDocumentsReturned_ThenIGetAMessageSayingNoDataWasFound()
    {
      _documentService.Setup(d => d.GetDocuments(Convert.ToInt32(_kpiReportViewModel.SelectedManCoId), _kpiReportViewModel.DateFrom.Value, _kpiReportViewModel.DateTo.Value)).Returns(new List<KpiReportData>());

      var result = (ViewResult)_controller.Run("Run", _kpiReportViewModel);

      var model = (KpiReportsDataViewModel)result.Model;

      model.KpiReport.Count.Should().Be(0);
      _controller.TempData["noData"].Should().Be("There were no documents found for your search criteria.");
    }
  }
}
