using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TA.Data.Entities;
using TA.Data.DataContext;
using TA.Business.Interfaces;
using System.IO;
using TA.Business.Helpers;
using System.Collections.Generic;
using EFCore.BulkExtensions;
using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using ClosedXML.Excel;
using System.Net.Http;

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

        public TransactionsController(ITransactionService service, 
            TransactionDbContext context)
        {
            _service = service;
            _context = context;
        }

        /// <summary>
        /// returns something
        /// </summary>
        /// <param name="userParams"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Transaction>> GetAllTransactions([FromQuery]UserParams userParams)
        {
            var transactions = await _service.GetAllTransactions(userParams);
            return Ok(transactions);

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

        [HttpPost("export in excel")]
        public IActionResult DownloadExcelDocument()
        {
            var data = _service.getData();
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "authors.xlsx";
            try
            {
                using (var stream = new MemoryStream())
                {
                    data.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
    }
}
