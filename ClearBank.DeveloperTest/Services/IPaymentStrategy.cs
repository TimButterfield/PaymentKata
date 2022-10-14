using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services;

public interface IPaymentStrategy
{
    bool Applies(MakePaymentRequest paymentRequest);

    MakePaymentResult ValidatePaymentRequest(MakePaymentRequest paymentRequest, Account account);
}