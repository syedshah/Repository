using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestConsoleApp
{
  using ClientProxies.ArchiveServiceReference;

  class Program
  {
    static void Main(string[] args)
    {
      IDocument proxy = new DocumentClient();
    }
  }
}
