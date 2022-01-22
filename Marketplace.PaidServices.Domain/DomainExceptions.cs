namespace Marketplace.PaidServices.Domain;

public static class DomainExceptions
{
    public class OperationNotAllowedException : Exception
    {
        public OperationNotAllowedException(
            object entity,
            string operation,
            string state
        ) : base(
            $"Operation {operation} is not allow for the entity {entity.GetType().Name} in state {state}")
        {
        }
    }
}