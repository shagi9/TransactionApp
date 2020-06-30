using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TA.Data.Entities;
using TA.Data.DataContext;
using TA.Business.Interfaces;
using System.IO;
using ClosedXML.Excel;
using TA.Business.Helpers;


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
    }
}
