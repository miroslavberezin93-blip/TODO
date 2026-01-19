namespace Server.Exceptions
{
    public class InvalidCredentialsException : HttpException
    {
        public InvalidCredentialsException(string message) : base(message, 400) { }
    }
}
