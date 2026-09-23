using PersonalBloggingPlatformAPI.Models;

namespace MyPersonalBloggingPlatformAPI.Models
{
    public class Article
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? PublishedAt { get; set; }

        // Ownership
        public int? UserId { get; set; }

        public User? User { get; set; }
    }
}
