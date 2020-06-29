using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TA.Business.Interfaces;
using TA.Data.DataContext;
using TA.Data.Entities;
using X.PagedList;
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;
using ClosedXML.Excel;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Web;

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
        
        // pagination
        public async Task<IPagedList<Transaction>> Pagination(int? page)
        {
            var pageNumber = page ?? 1;
            int pageSize = 2;
            var onepage = await _context.Transactions.ToPagedListAsync(pageNumber, pageSize);
            return onepage;
        }

        // Filtering by type
        public async Task<List<Transaction>> FilteringByType(string sortByType)
        {
            var transactions = await _context.Transactions
            .Where(x => x.Type.Contains(sortByType))
            .ToListAsync();

            switch (sortByType)
            {
                case "Refill":
                    transactions = transactions.OrderBy(x => x.Type).ToList();
                    break;
                case "Withdrawal":
                    transactions = transactions.OrderBy(x => x.Type).ToList();
                    break;
            }
            return transactions;
        }

        // Filtering by status
        public async Task<List<Transaction>> FilteringByStatus(string sortByStatus)
        {
            var transactions = await _context.Transactions
            .Where(x => x.Status.Contains(sortByStatus))
            .ToListAsync();
            
            switch (sortByStatus)
            {
                case "Pending":
                    transactions = transactions.OrderBy(x => x.Status).ToList();
                break;
                case "Cancelled":
                    transactions = transactions.OrderBy(x => x.Status).ToList();
                break;
                case "Completed":
                    transactions = transactions.OrderBy(x => x.Status).ToList();
                break;
            }
            return transactions;
        }

        // Get all transactions
        public async Task<List<Transaction>> GetAllTransactions()
        {
            List<Transaction> transactions = new List<Transaction>();
            transactions = await _context.Transactions.ToListAsync();
            return transactions;
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
