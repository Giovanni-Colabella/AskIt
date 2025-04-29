using AskIt.Models.Customizations.Errors;
using AskIt.Models.Customizations.Exceptions.Course;
using AskIt.Models.Enums;
using AskIt.Models.InputModels.CourseInputModels;
using AskIt.Models.Services.Application.CourseService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AskIt.Controllers;

public class CoursesController : Controller
{
    private readonly ICourseService _courseService;
    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public async Task<IActionResult> Index(int pageNumber = 1)
    {
        var result = await _courseService.GetCoursesAsync(pageNumber);
        var response = result.Match<IActionResult>(
            View,
            error => error switch
            {
                CourseError.NotFound => throw new CourseNotFoundException("Il corso specificato non è stato trovato."),
                _ => throw new Exception("Si è verificato un errore imprevisto durante il recupero dei corsi.")
            }
        );

        return response;
    }

    [HttpGet]
    [Authorize(Roles = nameof(Roles.Docente))]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = nameof(Roles.Docente))]
    public async Task<IActionResult> Create(CreateCourseInputModel inputModel)
    { 
        if(!ModelState.IsValid)
            return View(inputModel);

        var result = await _courseService.CreateCourseAsync(inputModel);
        var response = result.Match<IActionResult>(
            success => RedirectToAction(nameof(Index)),
            error => 
            {
                switch(error)
                {
                    case CourseError.CreationFailed:
                        ModelState.AddModelError(string.Empty, "Si è verificato un errore durante la creazione del corso.");
                        break;
                    default:
                        throw new Exception("Si è verificato un errore imprevisto durante la creazione del corso.");
                }

                return View(inputModel);
            }
        );

        return response;
    }

}
