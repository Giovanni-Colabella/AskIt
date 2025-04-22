using AskIt.Models.Customizations.Exceptions.Account;
using AskIt.Models.Customizations.Exceptions.Question;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AskIt.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;
        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        // GET: ErrorController
        public IActionResult Index()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var error = feature?.Error;
            if(error != null) _logger.LogWarning(error, "Si è verificata un'eccezione nella richiesta a {Path}", feature?.Path);

            return error switch
            {
                AccountNotFoundException => HandleError("Utente non trovato", 404, "AccountNotFoundError"),
                QuestionNotFoundException e => HandleError("Nessuna domanda tovata", 404, "QuestionNotFoundError", e.Message),
                _ => HandleError("Errore imprevisto", 500, "Index")
            };
        }

        private IActionResult HandleError(string title, int statusCode, string ViewName, string? exMsg = null)
        {
            ViewData["Title"] = title;
            ViewData["ExceptionMessage"] = exMsg;
            Response.StatusCode = statusCode;
            return View(ViewName);
        }

    }
}
