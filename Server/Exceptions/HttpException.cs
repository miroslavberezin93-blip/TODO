namespace Server.Exceptions
{
    public class HttpException : Exception
    {
        public HttpException(string message, int statusCode) : base (message)
        {
            StatusCode = statusCode;
        }
        public int StatusCode { get; }
    }
}
