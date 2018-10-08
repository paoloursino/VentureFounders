# Technical Test

The solution contains a .NET core library (TechnicalTest.App) which is structured into the following 3 folders:

* Domain - this contains the domain models for a user and an account, and a notification service.
* Features - this contains two operations, one which is implemented (transfer money) and another which isn't (withdraw money)
* DataAccess - this contains a repository for retrieving and saving an account (and the nested user it belongs to)

## The task

The task is to implement a money withdrawal in the WithdrawMoney.Execute(...) method in the features folder. For consistency, the logic should be the same as the TransferMoney.Execute(...) method i.e. notifications for low funds and exceptions where the operation is not possible. 

As part of this process however, you should look to refactor some of the code in the TransferMoney.Execute(...) method into the domain models, and make these models less susceptible to misuse. We're looking to make our domain models rich in behaviour and much more than just plain old objects, however we don't want any data persistance operations (i.e. data access repositories) to bleed into our domain. This should simplify the task of implementing WithdrawMoney.Execute(...).

## Guidelines

* You should spend no more than 1 hour on this task, although there is no time limit
* You should fork or copy this repository into your own public repository (Github, BitBucket etc.) before you do your work
* You should not alter the notification service or the the account repository interfaces
* You may add unit/integration tests using a test framework (and/or mocking framework) of your choice
* You may edit this README.md if you want to give more details around your work (e.g. why you have done something a particular way, or anything else you would look to do but didn't have time)

Once you have completed your work, send us a link to your public repository at pete@venturefounders.co.uk

Let us know if you have any questions or problems.

Good luck!

## Solution
The task consists in moving some of the functionality of Withdraw/Execute into the Account class, which can better and more securely handle requests. It was also requested to make the overall code more secure and prevent potential misuse.

Even if the task is simple, there are various options and choices to make. This is what I decided to do:

1. In Account: created two member functions: WithdrawMoney and PayMoney. The validation logic, including exceptions and notifications, is now fully controlled by these functions

2. in Account: 'balance', 'paidIn' and 'withdrawn', are now private and read-only properties; 'guid' and 'user' are read-only. I added a simple constructor to make tests possible. However, a better and more secure constructor for 'Account' should be implemented in production. Ideally, for consistency as well as security, the class Account should be, if not unmutable, tightly controlled. 

3. In Account: added INotificationservices interface. Initially, I didn't particularly like this approach. However, it doesn't create any issue or overhead with the storage. Correspondingly, I removed the interface from TransferMoney and WithdrawMoney as not used anymore.

4. TransferMoney and WithdrawMoney Execute functions: these are now very simple

In order to do some minimal unit tests, I created a separate test project. The INotificationService and IAccountRepository interfaces need some implementation. In a real case, these would be most likely external services/libraries, injected via Dependency Injection. Here, for the purpose of running some simple tests quickly, I manually provided a trivial implementation and manual binding in the Unit Test module. 
I created just a couple of basic tests, please note that the implementation of the repository is really not there, it would have requested just a bit more time to do something suitable for the tests. However, it should be self evident how it is possible to implement further tests.

Some notes on the current code:

-When paying money into an account the paidIn property is incremented by the amount paid in. Therefore, paidIn is an accumulator; it is the sum of all money paid into the account since creation. Inevitably, later on, it will be impossible to pay in money to that account: when the test 'paidIn' + 'amount' exceeds the 'paidInLimit' an exception is thrown. And similar issue for the paid amount close to the limit, in this case a notification is sent. This didn't seem correct to me: wWhy would it be a problem for an account to receive many successive amounts? In time any account would be unable to receive money. Maybe the intention was to limit the amount paid in to a certain amount, but within a time window?

-The 'withdrawn' property is similar to 'paidIn', it decrements according to the amount withdrawn. However, there is no test on this amount when withdrawing in the original code, so the problem above wouldn't occur. Also, in my view it would be more logical for 'withdrawn' to increment, not decrement, when a certain amount is withdrawn. It would be an accumulator (positive) showing the total amount withdrawn.

-If a newly created account has 'balance' == 'paidIn' == 'withdrawn' == 0, when created, then at any later time the condition 'balance' == 'paidIn' - 'withdrawn' (assuming 'withdrawn' is positive) must be true. And this could be tested with a check() method, which could assert, or send other notifications, when called.

I didn't do the code changes to implement/correct the above points. 

