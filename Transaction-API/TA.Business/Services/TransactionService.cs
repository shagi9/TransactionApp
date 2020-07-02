using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TA.Business.Interfaces;
using TA.Data.DataContext;
using TA.Data.Entities;
using TA.Business.Helpers;
using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using System.IO;
using System;

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
        public XLWorkbook getData()
        {
            List<Transaction> transactions = _context.Transactions.ToList();
            var workbook = new XLWorkbook();
            IXLWorksheet worksheet =
                    workbook.Worksheets.Add("Transactions");
            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "Status";
            worksheet.Cell(1, 3).Value = "Type";
            worksheet.Cell(1, 4).Value = "ClientName";
            worksheet.Cell(1, 5).Value = "Amount";
            for (int index = 1; index <= transactions.Count; index++)
            {
                worksheet.Cell(index + 1, 1).Value =
                transactions[index - 1].Id;
                worksheet.Cell(index + 1, 2).Value =
                transactions[index - 1].Status;
                worksheet.Cell(index + 1, 3).Value =
                transactions[index - 1].Type;
                worksheet.Cell(index + 1, 4).Value =
                transactions[index - 1].ClientName;
                worksheet.Cell(index + 1, 5).Value =
                transactions[index - 1].Amount;
            }
            return workbook;
        }
    }
}
