namespace Domain.Exceptions
{
    public class NotEnoughTicketException : BusinessException
    {
        public NotEnoughTicketException(string message)
            : base(message)
        {
        }
    }
}
