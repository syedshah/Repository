namespace Builder
{
  using System;

  public class BuildMeA
  {
    public static ZipFileBuilder ZipFile(
      bool bigZip, string documentSetId, string fileName, string parentFileName, DateTime received)
    {
      return (ZipFileBuilder)new ZipFileBuilder().With(
        z =>
          {
            z.BigZip = bigZip;
            z.DocumentSetId = documentSetId;
            z.FileName = fileName;
            z.ParentFileName = parentFileName;
            z.Received = received;
          });
    }

    public static XmlFileBuilder XmlFile(
      string documentSetId, string fileName, string parentFileName, bool offshore, DateTime received, DateTime allocated)
    {
      return (XmlFileBuilder)new XmlFileBuilder().With(
        z =>
          {
            z.DocumentSetId = documentSetId;
            z.FileName = fileName;
            z.ParentFileName = parentFileName;
            z.OffShore = offshore;
            z.Received = received;
            z.Allocated = allocated;
          });
    }

    public static ManCoBuilder ManCo(string description, string code)
    {
      return (ManCoBuilder)new ManCoBuilder().With(
        m =>
          {
            m.Description = description;
            m.Code = code;
          });
    }

    public static DocTypeBuilder DocType(string code, string description)
    {
      return (DocTypeBuilder)new DocTypeBuilder().With(
        d =>
          {
            d.Code = code;
            d.Description = description;
          });
    }

    public static AppManCoEmailBuilder AppManCoEmail(
      int appId, int manCoId, int docTypeId, string accountNumber, string email)
    {
      return (AppManCoEmailBuilder)new AppManCoEmailBuilder().With(
        d =>
          {
            d.ApplicationId = appId;
            d.ManCoId = manCoId;
            d.DocTypeId = docTypeId;
            d.AccountNumber = accountNumber;
            d.Email = email;
          });
    }

    public static IdentityRoleBuilder IdentityRole(string rolename)
    {
      return (IdentityRoleBuilder)new IdentityRoleBuilder().With(
        d =>
          {
            d.Name = rolename;
          });
    }

    public static SubDocTypeBuilder SubDocType(string code, string description)
    {
      return (SubDocTypeBuilder)new SubDocTypeBuilder().With(
        d =>
          {
            d.Code = code;
            d.Description = description;
          });
    }

    public static AutoApprovalBuilder AutoApproval()
    {
      return (AutoApprovalBuilder)new AutoApprovalBuilder().With(
        d =>
          {
            
          });
    }

    public static DomicileBuilder Domicile(string code, string description)
    {
      return (DomicileBuilder)new DomicileBuilder().With(
        d =>
          {
            d.Code = code;
            d.Description = description;
          });
    }

    public static SecurityQuestionBuilder SecurityQuestion(string question)
    {
      return (SecurityQuestionBuilder)new SecurityQuestionBuilder().With(
        d =>
          {
            d.Question = question;
          });
    }

    public static SecurityAnswerBuilder SecurityAnswer(string answer)
    {
      return (SecurityAnswerBuilder)new SecurityAnswerBuilder().With(
        d =>
          {
            d.Answer = answer;
          });
    }

    public static GridRunBuilder GridRun(string grid, bool isDebug, DateTime startDate, DateTime? enddate, int status)
    {
      return (GridRunBuilder)new GridRunBuilder().With(
        j =>
          {
            j.Grid = grid;
            j.IsDebug = isDebug;
            j.StartDate = startDate;
            j.EndDate = enddate;
            j.Status = status;
          });
    }

    public static ConFileBuilder ZipFile(
      string documentSetId, string fileName, string parentFileName, DateTime received)
    {
      return (ConFileBuilder)new ConFileBuilder().With(
        z =>
          {
            z.DocumentSetId = documentSetId;
            z.FileName = fileName;
            z.ParentFileName = parentFileName;
            z.Received = received;
          });
    }

    public static OneStepSyncBuilder OneStepSync(int gridRunId)
    {
      return (OneStepSyncBuilder)new OneStepSyncBuilder().With(
        o =>
          {
            o.GridRunId = gridRunId;
          });
    }

    public static ApplicationBuilder Application(string code, string description)
    {
      return (ApplicationBuilder)new ApplicationBuilder().With(
        o =>
          {
            o.Code = code;
            o.Description = description;
          });
    }

    public static ApplicationUserBuilder ApplicationUser(string userName)
    {
      return (ApplicationUserBuilder)new ApplicationUserBuilder().With(
        o =>
          {
            o.UserName = userName;
          });
    }

    public static PasswordHistoryBuilder PasswordHistory(string userId, string passwordHash)
    {
      return (PasswordHistoryBuilder)new PasswordHistoryBuilder().With(
        p =>
          {
            p.UserId = userId;
            p.PasswordHash = passwordHash;
            p.LogDate = DateTime.Now;
          });
    }

    public static IndexBuilder Index(string name, string archiveColumn)
    {
      return (IndexBuilder)new IndexBuilder().With(
        i =>
          {
            i.Name = name;
            i.ArchiveColumn = archiveColumn;
          });
    }

    public static DocumentBuilder Document(string documentId)
    {
      return (DocumentBuilder)new DocumentBuilder().With(
        d =>
          {
            d.DocumentId = documentId;
          });
    }

    public static GlobalSettingBuilder GlobalSetting(
      int minPasswordLength, int minNonAlphaChars, int passwordExpDays, int passChangeBefore, bool newUserPasswordReset)
    {
      return (GlobalSettingBuilder)new GlobalSettingBuilder().With(
        g =>
          {
            g.MinPasswordLength = minPasswordLength;
            g.MinNonAlphaChars = minNonAlphaChars;
            g.PasswordExpDays = passwordExpDays;
            g.PassChangeBefore = passChangeBefore;
            g.NewUserPasswordReset = newUserPasswordReset;
          });
    }

    public static CartItemBuilder CartItem(string cartId)
    {
      return (CartItemBuilder)new CartItemBuilder(cartId).With(
        p =>
          {
            p.CartId = cartId;
          });
    }

    public static CheckOutBuilder CheckOut(string checkOutBy, DateTime? checkOutDate)
    {
      return (CheckOutBuilder)new CheckOutBuilder().With(
        p =>
          {
            p.CheckOutBy = checkOutBy;
            p.CheckOutDate = checkOutDate;
          });
    }

    public static ApprovalBuilder Approval(string approvedBy, DateTime approvedDate)
    {
      return (ApprovalBuilder)new ApprovalBuilder().With(
        p =>
          {
            p.ApprovedBy = approvedBy;
            p.ApprovedDate = approvedDate;
          });
    }

    public static RejectionBuilder Rejection(string rejectedBy, DateTime approvedDate)
    {
      return (RejectionBuilder)new RejectionBuilder().With(
        p =>
          {
            p.RejectedBy = rejectedBy;
            p.RejectionDate = approvedDate;
          });
    }

    public static ExportBuilder Export(DateTime exportDate)
    {
      return (ExportBuilder)new ExportBuilder().With(
        p =>
          {
            p.ExportDate = exportDate;
          });
    }

    public static NewsTickerBuilder NewsTicker(string news, DateTime date)
    {
      return (NewsTickerBuilder)new NewsTickerBuilder().With(
        d =>
          {
            d.News = news;
            d.Date = date;
          });
    }

    public static HouseHoldingRunBuilder HouseHoldingRun(DateTime startDate, DateTime endDate, string grid)
    {
      return (HouseHoldingRunBuilder)new HouseHoldingRunBuilder().With(
        z =>
          {
            z.StartDate = startDate;
            z.EndDate = endDate;
            z.Grid = grid;
          });
    }

    public static HouseHoldBuilder HouseHold(DateTime houseHoldDate)
    {
      return (HouseHoldBuilder)new HouseHoldBuilder().With(
        p =>
        {
          p.HouseHoldDate = houseHoldDate;
        });
    }

    public static ExportFileBuilder ExportFile(Guid fileId, byte[] fileData)
    {
        return (ExportFileBuilder)new ExportFileBuilder().With(
          p =>
              {
                  p.Id = fileId;
                  p.FileData = fileData;
              });
    }

    public static SessionBuilder Session(String guid, DateTime start, DateTime? end)
    {
      return (SessionBuilder)new SessionBuilder().With(
        p =>
          {
            p.Guid = guid;
            p.End = end;
            p.Start = start;
          });
    }
  }
}
