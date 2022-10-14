using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Services;

public class PaymentSchemeStrategyTests
{
    [Theory]
    [InlineData(PaymentScheme.Bacs, typeof(BacsPaymentStrategy))]
    [InlineData(PaymentScheme.FasterPayments, typeof(FasterPaymentsPaymentStrategy))]
    [InlineData(PaymentScheme.Chaps, typeof(ChapsPaymentsPaymentStrategy))]
    public void WhenSelectingTheRightStrategy_ThenTheRightStrategyIsSelected(PaymentScheme paymentScheme, Type type)
    {
        var paymentStrategies = GetPaymentStrategies();
        var paymentRequest = new MakePaymentRequest { PaymentScheme = paymentScheme };

        var selectedPaymentStrategy = paymentStrategies.FirstOrDefault(x => x.Applies(paymentRequest));

        selectedPaymentStrategy.Should().NotBeNull();
        selectedPaymentStrategy.Should().BeOfType(type);

    }
    
    [Fact]
    public void WhenSelectingAStrategyForANoneExistentScheme_ThenNoStrategyIsSelected()
    {
        
        var paymentStrategies = GetPaymentStrategies();
        var paymentRequest = new MakePaymentRequest { PaymentScheme = (PaymentScheme)1000 };

        var selectedPaymentStrategy = paymentStrategies.FirstOrDefault(x => x.Applies(paymentRequest));

        selectedPaymentStrategy.Should().BeNull();

    }

    private static IEnumerable<IPaymentStrategy> GetPaymentStrategies()
    {
        //Consider using reflection
        var paymentStrategies = new List<IPaymentStrategy>
        {
            new BacsPaymentStrategy(),
            new FasterPaymentsPaymentStrategy(),
            new ChapsPaymentsPaymentStrategy()
        };
        return paymentStrategies;
    }
}