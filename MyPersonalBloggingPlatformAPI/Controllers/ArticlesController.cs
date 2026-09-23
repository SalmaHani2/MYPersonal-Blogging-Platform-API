using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MyPersonalBloggingPlatformAPI.DTOs.Articles;
using MyPersonalBloggingPlatformAPI.DTOs.Common;
using MyPersonalBloggingPlatformAPI.Models;
using PersonalBloggingPlatformAPI.DTOs.Articles;
using PersonalBloggingPlatformAPI.Services.Articles;

namespace PersonalBloggingPlatformAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly IArticleService _articleService;

    public ArticlesController(IArticleService articleService)
    {
        _articleService = articleService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResult<Article>>> GetArticles(
        [FromQuery] ArticleFilterDto filter)
    {
        var paged = await _articleService.GetArticlesAsync(filter);

        return Ok(paged);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<Article>> GetArticle(int id)
    {
        var article = await _articleService.GetArticleByIdAsync(id);

        if (article == null)
        {
            return NotFound();
        }

        return Ok(article);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Article>> CreateArticle(
        CreateArticleDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var article = await _articleService.CreateArticleAsync(dto, userId);

        return CreatedAtAction(
            nameof(GetArticle),
            new { id = article.Id },
            article);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateArticle(
        int id,
        UpdateArticleDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result = await _articleService.UpdateArticleAsync(
            id,
            dto,
            userId);

        return result switch
        {
            OperationResult.NotFound => NotFound(),
            OperationResult.Forbidden => Forbid(),
            OperationResult.Success => NoContent(),
            _ => StatusCode(500)
        };
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteArticle(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var result = await _articleService.DeleteArticleAsync(id, userId);

        return result switch
        {
            OperationResult.NotFound => NotFound(),
            OperationResult.Forbidden => Forbid(),
            OperationResult.Success => NoContent(),
            _ => StatusCode(500)
        };
    }
}