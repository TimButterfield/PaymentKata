namespace ClearBank.DeveloperTest.Types
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; private set; }
        public AccountStatus Status { get; set; }
        public AllowedPaymentSchemes AllowedPaymentSchemes { get; set; }

        public void DeductPayment(decimal paymentAmount)
        {
            Balance -= paymentAmount;
        }

        public Account WithBalance(decimal balance)
        {
            Balance = balance;
            return this;
        }
    }
}
