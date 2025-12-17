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
        public static List<Loan> GetAllLoans()
        {
            using (var context = new LibraryDBContext())
            {
                return context.Loans.ToList();
            }
        }
        public static ActionResult<List<Loan>> GetLoansByStatus(Loan.LoanStatusState i_status)
        {
            if (!Enum.IsDefined(typeof(Loan.LoanStatusState), i_status))
            {
                return new BadRequestObjectResult("Invalid loan status");
            }

            using (var context = new LibraryDBContext())
            {
                var checkLoans = context.Loans.Where(l => l.LoanStatus == i_status).ToList();

                if (checkLoans.Count == 0)
                {
                    return new NotFoundObjectResult("No loans found with this status");
                }

                return new OkObjectResult(checkLoans);
            }
        }
        public static ActionResult<List<Loan>> GetAllLoansByReader(int i_readerId)
        {
            //check if id is valid
            if (i_readerId < 0)
            {
                return new BadRequestObjectResult("Invalid reader ID");
            }

            using (var context = new LibraryDBContext())
            {
                //check if reader exsist
                var readerExists = context.Readers.Any(r => r.readerId == i_readerId);
                if (!readerExists)
                {
                    return new NotFoundObjectResult("Reader not found");
                }

                var loans = context.Loans.Where(l => l.ReaderId == i_readerId).ToList();

                //check if reader have loans
                if (loans.Count <= 0)
                {
                    return new NotFoundObjectResult("This reader has no loans");
                }

                return new OkObjectResult(loans);
            }
        }
        public static ActionResult BorrowBook(int i_readerId, int i_bookId) 
        {
            if (i_readerId < 0 || i_bookId < 0)
            {
                return new BadRequestObjectResult("Invalid reader or book id");
            }

            using (var context = new LibraryDBContext()) 
            {
                //check reader
                var reader = context.Readers.FirstOrDefault(r => r.readerId == i_readerId);
                if (reader == null)
                { 
                    return new NotFoundObjectResult("Reader not found");
                }
                if (reader.NumberOfAvailableLoans <= 0) 
                {
                    return new BadRequestObjectResult("Reader has no available loans left");
                }

                //check book
                var book = context.Books.FirstOrDefault(r => r.BookID == i_bookId);
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
                //update reader loans and book copies
                book.NumberOfAvailableCopies--;
                reader.NumberOfAvailableLoans--;

                //add loan to DB
                context.Loans.Add(loan);
                context.SaveChanges();

                return new OkObjectResult("Book borrowed successfully");
            }
        }
        public static ActionResult ReturnBook(int i_loanId)
        {
            if (i_loanId < 0)
            {
                return new BadRequestObjectResult("Invalid loan id");
            }

            var context = new LibraryDBContext();
            { 
                //check loan
                var loan = context.Loans.FirstOrDefault(l => l.LoanId == i_loanId);
                if (loan == null) 
                {
                    return new NotFoundObjectResult("Loan not found");
                }
                if (loan.LoanStatus == Loan.LoanStatusState.Returned) 
                {
                    return new BadRequestObjectResult("Book already returned");
                }

                //check reader
                var reader = context.Readers.FirstOrDefault(r => r.readerId == loan.ReaderId);
                if (reader == null)
                {
                    return new NotFoundObjectResult("Reader not found");
                }

                //check book
                var book = context.Books.FirstOrDefault(r => r.BookID == loan.BookId);
                if (book == null)
                {
                    return new NotFoundObjectResult("Book not found");
                }

                //update loan status
                loan.LoanStatus = Loan.LoanStatusState.Returned;

                //update reader loans and book copies
                book.NumberOfAvailableCopies++;
                reader.NumberOfAvailableLoans++;

                //DB update
                context.SaveChanges();

                return new OkObjectResult("Book returned successfully");
            }
        }
    }
}
