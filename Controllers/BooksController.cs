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
        private readonly BooksService i_booksService;
        public BooksController(BooksService booksService)
        {
            i_booksService = booksService;
        }

        [HttpPost]
        public ActionResult PostNewBook(string bookName, string Description, string auther,int numOfCopies)
        {
            return Ok(i_booksService.createNewBook(bookName, Description, auther, numOfCopies));
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok(i_booksService.getAllBooks());
        }

        [HttpGet("Name/{bookName}")]
        public ActionResult<List<Book>> GetBooksByNameList(string bookName)
        {
            return i_booksService.getBookByName(bookName);
        }

        [HttpGet("Auther/{bookAuther}")]
        public ActionResult<List<Book>> GetBooksByAutherList(string bookAuther)
        {
            return i_booksService.getBookByAuther(bookAuther);
        }

    }
}
