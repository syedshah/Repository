namespace WCFRepository.Interfaces
{
  using System;
  using System.Collections.Generic;
  using OneStepServiceFactory.OneStepService;

  public interface IFileDataRepository
  {
    List<FileStatusData> GetFileData(DateTime syncDate);
  }
}
