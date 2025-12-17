using Hasamba_Library.Model;
using Hasamba_Library.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hasamba_Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        //get all loans
        [HttpGet]
        public ActionResult GetAllLoansList()
        {
            return Ok(LoansService.GetAllLoans());
        }

        //gel loans by status
        [HttpGet("Status/{status}")]
        public static ActionResult<List<Loan>> GetLoansByStatusList(Loan.LoanStatusState status)
        {
            return LoansService.GetLoansByStatus(status);
        }

        //get loans by reader
        [HttpGet("ReaderID/{readerId}")]
        public ActionResult<List<Loan>> GetAllLoansByReaderList(int readerId)
        {
            return LoansService.GetAllLoansByReader(readerId);
        }

        //borrow book
        [HttpGet("Borrow/")]
        public ActionResult BorrowBookUpdate(int readerId, int bookId)
        {
            return LoansService.BorrowBook(readerId, bookId);
        }

        //return book
        [HttpGet("Return/")]
        public ActionResult ReturnBookUpdate(int loanId)
        {
            return LoansService.ReturnBook(loanId);
        }
    }
}
