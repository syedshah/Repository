namespace UnityWeb.Controllers
{
  using System.Web.Mvc;
  using Logging;
  using Microsoft.AspNet.Identity;
  using ServiceInterfaces;
  using UnityWeb.Models.Password;

  public class PasswordController : BaseController
  {
    private readonly IUserService _userService;
    private readonly IPasswordHistoryService _passwordHistoryService;
    private readonly ISecurityQuestionService _securityQuestionService;
    private readonly ISecurityAnswerService _securityAnswerService;

    public PasswordController(
        IUserService userService,
        IPasswordHistoryService passwordHistoryService,
        ISecurityQuestionService securityQuestionService,
        ISecurityAnswerService securityAnswerService,
        ILogger logger)
            : base(logger)
    {
        this._userService = userService;
        this._passwordHistoryService = passwordHistoryService;
        this._securityQuestionService = securityQuestionService;
        this._securityAnswerService = securityAnswerService;
    }

    public ActionResult Index()
    {
      return View();
    }

    public ActionResult Forgotten()
    {
        var forgottenPasswordModel = new ForgottenPasswordModel();

        return View(forgottenPasswordModel); 
    }

    [HttpPost]
    public ActionResult Forgotten(ForgottenPasswordModel model)
    {
      if (ModelState.IsValid)
      {
        var user = this._userService.GetApplicationUser(model.UserName);

        if (user == null)
        {
            TempData["message"] = "Please contact your administrator.";
          return View(model);  
        }

        if (this._securityAnswerService.HasSecurityAnswers(user.Id))
        {
          TempData["UserId"] = user.Id;
          return RedirectToAction("SecurityQuestions", "Password"); 
        }
        else
        {
          TempData["message"] = "Please contact your administrator.";
          return View(model); 
        }  
      }
      else
      {
        TempData["message"] = "Please correct the errors and try again.";
        return View(model);
      }
    }

    public ActionResult SecurityQuestions()
    {
      var userId = TempData["UserId"].ToString();

      var passwordSecurityQuestionsModel = this.PreparePasswordSecurityQuestionsModel(userId);

      return this.View(passwordSecurityQuestionsModel);
    }

    [HttpPost]
    public ActionResult SecurityQuestions(PasswordSecurityQuestionsModel model)
    {
      if (ModelState.IsValid)
      {
        var answerModel1 = new AnswerModel(model.Question1Id, model.Answer1);
        var answerModel2 = new AnswerModel(model.Question2Id, model.Answer2);
        var answerModel3 = new AnswerModel(model.Question3Id, model.Answer3);

        if (this.AnswersIsValid(model.UserId, answerModel1, answerModel2, answerModel3))
        {
          TempData["UserId"] = model.UserId;
          return RedirectToAction("Change", "Password");
        }
        else
        {
          TempData["message"] = "One or more of the answers are wrong. Please try again.";
          TempData["UserId"] = model.UserId;
          return RedirectToAction("SecurityQuestions", "Password");
        }
          
       }
      else
      {
        TempData["message"] = "Please correct the errors and try again.";
        return this.View(model);
      }
    }

    public ActionResult Change()
    {
      var changePasswordModel = new ChangePasswordModel();
      changePasswordModel.UserId = TempData["UserId"].ToString();

      return this.View(changePasswordModel);
    }

    [HttpPost]
    public ActionResult Change(ChangePasswordModel model)
    {
      if (ModelState.IsValid)
      {
        this._userService.ChangePassword(model.UserId, model.Password);

        var user = this._userService.GetApplicationUserById(model.UserId);

        this._userService.SignIn(user, false);

        this._userService.UpdateUserLastLogindate(model.UserId);

        return RedirectToAction("Index", "Dashboard");
      }
      else
      {
        TempData["message"] = "Please correct the errors and try again.";
        return View(model);  
      }
    }

    public ActionResult PasswordComplexity()
    {
       return PartialView("_PasswordComplexity"); 
    }

    private bool AnswersIsValid(string userId,AnswerModel answer1, AnswerModel answer2, AnswerModel answer3)
    {
      bool result = false;

      var result1 = this._securityAnswerService.SecurityAnswerIsValid(userId, answer1.QuestionId, answer1.Answer);
      var result2 = this._securityAnswerService.SecurityAnswerIsValid(userId, answer2.QuestionId, answer2.Answer);
      var result3 = this._securityAnswerService.SecurityAnswerIsValid(userId, answer3.QuestionId, answer3.Answer);

      if (result1 && result2 && result3)
      {
        result = true;
      }

      return result;
    }

    private PasswordSecurityQuestionsModel PreparePasswordSecurityQuestionsModel(string userId)
    {
        var questions = this._securityQuestionService.GetThreeRandomSecurityQuestions();

        var passwordSecurityQuestionsModel = new PasswordSecurityQuestionsModel(
            userId,
            questions[0].Id,
            questions[0].Question,
            questions[1].Id,
            questions[1].Question,
            questions[2].Id,
            questions[2].Question);

        return passwordSecurityQuestionsModel;
    }

    public ActionResult ChangeCurrent()
    {
      var user = this._userService.GetApplicationUser(TempData["username"].ToString());

      var changeCurrentPasswordModel = new ChangeCurrentPasswordModel(user.Id);

      return this.View(changeCurrentPasswordModel);
    }

    [HttpPost]
    public ActionResult ChangeCurrent(ChangeCurrentPasswordModel model)
    {
      if (ModelState.IsValid)
      {
        this._userService.ChangePassword(model.UserId, model.CurrentPassword, model.NewPassword);

        var user = this._userService.GetApplicationUserById(model.UserId);

        this._userService.SignIn(user, false);

        this._userService.UpdateUserLastLogindate(model.UserId);

        return RedirectToAction("Index", "Dashboard");
      }
      else
      {
         TempData["message"] = "Please correct the errors and try again.";
         return View(model);
      }
    }

  }
}
