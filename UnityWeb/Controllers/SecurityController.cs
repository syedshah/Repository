namespace UnityWeb.Controllers
{
  using System.Collections.Generic;
  using System.Web.Mvc;

  using Entities;

  using Logging;

  using ServiceInterfaces;

  using UnityWeb.Models.Security;

  public class SecurityController : BaseController
  {
    private readonly IUserService _userService;

    private readonly ISecurityQuestionService _securityQuestionService;

    private readonly ISecurityAnswerService _securityAnswerService;

    public SecurityController(
      IUserService userService,
      ISecurityQuestionService securityQuestionService,
      ISecurityAnswerService securityAnswerService,
      ILogger logger)
      : base(logger)
    {
      this._userService = userService;
      this._securityQuestionService = securityQuestionService;
      this._securityAnswerService = securityAnswerService;
    }

    public ActionResult Index()
    {
      return View();
    }

    public ActionResult AddOrUpdate()
    {
      var user = this._userService.GetApplicationUser();

      if (!this._securityAnswerService.HasSecurityAnswers(user.Id))
      {
        return RedirectToAction("AddAnswers", "Security");
      }
      else
      {
        return RedirectToAction("EditAnswers", "Security");

      }
    }

    public ActionResult AddAnswers()
    {
      var user = this._userService.GetApplicationUser();

      var securityQuestions = this._securityQuestionService.GetSecurityQuestions();

      var model = new AddSecurityAnswersModel { UserId = user.Id };

      for (int i = 0, k = 1; i < securityQuestions.Count; i++, k++)
      {
        model.GetType().GetProperty("Question" + k.ToString()).SetValue(model, securityQuestions[i].Question);
        model.GetType().GetProperty("Question" + k.ToString() + "Id").SetValue(model, securityQuestions[i].Id);
      }

      return this.View("Add", model);
    }

    [HttpPost]
    public ActionResult AddAnswers(AddSecurityAnswersModel model)
    {
      if (ModelState.IsValid)
      {
        var listSecurityAnswers = new List<SecurityAnswer>();

        listSecurityAnswers.Add(new SecurityAnswer(model.Answer1, model.Question1Id, model.UserId));
        listSecurityAnswers.Add(new SecurityAnswer(model.Answer2, model.Question2Id, model.UserId));
        listSecurityAnswers.Add(new SecurityAnswer(model.Answer3, model.Question3Id, model.UserId));
        listSecurityAnswers.Add(new SecurityAnswer(model.Answer4, model.Question4Id, model.UserId));
        listSecurityAnswers.Add(new SecurityAnswer(model.Answer5, model.Question5Id, model.UserId));
        listSecurityAnswers.Add(new SecurityAnswer(model.Answer6, model.Question6Id, model.UserId));
        listSecurityAnswers.Add(new SecurityAnswer(model.Answer7, model.Question7Id, model.UserId));
        listSecurityAnswers.Add(new SecurityAnswer(model.Answer8, model.Question8Id, model.UserId));
        listSecurityAnswers.Add(new SecurityAnswer(model.Answer9, model.Question9Id, model.UserId));
        listSecurityAnswers.Add(new SecurityAnswer(model.Answer10, model.Question10Id, model.UserId));

        this._securityAnswerService.SaveSecurityAnswers(listSecurityAnswers, model.UserId);

        return RedirectToAction("Index");
      }
      else
      {
        TempData["message"] = "Please correct the errors and try again.";
        return this.View("Add", model);
      }
    }

    public ActionResult EditAnswers()
    {
      var user = this._userService.GetApplicationUser();

      var securityAnswers = this._securityAnswerService.GetSecurityAnswers(user.Id);

      var model = new EditSecuirtyAnswersModel();

      for (int i = 0, k = 1; i < securityAnswers.Count; i++, k++)
      {
        model.GetType()
             .GetProperty("Question" + k.ToString())
             .SetValue(model, securityAnswers[i].SecurityQuestion.Question);
        model.GetType().GetProperty("Answer" + k.ToString()).SetValue(model, securityAnswers[i].Answer);
        model.GetType().GetProperty("Answer" + k.ToString() + "Id").SetValue(model, securityAnswers[i].Id);
      }

      return this.View("Edit", model);
    }

    [HttpPost]
    public ActionResult EditAnswers(EditSecuirtyAnswersModel model)
    {
      if (ModelState.IsValid)
      {
        var listSecurityAnswers = new List<SecurityAnswer>();

        listSecurityAnswers.Add(new SecurityAnswer(model.Answer1Id) { Answer = model.Answer1 });
        listSecurityAnswers.Add(new SecurityAnswer(model.Answer2Id) { Answer = model.Answer2 });
        listSecurityAnswers.Add(new SecurityAnswer(model.Answer3Id) { Answer = model.Answer3 });
        listSecurityAnswers.Add(new SecurityAnswer(model.Answer4Id) { Answer = model.Answer4 });
        listSecurityAnswers.Add(new SecurityAnswer(model.Answer5Id) { Answer = model.Answer5 });
        listSecurityAnswers.Add(new SecurityAnswer(model.Answer6Id) { Answer = model.Answer6 });
        listSecurityAnswers.Add(new SecurityAnswer(model.Answer7Id) { Answer = model.Answer7 });
        listSecurityAnswers.Add(new SecurityAnswer(model.Answer8Id) { Answer = model.Answer8 });
        listSecurityAnswers.Add(new SecurityAnswer(model.Answer9Id) { Answer = model.Answer9 });
        listSecurityAnswers.Add(new SecurityAnswer(model.Answer10Id) { Answer = model.Answer10 });

        this._securityAnswerService.UpdateSecurityAnswers(listSecurityAnswers);

        TempData["message"] = "Update was successful.";

        return this.View("Edit", model);
      }
      else
      {
        TempData["message"] = "Update was unsuccessful.";

        return this.View("Edit", model);
      }
    }
  }
}
