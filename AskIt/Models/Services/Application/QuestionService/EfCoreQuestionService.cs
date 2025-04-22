using AskIt.Models.Customizations.Errors;
using AskIt.Models.Customizations.Exceptions.Question;
using AskIt.Models.Customizations.Helpers;
using AskIt.Models.Data;
using AskIt.Models.Data.Entities;
using AskIt.Models.Enums;
using AskIt.Models.InputModels.Question;
using AskIt.Models.ViewModels.Question;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AskIt.Models.Services.Application.QuestionService;

public class EfCoreQuestionService : IQuestionService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<EfCoreQuestionService> _logger;
    private readonly IMapper _mapper;
    private readonly IAuthorizationService _authorizationService;
    public EfCoreQuestionService(UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        ApplicationDbContext dbContext,
        ILogger<EfCoreQuestionService> logger, 
        IMapper mapper,
        IAuthorizationService authorizationService)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
        _authorizationService = authorizationService;
    }

    public async Task<Result<CreateQuestionInputModel, QuestionError>> CreateQuestionAsync(CreateQuestionInputModel model)
    {
        // Ottieni l'utente corrente
        var user = await _userManager.GetUserFromHttpContextAsync(_httpContextAccessor);

        var question = _mapper.Map<Question>(model);
        question.AuthorId = user.Id;
        question.CreatedAt = DateTime.UtcNow;
        question.UpdatedAt = DateTime.UtcNow;

        await _dbContext.Questions.AddAsync(question);
        var result = await _dbContext.SaveChangesAsync();

        if(result > 0)
        {
            _logger.LogInformation("Domanda creata con successo: {Title} da utente {AuthorId}", question.Title, question.AuthorId);
            return Result<CreateQuestionInputModel, QuestionError>.Success(model);
        }
        else
        {
            _logger.LogError("Errore durante la creazione della domanda: {Title} da utente {AuthorId}", question.Title, question.AuthorId);
            return Result<CreateQuestionInputModel, QuestionError>.Failure(QuestionError.CreateFailed);
        }
        
    }

    public async Task<Result<IEnumerable<QuestionViewModel>, QuestionError>> GetQuestionsAsync()
    {
        var questions = await _dbContext.Questions.ToListAsync();

        var questionsViewModel = _mapper.Map<IEnumerable<QuestionViewModel>>(questions);

        return Result<IEnumerable<QuestionViewModel>, QuestionError>.Success(questionsViewModel);
    }

    public async Task<Result<QuestionError>> DeleteQuestionAsync(int id)
    {
        var user = await _userManager.GetUserFromHttpContextAsync(_httpContextAccessor);
        var question = await _dbContext.Questions.FirstOrDefaultAsync(q => q.Id == id);

        if (question is null)
            return Result<QuestionError>.Failure(QuestionError.NotFound);

        var authResult = await _authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, question, nameof(Policies.CanDeleteQuestion));

        if (!authResult.Succeeded)
        {
            _logger.LogInformation($"Utente {user.Id} ha provata a cancellare una domanda a cui non aveva i permessi.");
            return Result<QuestionError>.Failure(QuestionError.Unauthorized);
        }

        _dbContext.Questions.Remove(question);
        var result = await _dbContext.SaveChangesAsync();

        if (result <= 0)
        {
            _logger.LogError($"Utente {user.Id} ha provato a cancellare la sua domanda {question.Id} ma si è verificato un errore");
            return Result<QuestionError>.Failure(QuestionError.CouldNotDelete);
        }

        return Result<QuestionError>.Success();
    }

    public async Task<Result<QuestionDetailViewModel, QuestionError>> GetQuestionDetailAsync(int id)
    {
        Question? question = await _dbContext.Questions.FirstOrDefaultAsync(q => q.Id == id);
        if(question == null)
            throw new QuestionNotFoundException(id);

        QuestionDetailViewModel questionViewModel = _mapper.Map<QuestionDetailViewModel>(question);

        return Result<QuestionDetailViewModel, QuestionError>.Success(questionViewModel);
    }
}
