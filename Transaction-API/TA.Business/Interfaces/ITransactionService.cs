using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TA.Business.Dto;
using TA.Data.Entities;

namespace TA.Business.Interfaces
{
    public interface ITransaction
    {
        Task<Transaction> AddTransaction(string status, string type, string clientName, decimal amount);
        Task<List<Transaction>> GetAllTransactions();
        Task<Transaction> UpdateTransaction(int id, string status);
        Task<Transaction> DeleteTransaction(int id);
    }
}
