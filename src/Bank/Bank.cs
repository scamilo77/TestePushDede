using Communicator;
using Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Logging;
using System.Diagnostics.CodeAnalysis;

namespace Banking
{
    public class Bank
    {
        public readonly IVault Vault;
        public readonly IApiCaller apiCaller;        
        public readonly ILogger logger;        

        public Bank(IVault vault, IApiCaller _apiCaller, ILogger _logger)
        {
            Vault = vault;
            apiCaller = _apiCaller;
            logger = _logger;
        }

        #region Logger

        //public readonly ILogger logger;


        #endregion

        public void Deposit(Guid accountId, decimal value)
        {
            Vault.Deposit(accountId, value);
        }

        public void Withdraw(Guid accountId, decimal value)
        {
            Vault.Withdraw(accountId, value);
        }

        public async Task<string> RegisterCustomer(Customer customer)
        {
            var response = await apiCaller.Call(new Uri("http://localhost:5000/api/customer/create"), HttpMethod.Post, customer);
            var message = await response.Content.ReadAsStringAsync();

            #region Inner Method

            //ogger.Log(message);

            #endregion

            #region Static

            //Logger.Log(text);

            #endregion

            #region Injected

            logger.Log(message);

            #endregion

            return message;
        }

        public async Task<string> GetCustomer(string id)
        {
            var response = await apiCaller.Call(new Uri($"http://localhost:5000/api/customer/get/{id}"), HttpMethod.Get);
            var message = await response.Content.ReadAsStringAsync();

            #region Inner Method

            //Logger.Log(message);

            #endregion

            #region Static

            //Logger.Log(text);

            #endregion

            #region Injected

            //Injected
            logger.Log(message);

            #endregion

            return message;
        }

        public void AddAccount(Account account)
        {
            Vault.Add(account);
        }

        #region Inner Logging Method

        //public void Log(string text)
        //{
        //    using (var sw = new StreamWriter("Log.txt", true))
        //    {
        //        sw.WriteLine(text);
        //    }
        //}

        #endregion
    }

    [ExcludeFromCodeCoverage]
    public class xpto
    {
        public string CardNumber { get; private set; }
        public decimal TransactionAmount { get; private set; }
        public string CardInfo { get; private set; }
        public string Track2 { get; private set; }
    }

    public class XptoFormatter : IXptoFormatter
    {
        
    }

    public interface IXptoFormatter
    {

    }
}
