using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
  public interface ILogger
  {
    void Info(string message);
    void Warn(string message);
    void Debug(string message);
    void Error(string message);
    void Error(string message, Exception ex);
    void Error(Exception x, string path, string url);
    void Fatal(string message);
    void Fatal(string message, Exception ex);
    void Fatal(Exception ex, string path, string url);
  }
}
