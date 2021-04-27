namespace PagarMeTalk.Api.Entities.Enum
{
    public enum EOrderStatus
    {
        Pending,
        WaitingPayment,
        Paid,
        Overpaid,
        Underpaid,
        Canceled
    }
}