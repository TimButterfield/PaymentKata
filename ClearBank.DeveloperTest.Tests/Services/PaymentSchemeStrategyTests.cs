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


public interface IPaymentStrategy
{
    bool Applies(MakePaymentRequest paymentRequest);

    MakePaymentResult ValidateRequest(MakePaymentRequest paymentRequest, Account account);
}