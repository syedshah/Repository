namespace UnityWeb.Controllers
{
  using System;
  using System.Net;
  using System.Web;
  using System.Web.Mvc;
  using Logging;
  using UnityWeb.Models;

  public class BaseController : Controller
  {
    protected readonly ILogger Logger;

    public BaseController(ILogger logger)
    {
      if (logger == null)
      {
        throw new ArgumentNullException("logger");
      }
      Logger = logger;
    }

    protected override void OnException(ExceptionContext filterContext)
    {
      // Bail if we can't do anything; app will crash.
      if (filterContext == null)
        return;
      if (!filterContext.HttpContext.IsCustomErrorEnabled) return;
      var ex = filterContext.Exception ?? new Exception("No further information exists.");
      filterContext.ExceptionHandled = true;
      if ((ex.GetType() != typeof(HttpRequestValidationException)))
      {
        Logger.Error(ex, filterContext.HttpContext.Request.Path, filterContext.HttpContext.Request.RawUrl);
        var data = new ErrorViewModel
        {
          ErrorMessage = HttpUtility.HtmlEncode(ex.Message),
          DisplayMessage = "Unhandled exception",
          ErrorCode = ErrorCode.Unknown,
          Severity = ErrorSeverity.Error
        };
        filterContext.Result = View("Error", data);
      }
    }

    protected JsonResult JsonError(Exception e, ErrorCode errorCode, string displayMessage)
    {
      Response.StatusCode = (int)HttpStatusCode.InternalServerError;
      return Json(new ErrorViewModel
      {
        DisplayMessage = displayMessage,
        ErrorCode = errorCode,
        ErrorMessage = e.Message,
        Severity = ErrorSeverity.Error
      });
    }
  }
}
