using AskIt.Models.Authorization;
using AskIt.Models.Customizations.Errors;
using AskIt.Models.Customizations.Exceptions.Account;
using AskIt.Models.Customizations.Exceptions.Question;
using AskIt.Models.Customizations.Helpers;
using AskIt.Models.Data;
using AskIt.Models.Data.Entities;
using AskIt.Models.InputModels.ForumInputModels;
using AskIt.Models.Mappings;
using AskIt.Models.ViewModels.ForumViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AskIt.Models.Services.Application.ForumService;

public class ForumService : IForumService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthorizationService _authorizationService;

    public ForumService(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor, 
        IAuthorizationService authorizationService)
    {
        _context = context;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _authorizationService = authorizationService;
    }


    public async Task<Result<ForumViewModel, QuestionError>> GetQuestionsAsync(int pageNumber = 1)
    {
        if(pageNumber < 1)
            pageNumber = 1;

        var totalQuestions = await _context.Questions.CountAsync();

        var questionsQuery = _context.Questions
            .AsNoTracking()
            .Include(q => q.Author)
            .Include(q => q.Answers);

        var questions = await questionsQuery
            .Skip((pageNumber - 1) * 10)
            .Take(10)
            .ToListAsync();

        if (questions.Count == 0)
        {
            return Result<ForumViewModel, QuestionError>.Failure(QuestionError.NoQuestionsFound);
        }

        var listQuestionViewModel = questions.Select(q => q.ToModel(q.Author?.UserName ?? "Anonimo")).ToList();

        var totalPages = (int)Math.Ceiling((double)totalQuestions / 10);

        var viewModel = new ForumViewModel
        {
            Questions = listQuestionViewModel,
            CurrentPage = pageNumber,
            TotalPages = totalPages
        };

        return Result<ForumViewModel, QuestionError>.Success(viewModel);
    }


    public async Task<Result<QuestionViewModel, QuestionError>> GetQuestionByIdAsync(int id)
    {
        var question = await _context.Questions
            .AsNoTracking()
            .Include(q => q.Author)
            .Include(q => q.Answers)
                .ThenInclude(a => a.Author)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (question is null)
        {
            return Result<QuestionViewModel, QuestionError>.Failure(QuestionError.NotFound);
        }

        var questionViewModel = question.ToModel(question.Author?.UserName ?? "Anonimo");
        return Result<QuestionViewModel, QuestionError>.Success(questionViewModel);
    }

    public async Task AddQuestionAsync(CreateQuestionInputModel model)
    {
        var user = await _userManager.GetUserFromHttpContextAsync(_httpContextAccessor);

        Question? question = model.ToEntity(user.Id);

        await _context.Questions.AddAsync(question);
        await _context.SaveChangesAsync();
    }

    public async Task<Result<QuestionViewModel, AnswerError>> AddAnswerAsync(CreateAnswerInputModel model)
    {
        var user = await _userManager.GetUserFromHttpContextAsync(_httpContextAccessor);

        Answer? answer = model.ToEntity(user.Id);

        await _context.Answers.AddAsync(answer);
        var result = await _context.SaveChangesAsync();

        if(result <= 0 || answer is null)
            return Result<QuestionViewModel, AnswerError>.Failure(AnswerError.CreationFailed);

        var question = await _context.Questions
            .AsNoTracking()
            .Include(q => q.Author)
            .Include(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == model.QuestionId);

        if (question is null)
        {
            return Result<QuestionViewModel, AnswerError>.Failure(AnswerError.QuestionNotFound);
        }

        var questionViewModel = question.ToModel(question.Author?.UserName ?? "Anonimo");
        return Result<QuestionViewModel, AnswerError>.Success(questionViewModel);

    }

    public async Task<bool> DeleteAnswerAsync(int id)
    {
        Answer? answer = await _context.Answers.FirstOrDefaultAsync(a => a.Id == id);
        if (answer is null) return false;

        var authorizationResult = await _authorizationService.AuthorizeAsync(
            _httpContextAccessor?.HttpContext?.User ?? throw new AccountNotFoundException(),
            answer,
            new DeleteAnswerRequirement()
        );


        if (!authorizationResult.Succeeded)
            throw new UnauthorizedException();

        _context.Answers.Remove(answer);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteQuestionAsync(int questionId)
    {
        var user = await _userManager.GetUserFromHttpContextAsync(_httpContextAccessor);

        Question? question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == questionId);
        if (question is null) throw new QuestionNotFoundException(questionId);

        var authorizationResult = await _authorizationService.AuthorizeAsync(
            _httpContextAccessor?.HttpContext?.User ?? throw new AccountNotFoundException(),
            question,
            new DeleteQuestionRequirement()
        );

        if (!authorizationResult.Succeeded)
            throw new UnauthorizedException();

        _context.Questions.Remove(question);
        await _context.SaveChangesAsync();
        return true;
    }
}