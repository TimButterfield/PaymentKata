using System.Diagnostics;
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
        var isValidRequest = sut.ValidateRequest(paymentRequest, account:null);
        isValidRequest.Success.Should().BeFalse(); 
    }
    
    [Fact]
    public void WhenAccountAndPaymentIsBacs_ThenPaymentResultIsSuccessful()
    {
        var paymentRequest = new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs}; 
        var sut = GetSut();
        var isValidRequest = sut.ValidateRequest(paymentRequest, new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs });
        isValidRequest.Success.Should().BeTrue(); 
    }
    
    [Theory]
    [InlineData(AllowedPaymentSchemes.Chaps)]
    [InlineData(AllowedPaymentSchemes.FasterPayments)]
    public void WhenAccountIsNotBacs_ThenPaymentResultIsUnsuccessful(AllowedPaymentSchemes allowedPaymentSchemes)
    {
        var paymentRequest = new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs}; 
        var sut = GetSut();
        var isValidRequest = sut.ValidateRequest(paymentRequest, new Account { AllowedPaymentSchemes = allowedPaymentSchemes });
        isValidRequest.Success.Should().BeFalse(); 
    }
    
    [Theory]
    [InlineData(PaymentScheme.Chaps)]
    [InlineData(PaymentScheme.FasterPayments)]
    public void WhenPaymentRequestIsNotBacs_ThenPaymentResultIsUnsuccessful(PaymentScheme paymentScheme)
    {
        var paymentRequest = new MakePaymentRequest { PaymentScheme = paymentScheme}; 
        var sut = GetSut();
        var isValidRequest = sut.ValidateRequest(paymentRequest, new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs });
        isValidRequest.Success.Should().BeFalse(); 
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
        var accountIsNull = account == null;
        bool AccountIsNotBacs () => !account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs); 
        
        if (!Applies(paymentRequest) || accountIsNull || AccountIsNotBacs())
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