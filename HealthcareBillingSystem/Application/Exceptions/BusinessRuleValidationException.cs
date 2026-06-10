namespace HealthcareBillingSystem.Application.Exceptions;

public class BusinessRuleValidationException : Exception
{
    public BusinessRuleValidationException(string message)
        : base(message)
    {
    }
}
