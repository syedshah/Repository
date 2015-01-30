namespace Builder
{
  using Entities;

  public class DocumentBuilder : Builder<Document>
  {
    public DocumentBuilder()
    {
      Instance = new Document();
    }

    public DocumentBuilder WithDocType(DocType docType)
    {
      Instance.DocType = docType;
      return this;
    }

    public DocumentBuilder WithSubDocType(SubDocType subDocType)
    {
      Instance.SubDocType = subDocType;
      return this;
    }

    public DocumentBuilder WithManCo(ManCo manCo)
    {
      Instance.ManCo = manCo;
      return this;
    }

    public DocumentBuilder WithApproval(Approval approval)
    {
      Instance.Approval = approval;
      return this;
    }

    public DocumentBuilder WithRejection(Rejection rejection)
    {
      Instance.Rejection = rejection;
      return this;
    }

    public DocumentBuilder WithExport(Export export)
    {
      Instance.Export = export;
      return this;
    }

    public DocumentBuilder WithHouseHeld(HouseHold houseHold)
    {
      Instance.HouseHold = houseHold;
      return this;
    }
  }
}
