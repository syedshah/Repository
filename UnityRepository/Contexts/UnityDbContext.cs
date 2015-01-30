namespace UnityRepository.Contexts
{
  using System.Data.Entity;
  using Entities;
  using Entities.File;
  using Microsoft.AspNet.Identity.EntityFramework;
  using UnityRepository.Configuration;
  using UnityRepository.Initializers;

  public class UnityDbContext : IdentityDbContext<ApplicationUser>
  {
    public UnityDbContext(string connectionString) : base(connectionString)
    {
    }

    public DbSet<InputFile> InputFiles { get; set; }

    public DbSet<XmlFile> XmlFiles { get; set; }

    public DbSet<ZipFile> ZipFiles { get; set; }

    public DbSet<ConFile> ConFiles { get; set; }

    public DbSet<ManCo> Mancos { get; set; }

    public DbSet<DocType> DocTypes { get; set; }

    public DbSet<SubDocType> SubDocTypes { get; set; }

    public DbSet<Domicile> Domicile { get; set; }

    public DbSet<GridRun> GridRuns { get; set; }

    public DbSet<FileSync> OnestepSync { get; set; }

    public DbSet<Application> Applications { get; set; }

    public DbSet<IndexDefinition> IndexDefinitions { get; set; }

    public DbSet<Document> Documents { get; set; }

    public DbSet<AutoApproval> AutoApprovals { get; set; }

    public DbSet<Approval> Approvals { get; set; }

    public DbSet<CartItem> CartItems { get; set; }

    public DbSet<CheckOut> CheckOut { get; set; }

    public DbSet<PasswordHistory> PasswordHistories { get; set; }

    public DbSet<NewsTicker> NewsTicker { get; set; }

    public DbSet<SecurityQuestion> SecurityQuestions { get; set; }

    public DbSet<SecurityAnswer> SecurityAnswers { get; set; }

    public DbSet<GlobalSetting> GlobalSettings { get; set; }

    public DbSet<AppManCoEmail> AppManCoEmails { get; set; }

    public DbSet<HouseHold> HouseHolds { get; set; }

    public DbSet<HouseHoldingRun> HouseHoldingRuns { get; set; }

    public DbSet<Session> Sessions { get; set; }

    public DbSet<AppManCoEmailHistory> AppManCoEmailHistorys { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      Database.SetInitializer(new UnityDbInitializer());

      modelBuilder.Configurations.Add(new FileConfiguration());

      base.OnModelCreating(modelBuilder);
    }
  }
}
