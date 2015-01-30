namespace UnityWeb.Controllers
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Web.Mvc;
  using Logging;
  using ServiceInterfaces;
  using UnityWeb.Filters;
  using UnityWeb.Models.BigZip;

  [AuthorizeLoggedInUser]
  public class BigZipController : BaseController
  {
    private readonly IZipFileService _zipFileService;
    private readonly IUserService _userService;
    
    public BigZipController(IZipFileService zipFileService,
        IUserService userService,
        ILogger logger)
          : base(logger)
    {
      this._zipFileService = zipFileService;
      this._userService = userService;
    }

    [OutputCache(CacheProfile = "long", VaryByParam = "bigZip")]
    public ActionResult Search(string bigZip)
    {
      var model = new BigZipsViewModel();
      var bigZipFiles = this._zipFileService.SearchBigZip(bigZip, this._userService.GetUserManCoIds());
      model.AddFiles(bigZipFiles);
      return View(model);
    }
  }
}