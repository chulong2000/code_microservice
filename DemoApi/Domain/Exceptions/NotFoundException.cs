namespace DemoApi.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("The requested user was not found.")
        {

        }

        // 2. Constructor that accepts a custom message
        public NotFoundException(string message) : base(message)
        {

        }

        // 3. Constructor that accepts a message and an inner exception
        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
