using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialApp.infrastructure.Data.Constants
{
    public class DataConstants
    {
        //User
        public const int UsernameMaxLength = 20;
        public const int UsernameMinLength = 3;

        public const int PasswordMaxLength = 50;
        public const int PasswordMinLength = 8;

        public const int DescriptionMaxLength = 400;

        //Post
        public const int ContentMaxLength = 250;

        //Comment
        public const int CommentContentMaxLength = 200;

        public const int CommentContentMinLength = 1;
    }
}
