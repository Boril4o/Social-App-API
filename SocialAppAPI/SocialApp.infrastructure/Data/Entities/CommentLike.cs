using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocialApp.infrastructure.Data.Entities
{
    public class CommentLike : BaseEntity
    {
        [Required]
        [ForeignKey(nameof(Owner))]
        public Guid OwnerId { get; set; }
        public User Owner { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Comment))]
        public Guid CommentId { get; set; }
        public Comment Comment { get; set; } = null!;
    }
}
