namespace EFRepositoryTests
{
  using System.Configuration;
  using System.Transactions;
  using Builder;
  using FluentAssertions;
  using Microsoft.AspNet.Identity.EntityFramework;
  using NUnit.Framework;
  using UnityRepository.Repositories;

  [Category("Integration")]
  [TestFixture]
  public class IdentityRoleRepositoryTests
  {
       
    private TransactionScope _transactionScope;
    private IdentityRoleRepository _identityRoleRepository;
    private IdentityRole _identityRole1;
    private IdentityRole _identityRole2;
    private IdentityRole _identityRole3;

    [SetUp]
    public void Setup()
    {
       _transactionScope = new TransactionScope();
       _identityRoleRepository = new IdentityRoleRepository(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
       _identityRole1 = BuildMeA.IdentityRole("name1");
       _identityRole2 = BuildMeA.IdentityRole("name2");
       _identityRole3 = BuildMeA.IdentityRole("name3");
    }

    [TearDown]
    public void TearDown()
    {
      _transactionScope.Dispose();
    }

    [Test]
    public void WhenITryToRetrieveAllRoles_AllRolesAreRetrieved()
    {
      this._identityRoleRepository.Create(_identityRole1);
      this._identityRoleRepository.Create(_identityRole2);
      this._identityRoleRepository.Create(_identityRole3);

      var identities = this._identityRoleRepository.GetRoles();

      identities.Should().NotBeNull();
      identities.Count.Should().BeGreaterOrEqualTo(3);
    }
  }
}
