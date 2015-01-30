namespace UnityRepository.Repositories
{
  using System.Collections.Generic;
  using System.Linq;
  using EFRepository;
  using Microsoft.AspNet.Identity.EntityFramework;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class IdentityRoleRepository : BaseEfRepository<IdentityRole>, IIdentityRoleRepository
  {
    public IdentityRoleRepository(string connectionString)
          : base(new UnityDbContext(connectionString))
    {
          
    }

    public IList<IdentityRole> GetRoles()
    {
       return Entities.ToList();
    }
  }
}
