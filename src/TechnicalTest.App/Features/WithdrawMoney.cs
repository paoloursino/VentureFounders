using TechnicalTest.App.DataAccess;
using TechnicalTest.App.Domain.Services;
using System;

namespace TechnicalTest.App.Features
{
    public class WithdrawMoney
    {
        private IAccountRepository accountRepository;

        public WithdrawMoney(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public void Execute(Guid fromAccountId, decimal amount)
        {
            var from = this.accountRepository.GetAccountById(fromAccountId);

            try
            {
                from.WithdrawMoney(amount);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Detailed explanaion of the exception to be logged here: {0}", ex.Message);
                throw;
            }
            this.accountRepository.Update(from);
        }
    }
}
