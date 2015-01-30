namespace EFRepository
{
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Data.Entity;
  using System.Linq;
  using Repository;

  public abstract class BaseEfRepository<T> : IRepository<T> where T : class
  {
    private readonly DbContext _dataDbContext;
    private readonly IDbSet<T> _dbSet;

    protected BaseEfRepository(DbContext dataDbContext)
    {
      _dataDbContext = dataDbContext;
      _dbSet = dataDbContext.Set<T>();
    }

    public IQueryable<T> Entities
    {
      get { return _dbSet; }
    }

    public T New()
    {
      return _dbSet.Create();
    }

    public void Update(T entity)
    {
      _dbSet.Attach(entity);
      _dataDbContext.Entry(entity).State = EntityState.Modified;
      Save();
    }

    public T Create(T entity)
    {
      var added = _dbSet.Add(entity);
      Save();
      return added;
    }

    public void Create(List<T> collection)
    {
      _dataDbContext.Configuration.AutoDetectChangesEnabled = false;

      int count = 0;

      foreach (var entity in collection)
      {
        ++count;

        _dbSet.Add(entity);

        if (count % 100 == 0 || (collection.Count - count) < 100)
        {
          Save();
        }
      }      
    }

    public void Delete(T entity)
    {
      _dbSet.Remove(entity);
      Save();
    }

    public void Dispose()
    {
      if (_dataDbContext != null)
      {
       _dataDbContext.Dispose(); 
      }
    }

    protected internal void Save()
    {
      _dataDbContext.SaveChanges();
    }
  }
}
