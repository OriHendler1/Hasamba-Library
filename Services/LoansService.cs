using Hasamba_Library.Data;
using Hasamba_Library.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Hasamba_Library.Services
{
    public class LoansService
    {
        private readonly LibraryDBContext dbContext;
        public LoansService(LibraryDBContext i_dbContext)
        {
            dbContext = i_dbContext;
        }
        public List<Loan> GetAllLoans()
        {
            return dbContext.Loans.ToList();

        }
        public ActionResult<List<Loan>> GetLoansByStatus(Loan.LoanStatusState i_status)
        {
        if (!Enum.IsDefined(typeof(Loan.LoanStatusState), i_status))
        {
            return new BadRequestObjectResult("Invalid loan status");
        }

        var checkLoans = dbContext.Loans.Where(l => l.LoanStatus == i_status).ToList();

        if (checkLoans.Count == 0)
        {
            return new NotFoundObjectResult("No loans found with this status");
        }
            return new OkObjectResult(checkLoans); 
        }
        public ActionResult<List<Loan>> GetAllLoansByReader(int i_readerId)
        { 
            if (i_readerId < 0)
            {
                return new BadRequestObjectResult("Invalid reader ID");
            }

             var readerExists = dbContext.Readers.Any(r => r.readerId == i_readerId);
             if (!readerExists)
             {
                 return new NotFoundObjectResult("Reader not found");
             }

             var loans = dbContext.Loans.Where(l => l.ReaderId == i_readerId).ToList();

             if (loans.Count <= 0)
             {
                return new NotFoundObjectResult("This reader has no loans");
             }

             return new OkObjectResult(loans);
        }
        public ActionResult GetBorrowByName(string i_name)
        {
            var reader = dbContext.Readers.FirstOrDefault(r => r.name == i_name);
            if (reader == null)
            {
                return new NotFoundObjectResult("Reader not found");
            }
            var readerLoan = dbContext.Loans.Where(l => l.ReaderId == reader.readerId).ToList();
            if (readerLoan.Count == 0)
            {
                return new NotFoundObjectResult("No loans found for this reader");
            }
            var readerLoanBorrow = dbContext.Loans.Where(l => l.ReaderId == reader.readerId && l.LoanStatus == Loan.LoanStatusState.Borrowed).Count();
            var readerLoanReturned = dbContext.Loans.Where(l => l.ReaderId == reader.readerId && l.LoanStatus == Loan.LoanStatusState.Returned).Count();
            string res = string.Format("{0} borrowd {1} and return {2} books", i_name, readerLoanBorrow, readerLoanReturned);

            return new OkObjectResult(res);
        }
        public ActionResult BorrowBook(int i_readerId, int i_bookId) 
        {
            if (i_readerId < 0 || i_bookId < 0)
            {
                return new BadRequestObjectResult("Invalid reader or book id");
            }

            var reader = dbContext.Readers.FirstOrDefault(r => r.readerId == i_readerId);
            if (reader == null)
            { 
                return new NotFoundObjectResult("Reader not found");
            }
            if (reader.NumberOfAvailableLoans <= 0) 
            {
                return new BadRequestObjectResult("Reader has no available loans left");
            }

            var book = dbContext.Books.FirstOrDefault(r => r.BookID == i_bookId);
            if (book == null)
            {
                return new NotFoundObjectResult("Book not found");
            }
            if (book.NumberOfAvailableCopies <= 0)
            {
                return new BadRequestObjectResult("No available copies for this book");
            }

            var loan = new Loan
            {
                ReaderId = i_readerId,
                BookId = i_bookId,
                LoanStatus = Loan.LoanStatusState.Borrowed
            };

            book.NumberOfAvailableCopies--;
            reader.NumberOfAvailableLoans--;

            dbContext.Loans.Add(loan);
            dbContext.SaveChanges();

            return new OkObjectResult("Book borrowed successfully");
        }
        public ActionResult ReturnBook(int i_loanId)
        {
            if (i_loanId < 0)
            {
                return new BadRequestObjectResult("Invalid loan id");
            }

            var loan = dbContext.Loans.FirstOrDefault(l => l.LoanId == i_loanId);
            if (loan == null) 
            {
                    return new NotFoundObjectResult("Loan not found");
            }
            if (loan.LoanStatus == Loan.LoanStatusState.Returned) 
                {
                    return new BadRequestObjectResult("Book already returned");
                }

            var reader = dbContext.Readers.FirstOrDefault(r => r.readerId == loan.ReaderId);
            if (reader == null)
            {
                return new NotFoundObjectResult("Reader not found");
            }

            var book = dbContext.Books.FirstOrDefault(r => r.BookID == loan.BookId);
            if (book == null)
            {
                return new NotFoundObjectResult("Book not found");
            }

            loan.LoanStatus = Loan.LoanStatusState.Returned;

            book.NumberOfAvailableCopies++;
            reader.NumberOfAvailableLoans++;

            dbContext.SaveChanges();

            return new OkObjectResult("Book returned successfully");
            
        }
    }
}
