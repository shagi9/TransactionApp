using System.Collections.Generic;
using System.Threading.Tasks;
using TA.Business.Helpers;
using TA.Data.Entities;

namespace TA.Business.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> AddTransaction(string status, string type, string clientName, decimal amount);
        Task<PagedList<Transaction>> GetAllTransactions(UserParams userParams);
        Task<Transaction> UpdateTransaction(int id, string status);
        Task<Transaction> DeleteTransaction(int id);
    }
}
