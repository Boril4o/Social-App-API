using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialApp.infrastructure.Data.Entities
{
    public class PostLike : BaseEntity
    {
        [Required]
        [ForeignKey(nameof(Post))]
        public Guid PostId { get; set; }
        public Post Post { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Owner))]
        public Guid OwnerId { get; set; }
        public User Owner { get; set; } = null!;
    }
}
