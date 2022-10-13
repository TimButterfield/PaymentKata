using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services;

public class ChapsPaymentsPaymentStrategy : IPaymentStrategy
{
    public bool Applies(MakePaymentRequest paymentRequest) => paymentRequest.PaymentScheme == PaymentScheme.Chaps;
    
    public MakePaymentResult ValidateRequest(MakePaymentRequest paymentRequest, Account account)
    {
        var accountIsNull = account == null;
        bool AccountIsNotChapsPayments () => !account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps); 
        bool AccountIsNotLive () => account.Status != AccountStatus.Live;
        
        if (!Applies(paymentRequest) || accountIsNull || AccountIsNotChapsPayments() || AccountIsNotLive())
        {
            return new MakePaymentResult {Success= false};
        }
        
        return new MakePaymentResult { Success = true};
    }
}