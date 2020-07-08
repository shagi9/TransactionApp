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
using TA.Business.Models;
using DocumentFormat.OpenXml.Office2010.PowerPoint;

namespace TA.Business.Services
{
    public class TransactionService: ITransactionService
    {
        private readonly TransactionDbContext _context;
        public TransactionService(TransactionDbContext context)
        {
            _context = context;
        }

        // add or update transaction
        public async Task<Transaction> AddOrUpdateTransaction(AddOrUpdateTransactionVm addOrUpdateTransaction)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == addOrUpdateTransaction.Id);
            
            if (transaction == null)
            {
                Transaction newTransaction = new Transaction
                {
                    Status = addOrUpdateTransaction.Status,
                    Type = addOrUpdateTransaction.Type,
                    ClientName = addOrUpdateTransaction.ClientName,
                    Amount = addOrUpdateTransaction.Amount
                };
                await _context.Transactions.AddAsync(newTransaction);
                await _context.SaveChangesAsync();
                return newTransaction;
            }
            else
            {
                transaction.Status = addOrUpdateTransaction.Status;
                transaction.Type = addOrUpdateTransaction.Type;
                transaction.ClientName = addOrUpdateTransaction.ClientName;
                transaction.Amount = addOrUpdateTransaction.Amount;
                _context.Transactions.Update(transaction);
                await _context.SaveChangesAsync();
                return transaction;
            }
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
        public async Task<Transaction> UpdateStatusOfTransaction(UpdateStatusOfTransactionVm updateTransactionVm)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == updateTransactionVm.Id);

            if (transaction == null)
            {
                return default;
            }
            else
            {
                transaction.Status = updateTransactionVm.Status;
                _context.Transactions.Update(transaction);
                await _context.SaveChangesAsync();
                return transaction;
            }
        }

        // Delete transaction
        public async Task<Transaction> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        // export to excel
        public XLWorkbook getData()
        {
            var transactions = _context.Transactions.ToList();
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
