namespace UnityRepository.Repositories
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Data.Entity;
  using EFRepository;
  using Entities;
  using Exceptions;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class DocumentRepository : BaseEfRepository<Document>, IDocumentRepository
  {
    public DocumentRepository(string connectionString)
      : base(new UnityDbContext(connectionString))
    {
    }

    public Document GetDocument(int id)
    {
      return (from d in Entities 
              where d.Id == id 
              select d).FirstOrDefault();
    }

    public Document GetDocument(string documentId)
    {
      return (from d in GetDocumentCheckOutApprovalRejectionAndHouseHeld()
              where d.DocumentId == documentId 
              select d).FirstOrDefault();
    }

    public IList<Document> GetDocuments()
    {
      return Entities.ToList();
    }

    public IList<Document> GetDocuments(string grid)
    {
      return (from d in Entities 
              where d.GridRun.Grid == grid 
              select d).ToList();
    }

    public IList<Document> GetDocumentsWithApprovalAndRejection(string grid)
    {
      return (from d in Entities.Include(d => d.Approval).Include(r => r.Rejection)
              where d.GridRun.Grid == grid
              select d).ToList();
    }

    public IList<Document> GetUnApprovedDocuments(string grid)
    {
      return (from d in Entities.Include(d => d.Approval)
              where d.GridRun.Grid == grid &&
                    d.Approval == null &&
                    d.Rejection == null
              select d).ToList();
    }

    public IList<Document> GetApprovedDocuments(string grid)
    {
      return (from d in Entities.Include(d => d.Approval)
              where d.GridRun.Grid == grid &&
                    d.Approval != null
              select d).ToList();
    }

    public IList<Document> GetApprovedAndNotExported(bool offShore)
    {
      return (from d in GetDocumentCheckOutApprovalRejectiontApplicationGridRun() 
              where d.Approval != null 
                    && d.Export == null 
                    && d.GridRun.XmlFile.OffShore == offShore
              select d).ToList();
    }

    public IList<KpiReportData> GetDocuments(int mancoId, DateTime startDate, DateTime endDate)
    {
      endDate = endDate.AddHours(23).AddMinutes(59).AddSeconds(59);
      return (from d in Entities
                   where
                     d.ManCo.Id == mancoId 
                     && d.GridRun.EndDate >= startDate
                     && d.GridRun.EndDate < endDate
                   group d by new { DocType = d.DocType.Code, SubDocType = d.SubDocType.Code, ManCo = d.ManCo } into g 
                   select new KpiReportData
                                   {
                                     ManCo = g.Key.ManCo,
                                     NumberOfDocs = g.Count(), 
                                     DocType = g.Key.DocType,
                                     SubDocType = g.Key.SubDocType
                                   }).ToList();
    }

    public void Update(int documentId, int houseHoldingRunId)
    {
      Document document = GetDocument(documentId);
      if (document == null)
      {
        throw new UnityException("documentId not valid");
      }
      document.UpdateDocument(houseHoldingRunId);
      Update(document);
    }

    private IQueryable<Document> GetDocumentCheckOutApprovalRejectiontApplicationGridRun()
    {
      return
        Entities.Include(d => d.Approval)
                .Include(d => d.CheckOut)
                .Include(d => d.Rejection)
                .Include(d => d.GridRun)
                .Include(d => d.GridRun.Application);
    }

    private IQueryable<Document> GetDocumentCheckOutApprovalRejectionAndHouseHeld()
    {
      return Entities
        .Include(d => d.GridRun)
        .Include(d => d.Approval)
        .Include(d => d.CheckOut)
        .Include(d => d.Rejection)
        .Include(d => d.HouseHold);
    }
  }
}
