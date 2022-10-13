using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Services;

public class ChapsPaymentSchemeStrategyTests
{
    [Fact]
    public void WhenAccountIsNull_ThenPaymentResultIsUnsuccessful()
    {
        var paymentRequest = GetPaymentRequest(PaymentScheme.Chaps); 
        var sut = GetSut();
        var isValidRequest = sut.ValidateRequest(paymentRequest, account:null);
        isValidRequest.Success.Should().BeFalse(); 
    }

   
    [Fact]
    public void WhenAccountAndPaymentIsChapsPayments_ThenPaymentResultIsSuccessful()
    {
        var paymentRequest =  GetPaymentRequest(PaymentScheme.Chaps); 
        var sut = GetSut();
        var isValidRequest = sut.ValidateRequest(paymentRequest, new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps });
        isValidRequest.Success.Should().BeTrue(); 
    }
    
    [Theory]
    [InlineData(AllowedPaymentSchemes.FasterPayments)]
    [InlineData(AllowedPaymentSchemes.Bacs)]
    public void WhenAccountIsNotFasterPayment_ThenPaymentResultIsUnsuccessful(AllowedPaymentSchemes allowedPaymentSchemes)
    {
        var paymentRequest =  GetPaymentRequest(PaymentScheme.Chaps); 
        var sut = GetSut();
        var isValidRequest = sut.ValidateRequest(paymentRequest, new Account { AllowedPaymentSchemes = allowedPaymentSchemes });
        isValidRequest.Success.Should().BeFalse(); 
    }
    
    [Theory]
    [InlineData(PaymentScheme.FasterPayments)]
    [InlineData(PaymentScheme.Bacs)]
    public void WhenPaymentRequestIsNotFasterPayments_ThenPaymentResultIsUnsuccessful(PaymentScheme paymentScheme)
    {
        var paymentRequest = GetPaymentRequest(paymentScheme);
        var sut = GetSut();
        var isValidRequest = sut.ValidateRequest(paymentRequest, new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps });
        isValidRequest.Success.Should().BeFalse(); 
    }
    
    private static MakePaymentRequest GetPaymentRequest(PaymentScheme paymentScheme)
    {
        return new MakePaymentRequest { PaymentScheme = paymentScheme};
    }

   
    private ChapsPaymentsPaymentStrategy GetSut()
    {
        return new ChapsPaymentsPaymentStrategy(); 
    }
}