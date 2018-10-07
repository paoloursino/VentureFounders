using System;
using TechnicalTest.App.Domain.Services;


namespace TechnicalTest.App
{
    public class Account
    {
        private INotificationService notificationService;

        public const decimal PayInLimit = 4000m;
        public const decimal LowLimit = 500m;

        public Guid Id { get; }

        public User User { get; }
        private decimal balance;
        public decimal Balance
        {
            get
            {
                return balance;
            }
        }
        private decimal withdrawn;
        public decimal Withdrawn
        {
            get
            {
                return withdrawn;
            }
        }
        private decimal paidIn;
        public decimal PaidIn
        {
            get
            {
                return paidIn;
            }
        }
        public Account(Guid Id, User User, decimal balance, decimal paidIn, decimal withdrawn,INotificationService notificationService)
        {
            this.Id = Id;
            this.User = User;
            this.balance = balance;
            this.paidIn = paidIn;
            this.withdrawn = withdrawn;
            this.notificationService = notificationService;
        }
        public void WithdrawMoney(decimal amount)
        {
            var newBalance = balance - amount;

            if (newBalance < 0m)
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }

            if (newBalance < LowLimit)
            {
                this.notificationService.NotifyFundsLow(User.Email);
            }

            balance = balance - amount;
            withdrawn = withdrawn - amount;

        }
        public void PayMoney(decimal amount)
        {
            var newPaidIn = paidIn + amount;

            if (newPaidIn > PayInLimit)
            {
                throw new InvalidOperationException("Account pay in limit reached");
            }
            if (Account.PayInLimit - paidIn < LowLimit)
            {
                this.notificationService.NotifyApproachingPayInLimit(User.Email);
            }

            balance = balance + amount;
            paidIn = paidIn + amount;
        }
    }
}
