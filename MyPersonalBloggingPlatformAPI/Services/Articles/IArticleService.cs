using PersonalBloggingPlatformAPI.DTOs.Articles;
using PersonalBloggingPlatformAPI.Models;
using PersonalBloggingPlatformAPI.DTOs.Common;

namespace PersonalBloggingPlatformAPI.Services.Articles;

public interface IArticleService
{
    Task<PagedResult<Article>> GetArticlesAsync(
        ArticleFilterDto filter);

    Task<Article?> GetArticleByIdAsync(int id);

    Task<Article> CreateArticleAsync(
        CreateArticleDto dto,
        int userId);

    Task<OperationResult> UpdateArticleAsync(
        int id,
        UpdateArticleDto dto,
        int userId);

    Task<OperationResult> DeleteArticleAsync(int id, int userId);
}