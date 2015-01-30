namespace UnityWebTests.Controllers
{
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Web.Mvc;
  using Entities;
  using FluentAssertions;
  using Logging;
  using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using UnityWeb.Controllers;
  using UnityWeb.Models.Applicaiton;

  [TestFixture]
  public class ApplicationControllerTests : ControllerTestsBase
  {
    private Mock<IApplicationService> _applicationService;
    private Mock<ILogger> _logger;
    private ApplicationController _controller;

    [SetUp]
    public void SetUp()
    {
      _applicationService = new Mock<IApplicationService>();
      _logger = new Mock<ILogger>();

      _applicationService.Setup(x => x.GetApplications()).Returns(new List<Application>()
                                                                    {
                                                                      new Application() { IndexDefinitions = new Collection<IndexDefinition>()},
                                                                      new Application() { IndexDefinitions = new Collection<IndexDefinition>()},
                                                                      new Application() { IndexDefinitions = new Collection<IndexDefinition>()}
                                                                    });

      _controller = new ApplicationController(_applicationService.Object, _logger.Object);
    }


    [Test]
    public void GivenAnApplicationController_WhenTheIndexPageIsAccess_ThenTheIndexVieWModelIsReturned()
    {
      var result = _controller.Index();
      result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void GivenAnApplicationController_WhenTheIndexPageIsAccessed_ThenTheIndexViewShouldContainTheModel()
    {
      var result = (ViewResult)_controller.Index();
      result.Model.Should().BeOfType<ApplicationsViewModel>();
    }

    [Test]
    public void GivenAnApplicationController_WhenIViewTheIndexPage_ThenTheViewModelContainsTheCorrectNumberOfApplicaitons()
    {
      var result = _controller.Index() as ViewResult;

      var model = result.Model as ApplicationsViewModel;

      model.Applicaitons.Should().HaveCount(3);
    }
  } 
}
