using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Services;

public class FasterPaymentSchemeStrategyTests
{
    const int Balance = 100;
    
    [Fact]
    public void WhenAccountIsNull_ThenPaymentResultIsUnsuccessful()
    {
        var paymentRequest = GetPaymentRequest(PaymentScheme.FasterPayments); 
        var sut = GetSut();
        var isValidRequest = sut.ValidatePaymentRequest(paymentRequest, account:null);
        isValidRequest.Success.Should().BeFalse(); 
    }

   
    [Fact]
    public void WhenAccountAndPaymentIsFasterPayments_ThenPaymentResultIsSuccessful()
    {
        var paymentRequest =  GetPaymentRequest(PaymentScheme.FasterPayments); 
        var sut = GetSut();
        var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments }
            .WithBalance(Balance);
        
        var isValidRequest = sut.ValidatePaymentRequest(paymentRequest, account);
        isValidRequest.Success.Should().BeTrue(); 
    }

    [Fact] 
    public void WhenAccountAndPaymentIsFasterPayments_AndBalanceIsLessThanPaymentAmount_ThenPaymentResultIsSuccessful()
    {   var paymentAmount = Balance * 2;
        var paymentRequest =  GetPaymentRequest(PaymentScheme.FasterPayments); 
        paymentRequest.Amount = paymentAmount;
        var sut = GetSut();
        
        
        var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments
            }
            .WithBalance(Balance);
        
        var isValidRequest = sut.ValidatePaymentRequest(paymentRequest, account);
        isValidRequest.Success.Should().BeFalse(); 
    }
    
    [Theory]
    [InlineData(AllowedPaymentSchemes.Chaps)]
    [InlineData(AllowedPaymentSchemes.Bacs)]
    public void WhenAccountIsNotFasterPayment_ThenPaymentResultIsUnsuccessful(AllowedPaymentSchemes allowedPaymentSchemes)
    {
        var paymentRequest =  GetPaymentRequest(PaymentScheme.FasterPayments); 
        var sut = GetSut();
        var isValidRequest = sut.ValidatePaymentRequest(paymentRequest, new Account { AllowedPaymentSchemes = allowedPaymentSchemes });
        isValidRequest.Success.Should().BeFalse(); 
    }
    
    [Theory]
    [InlineData(PaymentScheme.Chaps)]
    [InlineData(PaymentScheme.Bacs)]
    public void WhenPaymentRequestIsNotFasterPayments_ThenPaymentResultIsUnsuccessful(PaymentScheme paymentScheme)
    {
        var paymentRequest = GetPaymentRequest(paymentScheme);
        var sut = GetSut();
        var isValidRequest = sut.ValidatePaymentRequest(paymentRequest, new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments });
        isValidRequest.Success.Should().BeFalse(); 
    }
    
    private static MakePaymentRequest GetPaymentRequest(PaymentScheme paymentScheme)
    {
        return new MakePaymentRequest { PaymentScheme = paymentScheme, Amount = Balance / 2};
    }

   
    private FasterPaymentsPaymentStrategy GetSut()
    {
        return new FasterPaymentsPaymentStrategy(); 
    }
}