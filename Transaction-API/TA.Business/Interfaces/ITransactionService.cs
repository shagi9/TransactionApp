using ClosedXML.Excel;
using System.Collections.Generic;
using System.Threading.Tasks;
using TA.Business.Helpers;
using TA.Data.Entities;
using TA.Business.Models;
using System;

namespace TA.Business.Interfaces
{
    public interface ITransactionService
    {
        Task<PagedList<Transaction>> GetAllTransactions(UserParams userParams);
        Task<Transaction> AddOrUpdateTransaction(AddOrUpdateTransactionVm addOrUpdateTransaction);
        Task<Transaction> UpdateStatusOfTransaction(UpdateStatusOfTransactionVm updateTransactionVm);
        Task<Transaction> DeleteTransaction(int id);
        public XLWorkbook getData();
    }
}
