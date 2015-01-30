namespace UnityWeb.Controllers
{
  using System.Web.Mvc;
  using Logging;
  using ServiceInterfaces;
  using UnityWeb.Constants;
  using UnityWeb.Filters; 
  using Entities;
  using System;
  using UnityWeb.Models.NewsTicker;

  public class NewsTickerController : BaseController
  {
    private readonly INewsTickerService _newsTickerService;

    public NewsTickerController(INewsTickerService newsTickerService, ILogger logger)  : base(logger)
    {
     _newsTickerService = newsTickerService;
    }

    [ChildActionOnly]
    [OutputCache(Duration = 180)]
    public ActionResult GetNewsTicker()
    {
      NewsTicker newsTickers= _newsTickerService.GetNewsTicker();
      if (newsTickers != null && newsTickers.News!=null && newsTickers.News.Length>0)
      {
        return PartialView("NewsTicker", new NewsTickerViewModel(newsTickers));
      }
      return PartialView("NewsTicker", null);
    }

    [HttpGet]
    [AuthorizeLoggedInUser(Roles = Roles.DstAdmin)]
    public virtual ActionResult Edit()
    {
      NewsTicker newsTickers = _newsTickerService.GetNewsTicker();
      if (newsTickers != null)
      {
        return View(new EditNewsTickerViewModel { Id = newsTickers.Id, News = newsTickers.News, Date = newsTickers.Date });
      }
      else
      {
       newsTickers= _newsTickerService.AddNewsTicker("", DateTime.Now);
       return View(new EditNewsTickerViewModel { Id = newsTickers.Id, News = newsTickers.News, Date = newsTickers.Date });
      }
    }

    [HttpPost]
    [AuthorizeLoggedInUser(Roles = Roles.DstAdmin)]
    public virtual ActionResult Update(EditNewsTickerViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return RedirectToAction("Edit", "NewsTicker");
      }
      return UpdateNewsTicker(new NewsTicker { Id = model.Id, News = model.News, Date = model.Date });
      
    }

    [AuthorizeLoggedInUser(Roles = Roles.DstAdmin)]
    private ActionResult UpdateNewsTicker(NewsTicker model)
    {
      _newsTickerService.UpdateNewsTicker(model.Id, model.News, DateTime.Now);
      return RedirectToRoute(new { controller = "Dashboard", action = "Index" });
    }



  }
}
