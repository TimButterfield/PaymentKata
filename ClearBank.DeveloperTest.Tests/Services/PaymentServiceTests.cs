using System;
using System.Collections.Generic;
using System.Linq;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Services;

public class PaymentServiceTests
{
    //placeholder
}

public class PaymentSchemeStrategyTests
{
    [Theory]
    [InlineData(PaymentScheme.Bacs, typeof(BacsPaymentStrategy))]
    [InlineData(PaymentScheme.FasterPayments, typeof(FasterPaymentsPaymentStrategy))]
    [InlineData(PaymentScheme.Chaps, typeof(ChapsPaymentsPaymentStrategy))]
    public void WhenSelectingTheRightStrategy_ThenTheRightStrategyIsSelected(PaymentScheme paymentScheme, Type type)
    {
         var paymentSchemeStrategy = new List<IPaymentStrategy>
         {
             new BacsPaymentStrategy(), 
             new FasterPaymentsPaymentStrategy(), 
             new ChapsPaymentsPaymentStrategy()
         };

         var paymentRequest = new MakePaymentRequest { PaymentScheme = paymentScheme };

         var selectedPaymentStrategy = paymentSchemeStrategy.FirstOrDefault(x => x.Applies(paymentRequest));

         selectedPaymentStrategy.Should().NotBeNull();
         selectedPaymentStrategy.Should().BeOfType(type);

    }
}