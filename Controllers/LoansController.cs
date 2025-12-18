using Hasamba_Library.Model;
using Hasamba_Library.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hasamba_Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly LoansService i_LoansService;
        public LoansController(LoansService loansService)
        {
            i_LoansService = loansService;
        }

        [HttpGet]
        public ActionResult GetAllLoansList()
        {
            return Ok(i_LoansService.GetAllLoans());
        }

        [HttpGet("Status/{status}")]
        public ActionResult<List<Loan>> GetLoansByStatusList(Loan.LoanStatusState status)
        {
            return i_LoansService.GetLoansByStatus(status);
        }

        [HttpGet("ReaderID/{readerId}")]
        public ActionResult<List<Loan>> GetAllLoansByReaderList(int readerId)
        {
            return i_LoansService.GetAllLoansByReader(readerId);
        }

        [HttpGet("Borrow/")]
        public ActionResult BorrowBookUpdate(int readerId, int bookId)
        {
            return i_LoansService.BorrowBook(readerId, bookId);
        }

        [HttpGet("Return/")]
        public ActionResult ReturnBookUpdate(int loanId)
        {
            return i_LoansService.ReturnBook(loanId);
        }
    }
}
