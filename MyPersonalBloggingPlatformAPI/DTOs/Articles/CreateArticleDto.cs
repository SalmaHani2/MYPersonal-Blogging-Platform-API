using System.ComponentModel.DataAnnotations;

namespace PersonalBloggingPlatformAPI.DTOs.Articles;

public class CreateArticleDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;
}
