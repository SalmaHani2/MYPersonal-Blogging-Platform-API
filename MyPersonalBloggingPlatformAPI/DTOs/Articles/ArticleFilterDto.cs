using System.ComponentModel.DataAnnotations;

namespace PersonalBloggingPlatformAPI.DTOs.Articles;

public class ArticleFilterDto
{
    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }
    public string? Search { get; set; }

    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 10;

    public string? SortBy { get; set; }

    public string? SortOrder { get; set; }
}