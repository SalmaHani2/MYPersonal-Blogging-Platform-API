using System.ComponentModel.DataAnnotations;

namespace MyPersonalBloggingPlatformAPI.DTOs.Articles
{

    public class UpdateArticleDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;
    }

}
