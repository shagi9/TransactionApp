using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transaction_API.Dto;
using Transaction_API.Models;

namespace Transaction_API.Controllers
{
    /// <summary>
    /// Transactions Controlller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionDbContext _context;

        public TransactionsController(TransactionDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Transactions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return await _context.Transactions.ToListAsync();
        }
        /// <summary>
        /// GET: api/Transaction/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTodoItem(long id)
        {
            var todoItem = await _context.Transactions.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        /// <summary>
        /// POST: api/Transactions
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns>A new transaction</returns>
        /// /// <response code="201">Returns the newly created transaction</response>
        /// <response code="400">If the item is null</response> 
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = transaction.Id.ToString() }, transaction);
        }

        /// <summary>
        /// PUT: api/Transaction/5  
        /// </summary>
        /// <param name="id"></param>
        /// <param name="EditStatusDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(long id, EditStatusDto EditStatusDto)
        {
            if (id != EditStatusDto.Id)
            {
                return BadRequest();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            
            if (transaction == null)
            {
                return NotFound();
            }

            transaction.Status = EditStatusDto.Status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TransactionExists(id))
            {
                return NotFound();
            }   

            return NoContent();
        }

        /// <summary>
        /// DELETE: api/Transactions/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Transaction>> DeleteTransaction(long id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        private bool TransactionExists(long id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
