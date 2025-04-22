using AskIt.Models.Customizations.Errors;
using AskIt.Models.Customizations.Helpers;
using AskIt.Models.InputModels.Question;
using AskIt.Models.Services.Application.QuestionService;
using AskIt.Models.ViewModels.Question;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AskIt.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly IQuestionService _questionService;
        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var questions = await _questionService.GetQuestionsAsync();

            return View(questions.SuccessValue);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateQuestionInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _questionService.CreateQuestionAsync(model);

            var response = result.Match<IActionResult>(
                success => RedirectToAction("Create", "Questions"),
                error =>
                {
                    switch(error)
                    {
                        case QuestionError.CreateFailed:
                            ModelState.AddModelError(string.Empty, "Errore durante la creazione della domanda");
                            return View(model);
                        
                        default:
                            ModelState.AddModelError(string.Empty, "Errore generico durante la creazione della domanda");
                            return View(model);
                    }

                }
            );

            return response;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _questionService.DeleteQuestionAsync(id);

            if(result.IsSuccess)
            {
                return RedirectToAction("Index", "Questions");
            }

            switch(result.ErrorValue)
            {
                case QuestionError.NotFound:
                    ModelState.AddModelError(string.Empty, "Nessuna domanda trovata per la cancellazione");
                    return View();
                case QuestionError.Unauthorized:
                    return RedirectToAction("AccessDenied", "Account");
                case QuestionError.CouldNotDelete:
                    ModelState.AddModelError(string.Empty, "Errore durante la cancellazione della domanda");
                    return View();
                default:
                    ModelState.AddModelError(string.Empty, "Errore generico durante la cancellazione");
                    return View();
            }
 
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Detail(int id)
        {
            Result<QuestionDetailViewModel, QuestionError>? model = await _questionService.GetQuestionDetailAsync(id);

            var response = model.Match<IActionResult>(
                success => View(success),
                error =>
                {
                    switch (error)
                    {
                        case QuestionError.NotFound:
                            return NotFound("Domanda non trovata.");
                        default:
                            return BadRequest("Errore generico durante il recupero della domanda.");
                    }
                }
            );

            return response;
        }

    }
}
