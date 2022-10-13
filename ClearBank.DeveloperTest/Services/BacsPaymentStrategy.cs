using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services;

public class BacsPaymentStrategy : IPaymentStrategy
{
    public bool Applies(MakePaymentRequest paymentRequest) => paymentRequest.PaymentScheme == PaymentScheme.Bacs;
    
    public MakePaymentResult ValidateRequest(MakePaymentRequest paymentRequest, Account account)
    {
        var accountIsNull = account == null;
        bool AccountIsNotBacs () => !account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs); 
        
        if (!Applies(paymentRequest) || accountIsNull || AccountIsNotBacs())
        {
            return new MakePaymentResult {Success= false};
        }
        
        return new MakePaymentResult {Success= true};
    }
}