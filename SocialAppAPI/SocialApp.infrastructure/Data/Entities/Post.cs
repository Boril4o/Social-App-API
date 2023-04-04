using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SocialApp.infrastructure.Data.Constants.DataConstants;

namespace SocialApp.infrastructure.Data.Entities
{
    public class Post : BaseEntity
    {
        public Post()
        {
            this.Comments = new();
            this.Likes = new();
        }

        [MaxLength(ContentMaxLength)]
        public string? Content { get; set; }

        public byte[]? Picture { get; set; }

        [ForeignKey(nameof(Owner))]
        [Required]
        public string OwnerId { get; set; } = null!;
        public User Owner { get; set; } = null!;

        public List<Comment> Comments { get; set; }

        public List<PostLike> Likes { get; set; }
    }
}
