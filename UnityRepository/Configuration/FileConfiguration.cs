namespace UnityRepository.Configuration
{
  using System.Data.Entity.ModelConfiguration;
  using Entities.File;

  public class FileConfiguration : EntityTypeConfiguration<InputFile>
  {
    public FileConfiguration()
    {
      this.Map<XmlFile>(x => x.ToTable("XmlFile"))
        .Map<ZipFile>(x => x.ToTable("ZipFile"))
        .Map<ConFile>(x => x.ToTable("ConFile"));
    }
  }
}
