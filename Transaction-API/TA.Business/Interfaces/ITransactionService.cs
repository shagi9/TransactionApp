using ClosedXML.Excel;
using System.Collections.Generic;
using System.Threading.Tasks;
using TA.Business.Helpers;
using TA.Data.Entities;
using TA.Business.Models;

namespace TA.Business.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> AddTransaction(string status, string type, string clientName, decimal amount);
        Task<PagedList<Transaction>> GetAllTransactions(UserParams userParams);
        Task<Transaction> UpdateTransaction(UpdateTransactionVm updateTransactionVm);
        Task<Transaction> DeleteTransaction(int id);
        public XLWorkbook getData();
    }
}
