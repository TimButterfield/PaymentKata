using System.Collections.Generic;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using System.Linq;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IDataStore _dataStore;
        private readonly IEnumerable<IPaymentStrategy> _paymentStrategies;

        public PaymentService(IDataStore dataStore, IEnumerable<IPaymentStrategy> paymentStrategies)
        {
            _dataStore = dataStore;
            _paymentStrategies = paymentStrategies;
        }
 
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            //TODO : Discuss this, default success of true. Is this really desired behaviour
            var result = new MakePaymentResult { Success = true };
            var account = _dataStore.GetAccount(request.DebtorAccountNumber);
            var strategy = _paymentStrategies.FirstOrDefault(x => x.Applies(request));
            
            if (strategy != null)
            {
                result = strategy.ValidatePaymentRequest(request, account); 
            }

            if (result.Success)
            {
                ProcessPayment(request, account);
            }

            return result;
        }

        private void ProcessPayment(MakePaymentRequest request, Account account)
        {
            account.DeductPayment(request.Amount);
            _dataStore.UpdateAccount(account);
        }
    }
}
