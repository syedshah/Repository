namespace UnityRepository.Interfaces
{
  using Entities;
  using Repository;

  public interface ICheckOutRepository : IRepository<CheckOut>
  {
    CheckOut GetCheckOut(string documentId);
  }
}
