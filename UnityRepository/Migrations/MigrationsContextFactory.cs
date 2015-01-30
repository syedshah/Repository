namespace UnityRepository.Migrations
{
    using System.Configuration;
    using System.Data.Entity.Infrastructure;
    using UnityRepository.Contexts;

    public class MigrationsContextFactory : IDbContextFactory<UnityDbContext>
    {
        public UnityDbContext Create()
        {
            return new UnityDbContext(ConfigurationManager.ConnectionStrings["Unity"].ConnectionString);
        }
    }
}
