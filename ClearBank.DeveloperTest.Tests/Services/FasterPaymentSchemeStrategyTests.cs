using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Services;

public class FasterPaymentSchemeStrategyTests
{
    [Fact]
    public void WhenAccountIsNull_ThenPaymentResultIsUnsuccessful()
    {
        var paymentRequest = GetPaymentRequest(PaymentScheme.FasterPayments); 
        var sut = GetSut();
        var isValidRequest = sut.ValidateRequest(paymentRequest, account:null);
        isValidRequest.Success.Should().BeFalse(); 
    }

   
    [Fact]
    public void WhenAccountAndPaymentIsFasterPayments_ThenPaymentResultIsSuccessful()
    {
        var paymentRequest =  GetPaymentRequest(PaymentScheme.FasterPayments); 
        var sut = GetSut();
        var isValidRequest = sut.ValidateRequest(paymentRequest, new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments });
        isValidRequest.Success.Should().BeTrue(); 
    }
    
    [Theory]
    [InlineData(AllowedPaymentSchemes.Chaps)]
    [InlineData(AllowedPaymentSchemes.Bacs)]
    public void WhenAccountIsNotFasterPayment_ThenPaymentResultIsUnsuccessful(AllowedPaymentSchemes allowedPaymentSchemes)
    {
        var paymentRequest =  GetPaymentRequest(PaymentScheme.FasterPayments); 
        var sut = GetSut();
        var isValidRequest = sut.ValidateRequest(paymentRequest, new Account { AllowedPaymentSchemes = allowedPaymentSchemes });
        isValidRequest.Success.Should().BeFalse(); 
    }
    
    [Theory]
    [InlineData(PaymentScheme.Chaps)]
    [InlineData(PaymentScheme.Bacs)]
    public void WhenPaymentRequestIsNotFasterPayments_ThenPaymentResultIsUnsuccessful(PaymentScheme paymentScheme)
    {
        var paymentRequest = GetPaymentRequest(paymentScheme);
        var sut = GetSut();
        var isValidRequest = sut.ValidateRequest(paymentRequest, new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments });
        isValidRequest.Success.Should().BeFalse(); 
    }
    
    private static MakePaymentRequest GetPaymentRequest(PaymentScheme paymentScheme)
    {
        return new MakePaymentRequest { PaymentScheme = paymentScheme};
    }

   
    private FasterPaymentsPaymentStrategy GetSut()
    {
        return new FasterPaymentsPaymentStrategy(); 
    }
}