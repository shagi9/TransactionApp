using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TA.Business.Interfaces;
using TA.Data.DataContext;
using TA.Data.Entities;
using TA.Business.Helpers;
using ClosedXML.Excel;
using TA.Business.Models;
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

        // add or update transaction
        public async Task<ServiceResponse<Transaction>> AddOrUpdateTransaction(AddOrUpdateTransactionVm addOrUpdateTransaction)
        {
            ServiceResponse<Transaction> serviceResponse = new ServiceResponse<Transaction>();
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == addOrUpdateTransaction.Id);
            try
            {
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
                    serviceResponse.Data = newTransaction;
                    serviceResponse.Message = "You're transactions has been added";
                }
                else
                {
                    transaction.Status = addOrUpdateTransaction.Status;
                    transaction.Type = addOrUpdateTransaction.Type;
                    transaction.ClientName = addOrUpdateTransaction.ClientName;
                    transaction.Amount = addOrUpdateTransaction.Amount;
                    _context.Transactions.Update(transaction);
                    await _context.SaveChangesAsync();
                    serviceResponse.Data = transaction;
                    serviceResponse.Message = "You're transaction has been updated";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
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
        public async Task<ServiceResponse<Transaction>> UpdateStatusOfTransaction(UpdateStatusOfTransactionVm updateTransactionVm)
        {
            ServiceResponse<Transaction> serviceResponse = new ServiceResponse<Transaction>();
            try
            {
                var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == updateTransactionVm.Id);
                transaction.Status = updateTransactionVm.Status;
                serviceResponse.Data = transaction;
                serviceResponse.Message = "Status of you're transaction has been updated";
                _context.Transactions.Update(transaction);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        // Delete transaction
        public async Task<ServiceResponse<Transaction>> DeleteTransaction(int id)
        {
            ServiceResponse<Transaction> serviceResponse = new ServiceResponse<Transaction>();
            try
            {
                var transaction = await _context.Transactions.FindAsync(id);
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
                serviceResponse.Data = transaction;
                serviceResponse.Message = "You're transaction has been deleted";
                
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
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
