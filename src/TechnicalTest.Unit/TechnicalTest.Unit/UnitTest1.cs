using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechnicalTest.App.Domain.Services;
using TechnicalTest.App.DataAccess;
using TechnicalTest.App;
using System;
using TechnicalTest.App.Features;

namespace TechnicalTest.Unit
{
    public class DummyNotification : INotificationService
    {
        public void NotifyApproachingPayInLimit(string emailAddress)
        {
            Console.WriteLine(emailAddress);
        }

        public void NotifyFundsLow(string emailAddress)
        {
            Console.WriteLine(emailAddress);
        }
    }
    public class DummyRepository:IAccountRepository
    {
        public Account GetAccountById(Guid accountId)
        {
            return new Account(new Guid(), new User(), 1000m, 0m, 0m, new DummyNotification());
        }

        public void Update(Account account)
        {
            ;
        }

    }


    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestAccount()
        {
            Account tgt = new Account(new Guid(), new User(), 0m,0m,0m,new DummyNotification());
            var initialBalance = tgt.Balance;
            tgt.PayMoney(100);
            tgt.WithdrawMoney(100);
            tgt.PayMoney(500);
            tgt.WithdrawMoney(500);
            Assert.AreEqual(tgt.Balance,initialBalance);
        }
        [TestMethod]
        public void TestTransfer()
        {
            TransferMoney tgt = new TransferMoney(new DummyRepository());
            tgt.Execute(new Guid(), new Guid(), 1001m);
            
        }
    }
}
