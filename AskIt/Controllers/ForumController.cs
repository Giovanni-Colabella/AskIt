using AskIt.Models.Customizations.Errors;
using AskIt.Models.Customizations.Exceptions.Question;
using AskIt.Models.InputModels.ForumInputModels;
using AskIt.Models.Services.Application.ForumService;
using AskIt.Models.ViewModels.ForumViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AskIt.Controllers;

public class ForumController : Controller
{
    private readonly ICachedForumService _memoryCaheForumService;

    public ForumController(ICachedForumService _memoryCacheForumService)
    {
        _memoryCaheForumService = _memoryCacheForumService;
    }

    public async Task<IActionResult> Index(int pageNumber)
    {
        try
        {
            var result = await _memoryCaheForumService.GetQuestionsAsync(pageNumber);


            var response = result.Match<IActionResult>(
                View,
                error =>
                {
                    ViewBag.Message = "Non ci sono domande disponibili.";
                    return View(new ForumViewModel());
                }
            );

            return response;
        }
        catch (Exception ex)
        {
            // Log dell'errore
            Console.WriteLine($"Error in ForumController.Index: {ex.Message}");
            return StatusCode(500, "Si è verificato un errore imprevisto.");
        }
    }


    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        var question = await _memoryCaheForumService.GetQuestionByIdAsync(id);

        var response = question.Match<IActionResult>(
            success => View(success),
            error => error switch
            {
                QuestionError.NotFound => throw new QuestionNotFoundException(id),
                _ => throw new Exception("Si è verificato un errore imprevisto.")
            }
        );

        return response;
    }

    [Authorize]
    public IActionResult AddQuestion()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddQuestion(CreateQuestionInputModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        await _memoryCaheForumService.AddQuestionAsync(model);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> AddAnswer(QuestionViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _memoryCaheForumService.AddAnswerAsync(model.AnswerInputModel);
            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Details), new { id = model.AnswerInputModel.QuestionId });
            }
            else
            {
                return View("Error");
            }

        }
        var question = await _memoryCaheForumService.GetQuestionByIdAsync(model.AnswerInputModel.QuestionId);
        var response = question.Match<IActionResult>(
            success => View(nameof(Details), success),
            error => View("Error")
        );

        return response;
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAnswer(int id)
    {
        var result = await _memoryCaheForumService.DeleteAnswerAsync(id);
        if (!result) throw new Exception();

        return RedirectToAction("Index", "Forum");
    }

    [Authorize]
    public IActionResult DeleteQuestionPage(int questionId)
    {
        return View(questionId);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteQuestion(int questionId)
    {
        var result = await _memoryCaheForumService.DeleteQuestionAsync(questionId);
        if(!result) throw new Exception();

        return RedirectToAction("Index", "Forum");
    }

}