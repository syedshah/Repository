namespace UnityWeb.Controllers
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Web.Mvc;
  using Logging;
  using ServiceInterfaces;
  using UnityWeb.Filters;
  using UnityWeb.Models.File;

  [AuthorizeLoggedInUser]
  public class FileController : BaseController
  {
    private readonly IXmlFileService _xmlFileService;
    private readonly IUserService _userService;

    public FileController(
        IXmlFileService xmlFileService,
        IUserService userService,
        ILogger logger)
      : base(logger)
    {
      _xmlFileService = xmlFileService;
      this._userService = userService;
    }

    [OutputCache(CacheProfile = "long", VaryByParam = "file")]
    public ActionResult Search(string file)
    {
      var filesViewModel = new FilesViewModel();
      var xmlFiles = _xmlFileService.Search(file, this._userService.GetUserManCoIds());
      filesViewModel.AddFiles(xmlFiles);
      return View(filesViewModel);
    }

  }
}
