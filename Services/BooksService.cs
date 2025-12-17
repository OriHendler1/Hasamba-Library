using Hasamba_Library.Model;
using Hasamba_Library.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Hasamba_Library.Services
{
    public class BooksService
    {
        public static ActionResult createNewBook(string i_Name, string i_Description, string i_Auther, int i_NumOfCopies)
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
            using (var context = new LibraryDBContext()) 
            {
                var book = new Book
                {
                    BookName = i_Name,
                    Description = i_Description,
                    Author = i_Auther,
                    NumberOfAvailableCopies = i_NumOfCopies
                };

                context.Books.Add(book);
                context.SaveChanges();

                string displayId = $"B{book.BookID:D4}";

                return new CreatedAtActionResult(nameof(createNewBook), null, new { book = displayId }, book);

            }
            
        }
        public static List<Book> getAllBooks()
        {
            using (var context = new LibraryDBContext())
            {
                return context.Books.ToList();
            }
        }
        public static ActionResult<List<Book>> getBookByName(string i_Name)
        {
            if (string.IsNullOrWhiteSpace(i_Name))
            {
                return new BadRequestObjectResult("Book name is required");
            }

            using (var context = new LibraryDBContext())
            {
                return context.Books.Where(b => b.BookName.ToLower().Contains(i_Name.ToLower())).ToList();
            }
        }
        public static ActionResult<List<Book>> getBookByAuther(string i_Auther)
        {
            if (string.IsNullOrWhiteSpace(i_Auther))
            {
                return new BadRequestObjectResult("Auther name is required");
            }
            using (var context = new LibraryDBContext())
            {
                return context.Books.Where(b => b.Author.ToLower().Contains(i_Auther.ToLower())).ToList();
            }

        }
    }
}
