using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TA.Data.Entities;
using X.PagedList;

namespace TA.Business.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> AddTransaction(string status, string type, string clientName, decimal amount);
        Task<List<Transaction>> Filtering(string sortOrder); 
        Task<IPagedList<Transaction>> Pagination(int? page);
        Task<List<Transaction>> GetAllTransactions();
        Task<Transaction> UpdateTransaction(int id, string status);
        Task<Transaction> DeleteTransaction(int id);
    }
}
