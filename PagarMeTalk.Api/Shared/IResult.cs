namespace PagarMeTalk.Api.Shared
{
    public interface IResult
    {
        string Message { get; set; }
        bool Success { get; set; }
    }
}
