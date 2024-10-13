namespace Entities.Exceptions
{
    public abstract class NotFoundException : Exception
    {
        protected NotFoundException(string message) : base(message)
        {
        }
    }
    
    public sealed class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException() : base(
            "User not found.")
        {
        }
    }
}