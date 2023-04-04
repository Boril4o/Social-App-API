using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static SocialApp.infrastructure.Data.Constants.DataConstants;

namespace SocialApp.infrastructure.Data.Entities
{
    public class User : IdentityUser
    {
        public User()
        {
            this.Comments = new();
            this.Posts = new();
            this.LikedPosts = new();
            this.LikedComments = new();
        }

        [Required]
        [MaxLength(UsernameMaxLength)]
        override public string? UserName { get; set; } = null!;

        [Required]
        public byte[] ProfilePicture { get; set; } = null!;

        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        public List<Comment> Comments { get; set; }

        public List<CommentLike> LikedComments { get; set; }

        public List<Post> Posts { get; set; }

        public List<PostLike> LikedPosts { get; set; }
    }
}
