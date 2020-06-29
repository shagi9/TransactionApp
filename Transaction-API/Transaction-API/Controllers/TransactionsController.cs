using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TA.Data.Entities;
using TA.Data.DataContext;
using Microsoft.EntityFrameworkCore.Storage;
using TA.Business.Interfaces;
using TA.Business.Services;
using X.PagedList;
using System.IO;
using ClosedXML.Excel;

namespace Transaction_API.Controllers
{
    /// <summary>
    /// Transactions Controlller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _service;
        private readonly TransactionDbContext _context;
        public TransactionsController(ITransactionService service, TransactionDbContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpGet("get all")]
        public async Task<ActionResult<List<Transaction>>> GetAllTransactions()
        {
            var transactions = await _service.GetAllTransactions();
            return transactions;
        }

        [HttpGet("pagination")]
        public async Task<IPagedList<Transaction>> Pagination(int? page)
        {
            var sort = await _service.Pagination(page);
            return sort;
        }

        [HttpGet("filtering by type")]
        public async Task<List<Transaction>> FilteringByType(string sortByType)
        {
            var sort = await _service.FilteringByType(sortByType);
            return sort;
        }

        [HttpGet("filtering by status")]
        public async Task<List<Transaction>> FilteringByStatus(string sortByStatus)
        {
            var sort = await _service.FilteringByStatus(sortByStatus);
            return sort;
        }
            
        [HttpPost("add")]
       public async Task<ActionResult<Transaction>> AddTransaction(string status, string type, string clientName, decimal amount)
        {
            var newTransaction = await _service.AddTransaction(status, type, clientName, amount);
            return newTransaction;
        }

        [HttpPut("{id}")]
        public async Task<Transaction> PutTransaction(int id, string status)
        {
            var trans = await _service.UpdateTransaction(id, status);
            return trans;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Transaction>> DeleteTransaction(int id)
        {
            var deleteTransaction = await _service.DeleteTransaction(id);
            return deleteTransaction;
        }
         
        [HttpGet("csv")]
        public IActionResult ExportFile()
        {
            var transactions = _context.Transactions.ToList();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("Transactions");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Status";
                worksheet.Cell(currentRow, 3).Value = "Type";
                worksheet.Cell(currentRow, 4).Value = "ClientName";
                worksheet.Cell(currentRow, 5).Value = "Amount";

                foreach (var trans in transactions)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = trans.Id;
                    worksheet.Cell(currentRow, 2).Value = trans.Status;
                    worksheet.Cell(currentRow, 3).Value = trans.Type;
                    worksheet.Cell(currentRow, 4).Value = trans.ClientName;
                    worksheet.Cell(currentRow, 5).Value = trans.Amount;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Transactions.xlsx");
                }
            }
        }
    }
}
