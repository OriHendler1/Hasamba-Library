using Hasamba_Library.Data;
using Hasamba_Library.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Hasamba_Library.Services
{
    public class BooksService
    {
        private readonly LibraryDBContext dbContext;
        public BooksService(LibraryDBContext i_dbContext)
        {
            dbContext = i_dbContext;
        }
        public ActionResult createNewBook(string i_Name, string i_Description, string i_Auther, int i_NumOfCopies)
        {
            if (string.IsNullOrWhiteSpace(i_Name))
            {
                return new BadRequestObjectResult("Book name is required");
            }
            if (string.IsNullOrWhiteSpace(i_Auther))
            {
                return new BadRequestObjectResult("Book name is required");
            }
            if (i_NumOfCopies < 0)
            {
                return new BadRequestObjectResult("Number of copies cannot be negative");
            }
            
            var book = new Book
            {
                BookName = i_Name,
                Description = i_Description,
                Author = i_Auther,
                NumberOfAvailableCopies = i_NumOfCopies
            };

            dbContext.Books.Add(book);
            dbContext.SaveChanges();

            string displayId = $"B{book.BookID:D4}";

             return new CreatedAtActionResult(nameof(createNewBook), null, new { book = displayId }, book);   
            
        }
        public List<Book> getAllBooks()
        {
            return dbContext.Books.ToList();
        }
        public ActionResult<List<Book>> getBookByName(string i_Name)
        {
            if (string.IsNullOrWhiteSpace(i_Name))
            {
                return new BadRequestObjectResult("Book name is required");
            }
            
            return dbContext.Books.Where(b => b.BookName.ToLower().Contains(i_Name.ToLower())).ToList();

        }
        public ActionResult<List<Book>> getBookByAuther(string i_Auther)
        {
            if (string.IsNullOrWhiteSpace(i_Auther))
            {
                return new BadRequestObjectResult("Auther name is required");
            }
            
            return dbContext.Books.Where(b => b.Author.ToLower().Contains(i_Auther.ToLower())).ToList();
        }
    }
}
