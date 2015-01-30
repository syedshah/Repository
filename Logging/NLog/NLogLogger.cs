namespace Logging.NLog
{
  using System;
  using global::NLog;

  public class NLogLogger : ILogger
  {
    private readonly Logger _logger;

    public NLogLogger()
    {
      _logger = LogManager.GetLogger("Unity");
    }

    public void Info(string message)
    {
      _logger.Info(message);
    }

    public void Warn(string message)
    {
      _logger.Warn(message);
    }

    public void Debug(string message)
    {
      _logger.Debug(message);
    }

    public void Error(string message)
    {
      _logger.Error(message);
    }

    public void Error(Exception x, string path, string url)
    {
      Error(x.BuildExceptionMessage(path, url));
    }

    public void Error(string message, Exception x)
    {
      _logger.ErrorException(message, x);
    }

    public void Fatal(string message)
    {
      _logger.Fatal(message);
    }

    public void Fatal(string message, Exception x)
    {
      _logger.Fatal(message, x);
    }

    public void Fatal(Exception x, string path, string url)
    {
      Fatal(x.BuildExceptionMessage(path, url));
    }
  }
}
