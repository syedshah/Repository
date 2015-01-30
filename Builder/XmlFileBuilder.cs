namespace Builder
{
  using Entities;
  using Entities.File;

  public class XmlFileBuilder : Builder<XmlFile>
  {
    public XmlFileBuilder()
    {
      Instance = new XmlFile();
    }

    public XmlFileBuilder WithManCo(ManCo manCo)
    {
      Instance.ManCo = manCo;
      return this;
    }

    public XmlFileBuilder WithDocType(DocType docType)
    {
      Instance.DocType = docType;
      return this;
    }

    public XmlFileBuilder WithDomicile(Domicile domicile)
    {
      Instance.Domicile = domicile;
      return this;
    }

    public XmlFileBuilder WithGridRun(GridRun gridRun)
    {
      if (gridRun != null)
      {
        Instance.GridRuns.Add(gridRun); 
      }
      return this;
    }
  }
}
