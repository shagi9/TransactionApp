using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TA.Business.Interfaces;
using TA.Data.DataContext;
using TA.Data.Entities;
using TA.Business.Helpers;

namespace TA.Business.Services
{
    public class TransactionService: ITransactionService
    {
        private readonly TransactionDbContext _context;
        public TransactionService(TransactionDbContext context)
        {
            _context = context;
        }

        // Add new transaction
        public async Task<Transaction> AddTransaction(string status, string type, string clientName, decimal amount)
        {
            Transaction newTransaction = new Transaction
            {
                Status = status,
                Type = type,
                ClientName = clientName,
                Amount = amount
            };
            await _context.Transactions.AddAsync(newTransaction);
            await _context.SaveChangesAsync();
            return newTransaction;
        }
        
        // Get all transactions with pagination and filtering
        public async Task<PagedList<Transaction>> GetAllTransactions(UserParams userParams)
        {
            var transactions = _context.Transactions.AsQueryable();

            if (!string.IsNullOrEmpty(userParams.OrderByStatus))
            {
                transactions = transactions.Where(s => s.Status.Contains(userParams.OrderByStatus));
            }
            if (!string.IsNullOrEmpty(userParams.OrderByType))
            {
                transactions = transactions.Where(s => s.Type.Contains(userParams.OrderByType));
            }

            return await PagedList<Transaction>.CreateAsync(transactions, userParams.PageNumber, userParams.PageSize);
        }

        // Update status of transaction
        public async Task<Transaction> UpdateTransaction(int id, string status)
        {
            var transaction = await _context.Transactions.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
            var newTransaction = new Transaction
            {
                Id = id,
                Status = status
            };
            _context.Transactions.Attach(newTransaction).Property(s => s.Status).IsModified = true;
            await _context.SaveChangesAsync();
            return newTransaction;
        }

        // Delete transaction
        public async Task<Transaction> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }
    }
}
