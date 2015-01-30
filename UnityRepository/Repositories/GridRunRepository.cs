namespace UnityRepository.Repositories
{
  using System;
  using System.Collections.Generic;
  using System.Data.Entity;
  using System.Linq;
  using EFRepository;
  using Entities;
  using UnityRepository.Contexts;
  using UnityRepository.Interfaces;

  public class GridRunRepository : BaseEfRepository<GridRun>, IGridRunRepository
  {
    public GridRunRepository(string connectionString)
      : base(new UnityDbContext(connectionString))
    {
    }

    public IList<GridRun> GetProcessing()
    {
      return (from i in this.GetGridRunAndFile()
              where i.EndDate == null && i.XmlFile != null
              select i).ToList();
    }

    public IList<GridRun> GetProcessing(List<int> manCoIds)
    {
        return (from i in this.GetGridRunAndFile()
                where i.EndDate == null && manCoIds.Contains(i.XmlFile.ManCoId)
                select i).ToList();
    }

    public IList<GridRun> GetTopFifteenSuccessfullyCompleted()
    {
      return this.GetGridRunAndFile()
              .Where(e => e.EndDate != null && e.Status == 2)
              .OrderByDescending(o => o.Id)
              .Take(15).ToList();
    }

    public IList<GridRun> GetTopFifteenSuccessfullyCompleted(List<int> manCoIds)
    {
        return this.GetGridRunAndFile()
                .Where(e => (e.EndDate != null && e.Status == 2) && manCoIds.Contains(e.XmlFile.ManCoId))
                .OrderByDescending(o => o.Id)
                .Take(15).ToList();
    }

    public IList<GridRun> GetTopFifteenRecentExceptions()
    {
      return this.GetGridRunAndFile()
               .Where(e => e.EndDate != null && (e.Status == 4 || e.Status == 5))
               .OrderByDescending(o => o.Id)
               .Take(15).ToList();
    }

    public IList<GridRun> GetTopFifteenRecentExceptions(List<int> manCoIds)
    {
        return GetGridRunAndFile()
               .Where(e => e.EndDate != null && (e.Status == 4 || e.Status == 5) && manCoIds.Contains(e.XmlFile.ManCoId))
               .OrderByDescending(o => o.Id)
               .Take(15).ToList();
    }

    public IList<GridRun> GetTopFifteenRecentUnapprovedGrids(List<int> manCoIds)
    {
      return Entities.Where(g => g.Documents.Any(d => d.Approval == null && d.Rejection == null) && manCoIds.Contains(g.Documents.FirstOrDefault().ManCoId))
                .OrderByDescending(o => o.Id)
                .Take(15)
                .ToList();
    }

    public IList<GridRun> GetTopFifteenGridsWithRejectedDocuments(List<int> manCoIds)
    {
      return Entities.Where(g => g.Documents.Any(d => d.Rejection != null) && manCoIds.Contains(g.Documents.FirstOrDefault().ManCoId))
                .OrderByDescending(o => o.Id)
                .Take(15)
                .ToList();
    }

    public IList<GridRun> GetTopFifteenGridsAwaitingHouseHolding(List<int> manCoIds)
    {
      return Entities.Where(g => g.Documents.Any(d => d.HouseHold == null && d.Approval != null) && manCoIds.Contains(g.Documents.FirstOrDefault().ManCoId))
                .OrderByDescending(o => o.Id)
                .Take(15)
                .ToList();
    }

    public PagedResult<GridRun> GetGridRuns(int pageNumber, int numberOfItems, string houseHoldingGrid, List<int> manCoIds)
    {
      return new PagedResult<GridRun>
      {
        CurrentPage = pageNumber,
        ItemsPerPage = numberOfItems,
        TotalItems = Entities.Count(p => p.HouseHoldingRun.Grid == houseHoldingGrid && manCoIds.Contains(p.Documents.FirstOrDefault().ManCoId)),
        Results = Entities.Where(p => p.HouseHoldingRun.Grid == houseHoldingGrid && manCoIds.Contains(p.Documents.FirstOrDefault().ManCoId))
                  .OrderBy(p => p.Id)
                  .Skip((pageNumber - 1) * numberOfItems)
                  .Take(numberOfItems)
                  .ToList()
      };
    }

    public PagedResult<GridRun> GetUnapproved(int pageNumber, int numberOfItems, List<int> manCoIds)
    {
     return new PagedResult<GridRun>
                    {
                      CurrentPage = pageNumber,
                      ItemsPerPage = numberOfItems,
                      TotalItems = Entities.Count(p => p.Documents.Any(d => d.Approval == null && d.Rejection == null) && manCoIds.Contains(p.Documents.FirstOrDefault().ManCoId)),
                      Results = Entities.Where(p => p.Documents.Any(d => d.Approval == null && d.Rejection == null) && manCoIds.Contains(p.Documents.FirstOrDefault().ManCoId))
                                .OrderByDescending(p => p.StartDate)
                                .Skip((pageNumber - 1) * numberOfItems)
                                .Take(numberOfItems)
                                .ToList()
                    };
    }

    public GridRun GetGridRun(int id)
    {
      return (from j in ApprovalRejectionAndHouseHold() 
              where j.Id == id 
              select j).FirstOrDefault();
    }

    public GridRun GetGridRun(string grid)
    {
      return (from g in Entities
              where g.Grid == grid
              select g).FirstOrDefault();
    }

    public GridRun GetGridRun(string code, string grid)
    {
      return (from g in Entities
              where g.Application.Code == code && g.Grid == grid
              select g).FirstOrDefault();
    }

    public GridRun GetGridRun(string fileName, string code, string grid, DateTime startDate)
    {
      return (from g in Entities
              where g.XmlFile.FileName == fileName && g.Application.Code == code && g.Grid == grid && g.StartDate == startDate
              select g).FirstOrDefault();
    }

    public IList<GridRun> Search(string grid)
    {
      return (from g in Entities 
              where g.Grid.Contains(grid) 
              select g).ToList();
    }

    public IList<GridRun> Search(string grid, List<int> manCoIds)                                                                                                           
    {
      return (from g in Entities
              where g.Grid.Contains(grid) && manCoIds.Contains(g.XmlFile.ManCoId)
            select g).ToList();
    }

    private IQueryable<GridRun> GetGridRunAndFile()
    {
      return Entities
        .Include(f => f.XmlFile);
    }

    private IQueryable<GridRun> ApprovalRejectionAndHouseHold()
    {
      return
        Entities.Include(f => f.XmlFile)
                .Include(d => d.Documents.Select(a => a.Approval))
                .Include(d => d.Documents.Select(a => a.Rejection))
                .Include(d => d.Documents.Select(h => h.HouseHold));
    }
  }
}
