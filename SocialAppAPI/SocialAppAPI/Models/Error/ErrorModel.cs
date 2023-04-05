namespace SocialAppAPI.Models.Error
{
    public class ErrorModel
    {
        public ErrorModel(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public ErrorModel(string errorMessage, string exceptionMessage) : this(errorMessage)
        {
            ExceptionMessage = exceptionMessage;
        }

        public string ErrorMessage { get; set; } = null!;

        public string? ExceptionMessage { get; set; }

        public override string ToString()
            => $"{ErrorMessage}. Exception Message: {ExceptionMessage}";
    }
}
