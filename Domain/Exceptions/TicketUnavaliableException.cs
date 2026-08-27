namespace Domain.Exceptions
{
    public class TicketUnavailableException : BusinessException
    {
        public TicketUnavailableException(string message)
            : base(message)
        {
        }
    }
}
