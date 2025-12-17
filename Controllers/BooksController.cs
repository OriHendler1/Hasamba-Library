using Hasamba_Library.Model;
using Hasamba_Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Hasamba_Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        //create new book
        [HttpPost]
        public ActionResult PostNewBook(string bookName, string Description, string auther,int numOfCopies)
        {
            return Ok(BooksService.createNewBook(bookName, Description, auther, numOfCopies));
        }

        //get all books
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(BooksService.getAllBooks());
        }

        //get book by name
        [HttpGet("Name/{bookName}")]
        public ActionResult<List<Book>> GetBooksByNameList(string bookName)
        {
            return BooksService.getBookByName(bookName);
        }

        //get book by auther
        [HttpGet("Auther/{bookAuther}")]
        public ActionResult<List<Book>> GetBooksByAutherList(string bookAuther)
        {
            return BooksService.getBookByAuther(bookAuther);
        }

    }
}
