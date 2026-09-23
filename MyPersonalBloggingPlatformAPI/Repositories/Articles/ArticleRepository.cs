using Microsoft.EntityFrameworkCore;
using PersonalBloggingPlatformAPI.Models;
using PersonalBloggingPlatformAPI.Data;
using PersonalBloggingPlatformAPI.DTOs.Articles;
using PersonalBloggingPlatformAPI.DTOs.Common;

namespace PersonalBloggingPlatformAPI.Repositories.Articles;

public class ArticleRepository : IArticleRepository
{
    private readonly ApplicationDbContext _context;

    public ArticleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

   

    public async Task<Article?> GetByIdAsync(int id)
    {
        return await _context.Articles
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddAsync(Article article)
    {
        await _context.Articles.AddAsync(article);
    }

    public void Update(Article article)
    {
        _context.Articles.Update(article);
    }

    public void Delete(Article article)
    {
        _context.Articles.Remove(article);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    public async Task<PagedResult<Article>> GetAllAsync(
    ArticleFilterDto filter)
    {
        var query = _context.Articles.AsQueryable();

        if (filter.FromDate.HasValue)
        {
            query = query.Where(
                article => article.CreatedAt >= filter.FromDate.Value);
        }

        if (filter.ToDate.HasValue)
        {
            query = query.Where(
                article => article.CreatedAt <= filter.ToDate.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(article =>
                article.Title.Contains(filter.Search) ||
                article.Content.Contains(filter.Search));
        }

        // Validate sort options
        var sortBy = (filter.SortBy ?? "createdAt").ToLowerInvariant();
        var sortOrder = (filter.SortOrder ?? "desc").ToLowerInvariant();

        // Deterministic ordering
        if (sortBy == "title")
        {
            query = sortOrder == "asc" ? query.OrderBy(a => a.Title).ThenBy(a => a.Id) : query.OrderByDescending(a => a.Title).ThenByDescending(a => a.Id);
        }
        else // default createdAt
        {
            query = sortOrder == "asc" ? query.OrderBy(a => a.CreatedAt).ThenBy(a => a.Id) : query.OrderByDescending(a => a.CreatedAt).ThenByDescending(a => a.Id);
        }

        var total = await query.CountAsync();

        var pageNumber = Math.Max(filter.PageNumber, 1);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Article>
        {
            Items = items,
            TotalCount = total,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}