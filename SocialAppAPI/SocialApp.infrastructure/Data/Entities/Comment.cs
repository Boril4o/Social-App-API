using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SocialApp.infrastructure.Data.Constants.DataConstants;

namespace SocialApp.infrastructure.Data.Entities
{
    public class Comment : BaseEntity
    {
        public Comment()
        {
            this.Likes = new();
        }

        [Required]
        [MaxLength(CommentContentMaxLength)]
        public string CommentContent { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Owner))]
        public Guid OwnerId { get; set; }
        public User Owner { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Post))]
        public Guid PostId { get; set; }
        public Post Post { get; set; } = null!;

        public List<CommentLike> Likes { get; set; }
    }
}
