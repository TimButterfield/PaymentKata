using System.Collections.Generic;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Services;

public class PaymentServiceTests
{
    const int Balance = 1000;
    const int PaymentAmount = Balance / 2;
    const string DebtorAccountNumber = "123456789";
    
    [Fact]
    private void WhenPaymentRequestIsNotValid_ThenAccountIsNotUpdated()
    {
        //arrange
        var dataStore = Substitute.For<IDataStore>();
        var paymentStrategy = Substitute.For<IPaymentStrategy>();
        paymentStrategy.Applies(Arg.Any<MakePaymentRequest>()).Returns(true); 
        paymentStrategy.ValidateRequest(Arg.Any<MakePaymentRequest>(), Arg.Any<Account>()).Returns(new MakePaymentResult{Success = false}); 
            
        var sut = new PaymentService(dataStore, GetPaymentStrategies(paymentStrategy));
        
        var paymentRequest = new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs, DebtorAccountNumber =  DebtorAccountNumber};
        
        //act
        var result = sut.MakePayment(paymentRequest);

        //assert
        dataStore.Received(1).GetAccount(DebtorAccountNumber);
        dataStore.DidNotReceive().UpdateAccount(Arg.Any<Account>());
        result.Success.Should().BeFalse();
    }
    
    [Fact]
    private void WhenPaymentRequestIsValid_AndAccountFound_ThenBalanceIsUpdated()
    {
        //arrange
        var expectedBalance = Balance - PaymentAmount;
        var dataStore = Substitute.For<IDataStore>();
        var paymentStrategy = Substitute.For<IPaymentStrategy>();
        var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs }
            .WithBalance(Balance); 
        
        dataStore.GetAccount(Arg.Any<string>()).Returns(account);
        paymentStrategy.Applies(Arg.Any<MakePaymentRequest>()).Returns(true); 
        paymentStrategy.ValidateRequest(Arg.Any<MakePaymentRequest>(), Arg.Any<Account>()).Returns(new MakePaymentResult{Success = true}); 
        
        var sut = new PaymentService(dataStore, GetPaymentStrategies(paymentStrategy));
        var paymentRequest = new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs, DebtorAccountNumber =  DebtorAccountNumber, Amount = PaymentAmount};
        
        //act
        var result = sut.MakePayment(paymentRequest);

        //assert
        dataStore.Received(1).GetAccount(DebtorAccountNumber);
        dataStore.Received(1).UpdateAccount(Arg.Is<Account>(x => x.Balance == expectedBalance));
        result.Success.Should().BeTrue();
    }
    
    //This test demonstrates a bug in the code, caused by default success state of true. 
    //Need to see it's desired behaviour before removing!
    [Fact]
    private void WhenPaymentSchemeIsNotInRange_ThenBalancesIsStillUpdated()
    {
        var invalidPaymentScheme = (PaymentScheme)10;
        var dataStore = Substitute.For<IDataStore>();
        var expectedBalance = Balance - PaymentAmount;
        var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs }
            .WithBalance(Balance); 
        
        dataStore.GetAccount(Arg.Any<string>()).Returns(account);
        var paymentStrategy = Substitute.For<IPaymentStrategy>();
        paymentStrategy.Applies(Arg.Any<MakePaymentRequest>()).Returns(false);
        
        var sut = new PaymentService(dataStore, GetPaymentStrategies(paymentStrategy));
        var paymentRequest = new MakePaymentRequest { PaymentScheme = invalidPaymentScheme, DebtorAccountNumber =  DebtorAccountNumber, Amount = PaymentAmount};
        
        //act
        var result = sut.MakePayment(paymentRequest);

        dataStore.Received(1).GetAccount(DebtorAccountNumber);
        dataStore.Received(1).UpdateAccount(Arg.Is<Account>(x => x.Balance == expectedBalance));
        result.Success.Should().BeTrue();
    }

    private IEnumerable<IPaymentStrategy> GetPaymentStrategies(IPaymentStrategy paymentStrategy)
    {
        return new List<IPaymentStrategy>
        {
            paymentStrategy
        };
    }
}