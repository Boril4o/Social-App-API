namespace SocialAppAPI.Responses
{
    public class ResponseMessages
    {
        //Global messages
        public const string Error = "Something went wrong";

        //User
        public const string UserRegistrationFailed = "User registration attempt failed";
        public const string UserRegistrationSuccessful = "User has been registered";
        public const string UserDoesNotExist = "User does not exist";
        public const string UserIdDoesNotExist = "User with id {0} not found.";
        public const string EmptyUserName = "Username cannot be empty";
        public const string UserNameExist = "Username already exist";
    }
}
