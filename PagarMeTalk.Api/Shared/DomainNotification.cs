namespace PagarMeTalk.Api.Shared
{
    public class DomainNotification
    {
        public DomainNotification(string propertyName, string message)
        {
            PropertyName = propertyName;
            Message = message;
        }

        public string PropertyName { get; set; }
        public string Message { get; set; }
    }
}