namespace Mymarket.Application.Common.Exceptions;

public class InsufficientBalanceException(decimal requiredAmount, decimal currentBalance)
    : Exception("Insufficient balance.")
{
    public decimal RequiredAmount { get; } = requiredAmount;
    public decimal CurrentBalance { get; } = currentBalance;
}
