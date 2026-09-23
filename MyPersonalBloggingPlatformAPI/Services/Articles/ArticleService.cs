using MyPersonalBloggingPlatformAPI.DTOs.Articles;
using MyPersonalBloggingPlatformAPI.Models;
using PersonalBloggingPlatformAPI.DTOs.Articles;
using PersonalBloggingPlatformAPI.Repositories.Articles;
using MyPersonalBloggingPlatformAPI.DTOs.Common;

namespace PersonalBloggingPlatformAPI.Services.Articles;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _repository;
    private readonly ILogger<ArticleService> _logger;

    public ArticleService(
        IArticleRepository repository,
        ILogger<ArticleService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<PagedResult<Article>> GetArticlesAsync(
        ArticleFilterDto filter)
    {
        _logger.LogInformation(
            "Getting articles with filters.");

        return await _repository.GetAllAsync(filter);
    }

    public async Task<Article?> GetArticleByIdAsync(int id)
    {
        _logger.LogInformation(
            "Getting article with ID {Id}.",
            id);

        var article = await _repository.GetByIdAsync(id);

        if (article == null)
        {
            _logger.LogWarning(
                "Article with ID {Id} was not found.",
                id);
        }

        return article;
    }

    public async Task<Article> CreateArticleAsync(
        CreateArticleDto dto,
        int userId)
    {
        _logger.LogInformation(
            "Creating a new article with title {Title} by user {UserId}.",
            dto.Title, userId);

        var article = new Article
        {
            Title = dto.Title,
            Content = dto.Content,
            CreatedAt = DateTime.UtcNow,
            UserId = userId
        };

        await _repository.AddAsync(article);

        await _repository.SaveChangesAsync();

        _logger.LogInformation(
            "Article with ID {Id} was created successfully.",
            article.Id);

        return article;
    }

    public async Task<OperationResult> UpdateArticleAsync(
        int id,
        UpdateArticleDto dto,
        int userId)
    {
        _logger.LogInformation(
            "Updating article with ID {Id} by user {UserId}.",
            id, userId);

        var article = await _repository.GetByIdAsync(id);

        if (article == null)
        {
            _logger.LogWarning(
                "Article with ID {Id} was not found.",
                id);

            return OperationResult.NotFound;
        }

        if (article.UserId != userId)
        {
            _logger.LogWarning(
                "User {UserId} attempted to update article {Id} owned by {OwnerId}.",
                userId, id, article.UserId);

            return OperationResult.Forbidden;
        }

        article.Title = dto.Title;
        article.Content = dto.Content;
        article.UpdatedAt = DateTime.UtcNow;

        _repository.Update(article);

        await _repository.SaveChangesAsync();

        _logger.LogInformation(
            "Article with ID {Id} was updated successfully.",
            id);

        return OperationResult.Success;
    }

    public async Task<OperationResult> DeleteArticleAsync(int id, int userId)
    {
        _logger.LogInformation(
            "Deleting article with ID {Id} by user {UserId}.",
            id, userId);

        var article = await _repository.GetByIdAsync(id);

        if (article == null)
        {
            _logger.LogWarning(
                "Article with ID {Id} was not found.",
                id);

            return OperationResult.NotFound;
        }

        if (article.UserId != userId)
        {
            _logger.LogWarning(
                "User {UserId} attempted to delete article {Id} owned by {OwnerId}.",
                userId, id, article.UserId);

            return OperationResult.Forbidden;
        }

        _repository.Delete(article);

        await _repository.SaveChangesAsync();

        _logger.LogInformation(
            "Article with ID {Id} was deleted successfully.",
            id);

        return OperationResult.Success;
    }
}
