namespace ServiceTests
{
    using System;
    using System.Collections.Generic;

    using Exceptions;

    using FluentAssertions;

    using Microsoft.AspNet.Identity.EntityFramework;

    using Moq;
  using NUnit.Framework;
  using ServiceInterfaces;
  using Services;
  using UnityRepository.Interfaces;

  [TestFixture]
  public class IdentityRoleServiceTests
  {
    private Mock<IIdentityRoleRepository> _identityRoleRepository;
    private IIdentityRoleService _identityRoleService;

    [SetUp]
    public void SetUp()
    {
      _identityRoleRepository = new Mock<IIdentityRoleRepository>();
      _identityRoleService = new IdentityRoleService(_identityRoleRepository.Object);
    }

    [Test]
    public void WhenITryToRetrieveAllIdentityRoles_AndDatabaseIsAvailable_AllIdentityRolesAreRetrieved()
    {
       var listRoles = new List<IdentityRole>();

       listRoles.Add(new IdentityRole("role1"));
       listRoles.Add(new IdentityRole("role2"));
       listRoles.Add(new IdentityRole("role3"));

       this._identityRoleRepository.Setup(x => x.GetRoles()).Returns(listRoles);

       var returnList = this._identityRoleService.GetRoles();

       this._identityRoleRepository.Verify(x => x.GetRoles(), Times.AtLeastOnce);
       returnList.Should().NotBeNull();
       returnList.Count.Should().BeGreaterOrEqualTo(3);
       returnList[1].Name.ShouldBeEquivalentTo(listRoles[1].Name);
    }

    [Test]
    public void WhenITryToRetrieveAllIdentityRoles_AndDatabaseIsUnavailable_AUnityExceptionShouldBeThrown()
    {
       this._identityRoleRepository.Setup(x => x.GetRoles()).Throws<Exception>();
       Action act = () => this._identityRoleService.GetRoles();

       act.ShouldThrow<UnityException>();
    }
  }
}
