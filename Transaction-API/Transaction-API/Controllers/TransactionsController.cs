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

        public TransactionsController(ITransactionService service)
        {
            _service = service;
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

        [HttpGet("filtering")]
        public async Task<ActionResult<List<Transaction>>> Filering(string sortOrder)
        {
            var sort = await _service.Filtering(sortOrder);
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
    }
}
