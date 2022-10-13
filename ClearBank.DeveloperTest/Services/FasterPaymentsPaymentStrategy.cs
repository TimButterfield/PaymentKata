using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services;

public class FasterPaymentsPaymentStrategy : IPaymentStrategy
{
    public bool Applies(MakePaymentRequest paymentRequest) => paymentRequest.PaymentScheme == PaymentScheme.FasterPayments;
    
    public MakePaymentResult ValidateRequest(MakePaymentRequest paymentRequest, Account account)
    {
        var accountIsNull = account == null;
        bool AccountIsNotFasterPayments () => !account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments); 
        bool BalanceIsLessThanPaymentAmount () => account.Balance < paymentRequest.Amount;
        
        if (!Applies(paymentRequest) || accountIsNull || AccountIsNotFasterPayments() || BalanceIsLessThanPaymentAmount())
        {
            return new MakePaymentResult {Success= false};
        }
        
        return new MakePaymentResult {Success= true};
    }
}