namespace Server.Exceptions
{
    public class ConflictException : HttpException
    {
        public ConflictException(string message) : base(message, 409) { }
    }
}
