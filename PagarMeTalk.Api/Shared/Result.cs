namespace PagarMeTalk.Api.Shared
{
    public class Result<T> : IResult where T : class
    {
        public T Data { get; private set; }

        public string Message { get; set; }
        public bool Success { get; set; }

        public Result(string message, bool success)
        {
            Message = message;
            Success = success;
        }

        public Result(T data, string message, bool success)
        {
            Data = data;
            Message = message;
            Success = success;
        }
    }
}
