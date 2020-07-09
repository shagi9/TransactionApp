using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TA.Data.Entities;
using TA.Business.Interfaces;
using System.IO;
using TA.Business.Helpers;
using System;
using TA.Business.Models;

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

        /// <summary>
        /// returns all transactions
        /// </summary>
        /// <param name="userParams"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Transaction>> GetAllTransactions([FromQuery]UserParams userParams)
        {
            var transactions = await _service.GetAllTransactions(userParams);
            return Ok(transactions);
        }

        /// <summary>
        /// Add transaction or update existing transaction
        /// </summary>
        /// <param name="addOrUpdateTransaction"></param>
        /// <returns>sadsa</returns>
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateTransaction(AddOrUpdateTransactionVm addOrUpdateTransaction)
        {
            ServiceResponse<Transaction> response = await _service.AddOrUpdateTransaction(addOrUpdateTransaction);
            if (response.Data == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        /// <summary>
        /// update status of transaction
        /// </summary>
        /// <param name="updateTransactionVm"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateStatusOfTransaction(UpdateStatusOfTransactionVm updateTransactionVm)
        {
            ServiceResponse<Transaction> response = await _service.UpdateStatusOfTransaction(updateTransactionVm);
            if (response.Data == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        /// <summary>
        /// delete transaction
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            ServiceResponse<Transaction> response = await _service.DeleteTransaction(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// for now export to excel all datas
        /// </summary>
        /// <returns></returns>
        [HttpPost("export to excel")]
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
