using MyPersonalBloggingPlatformAPI.Models;
using PersonalBloggingPlatformAPI.DTOs.Articles;
using MyPersonalBloggingPlatformAPI.DTOs.Common;

namespace PersonalBloggingPlatformAPI.Repositories.Articles;

public interface IArticleRepository
{
    Task<PagedResult<Article>> GetAllAsync(
        ArticleFilterDto filter);

    Task<Article?> GetByIdAsync(int id);

    Task AddAsync(Article article);

    void Update(Article article);

    void Delete(Article article);

    Task SaveChangesAsync();
}