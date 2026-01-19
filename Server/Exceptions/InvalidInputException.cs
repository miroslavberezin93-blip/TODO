namespace Server.Exceptions
{
    public class InvalidInputException : HttpException
    {
        public InvalidInputException(string message) : base(message, 400) { }
    }
}
