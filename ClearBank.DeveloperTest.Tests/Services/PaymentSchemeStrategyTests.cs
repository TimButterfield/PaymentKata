using System;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Services;


public class BacsPaymentSchemeStrategyTests
{
    [Fact]
    public void WhenAccountIsNull_ThenPaymentResultIsUnsuccessful()
    {
        var paymentRequest = new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs}; 
        var sut = GetSut();
        var isValidRequest = sut.ValidateRequest(paymentRequest, new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs });
        isValidRequest.Success.Should().BeTrue(); 
    }

    private BacsPaymentStrategy GetSut()
    {
        return new BacsPaymentStrategy(); 
    }
}


public class BacsPaymentStrategy : IPaymentStrategy
{
    public bool Applies(MakePaymentRequest paymentRequest) => paymentRequest.PaymentScheme == PaymentScheme.Bacs;
    
    public MakePaymentResult ValidateRequest(MakePaymentRequest paymentRequest, Account account)
    {
        if (!Applies(paymentRequest))
        {
            return new MakePaymentResult {Success= false};
        }
        
        //Ths can be further refactored
        if (account == null)
        {
            return new MakePaymentResult {Success= false};
        }

        if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
        {
            return new MakePaymentResult {Success= false};
        }
        
        return new MakePaymentResult {Success= true};
    }
}

public interface IPaymentStrategy
{
    bool Applies(MakePaymentRequest paymentRequest);

    MakePaymentResult ValidateRequest(MakePaymentRequest paymentRequest, Account account);
}