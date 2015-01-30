namespace EFRepositoryTests
{
  using System.Configuration;
  using System.Linq;
  using System.Transactions;
  using Builder;
  using Entities;
  using Exceptions;
  using FluentAssertions;
  using NUnit.Framework;
  using UnityRepository.Repositories;

  [Category("Integration")]
  [TestFixture]
  public class ManCoRepositoryTests
  {
    [SetUp]
    public void Setup()
    {
      _transactionScope = new TransactionScope();
      _manCoRepository = new ManCoRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _domicileRepository = new DomicileRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _applicationUserRepository = new ApplicationUserRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _domicile = BuildMeA.Domicile("code", "description");
      _domicile2 = BuildMeA.Domicile("code2", "description2");
      _manCo1 = BuildMeA.ManCo("description", "code").WithDomicile(_domicile);
      _manCo2 = BuildMeA.ManCo("description2", "code2").WithDomicile(_domicile);
      _manCo3 = BuildMeA.ManCo("description3", "code3").WithDomicile(_domicile);
      _manCo4 = BuildMeA.ManCo("description4", "code4").WithDomicile(_domicile2);
      _user = BuildMeA.ApplicationUser("warrior");
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    }

    private TransactionScope _transactionScope;
    private ManCoRepository _manCoRepository;
    private DomicileRepository _domicileRepository;
    private ApplicationUserRepository _applicationUserRepository;
    private ManCo _manCo1;
    private ManCo _manCo2;
    private ManCo _manCo3;
    private ManCo _manCo4;
    private Domicile _domicile;
    private Domicile _domicile2;
    private ApplicationUser _user;

    [Test]
    public void GivenAManCo_WhenITryToSaveToTheDatabase_ItIsSavedToTheDatabase()
    {
      int initialCount = _manCoRepository.Entities.Count();
      _manCoRepository.Create(_manCo1);

      _manCoRepository.Entities.Count().Should().Be(initialCount + 1);
    }

    [Test]
    public void GivenAnDocType_WhenITryToSearchForItById_ItIsRetrievedFromTheDatabase()
    {
      _manCoRepository.Create(_manCo1);
      var result = _manCoRepository.GetManCo(_manCo1.Id);

      result.Should().NotBeNull();
      result.Description.Should().Be("description");
    }

    [Test]
    public void WhenIUpdateAManCo_AndTheManCoDoesNotExist_ThenAnExceptionIsThrown()
    {
      _manCoRepository.Create(_manCo1);

      ManCo manCo = _manCoRepository.Entities.Where(p => p.Code == "code").FirstOrDefault();
      Assert.Throws<UnityException>(() => _manCoRepository.Update(manCo.Id + 1001, "code 1", "description name 1"));
    }

    [Test]
    public void WhenIUpdateADocType_ThenTheDocTypeIsUpdated()
    {
      _manCoRepository.Create(_manCo1);

      ManCo manCo = _manCoRepository.Entities.Where(p => p.Code == "code").FirstOrDefault();
      _manCoRepository.Update(manCo.Id, "new code", "description 1");
      manCo = _manCoRepository.Entities.Where(p => p.Code == "new code").FirstOrDefault();

      manCo.Should().NotBeNull();
    }

    [Test]
    public void GivenADomicileId_WhenITryToRetrieveManCosByDomicileId_TheManCosAreRetrieved()
    {
        _domicileRepository.Create(_domicile);
        _domicileRepository.Create(_domicile2);

        _manCo1.DomicileId = _domicile.Id;
        _manCo2.DomicileId = _domicile.Id;
        _manCo3.DomicileId = _domicile.Id;
        _manCo4.DomicileId = _domicile2.Id;

        _manCoRepository.Create(_manCo1);
        _manCoRepository.Create(_manCo2);
        _manCoRepository.Create(_manCo3);
        _manCoRepository.Create(_manCo4);

        var manCos = _manCoRepository.GetManCos(_domicile.Id);

        manCos.Should().NotBeNull();
        manCos.Count().Should().Be(3);
        manCos.ElementAt(1).DomicileId.Should().Be(_domicile.Id);
        manCos.ElementAt(1).DomicileId.Should().Be(manCos.ElementAt(2).DomicileId);
        manCos.ElementAt(2).DomicileId.Should().NotBe(_domicile2.Id);
    }

    [Test]
    public void GivenAUserId_WhenITryToRetrieveManCosByUserId_TheManCosAreRetrieved()
    {
      _manCoRepository.Create(_manCo1);
      _manCoRepository.Create(_manCo2);
      _manCoRepository.Create(_manCo3);
      _manCoRepository.Create(_manCo4);

      _user.ManCos.Add(new ApplicationUserManCo { UserId = _user.Id, ManCoId = _manCo1.Id});
      _user.ManCos.Add(new ApplicationUserManCo { UserId = _user.Id, ManCoId = _manCo2.Id });
      _user.ManCos.Add(new ApplicationUserManCo { UserId = _user.Id, ManCoId = _manCo3.Id });
      _user.ManCos.Add(new ApplicationUserManCo { UserId = _user.Id, ManCoId = _manCo4.Id });

      this._applicationUserRepository.Create(_user);

      var manCosResult = this._manCoRepository.GetManCosByUserId(_user.Id);

      manCosResult.Should().NotBeNull();
      manCosResult.Count().Should().Be(4);
    }
  }
}
