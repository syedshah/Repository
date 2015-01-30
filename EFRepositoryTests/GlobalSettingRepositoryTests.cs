namespace EFRepositoryTests
{
  using System.Configuration;
  using System.Transactions;
  using Builder;
  using Entities;
  using FluentAssertions;
  using NUnit.Framework;
  using UnityRepository.Repositories;

  [Category("Integration")]
  [TestFixture]
  public class GlobalSettingRepositoryTests
  {
    private TransactionScope _transactionScope;
    private GlobalSettingRepository _globalSettingRepository;
    private GlobalSetting _globalSetting;

    [SetUp]
    public void SetUp()
    {
      _transactionScope = new TransactionScope();
      _globalSettingRepository = new GlobalSettingRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
      _globalSetting = BuildMeA.GlobalSetting(5, 0, 30, 12, false);
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    }

    [Test]
    public void WhenIWantToRetrieveGlobalSetting_ItIsRetrievedFromTheDatabase()
    {
      this._globalSettingRepository.Create(_globalSetting);

      var result = this._globalSettingRepository.Get();

      result.Should().NotBeNull();
    }
  }
}
