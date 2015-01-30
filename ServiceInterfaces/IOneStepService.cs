namespace ServiceInterfaces
{
  using System;
  using System.Collections.Generic;
  using ClientProxies.OneStepServiceReference;

  public interface IOneStepService
  {
    List<FileStatusData> GetFileData(DateTime syncDate);
  }
}
