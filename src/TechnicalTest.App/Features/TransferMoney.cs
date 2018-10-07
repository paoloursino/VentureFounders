using TechnicalTest.App.DataAccess;
using TechnicalTest.App.Domain.Services;
using System;

namespace TechnicalTest.App.Features
{
    public class TransferMoney
    {
        private IAccountRepository accountRepository;

        public TransferMoney(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public void Execute(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            var from = this.accountRepository.GetAccountById(fromAccountId);
            var to = this.accountRepository.GetAccountById(toAccountId);

            try
            {
                from.WithdrawMoney(amount);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Detailed explanaion of the exception to be logged here: {0}", ex.Message);
                throw;
            }
            try
            {
                to.PayMoney(amount);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Detailed explanaion of the exception to be logged here: {0}", ex.Message);
                throw;
            }

            accountRepository.Update(from);
            accountRepository.Update(to);
        }
    }
}
