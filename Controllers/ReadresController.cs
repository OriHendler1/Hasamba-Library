using Microsoft.AspNetCore.Mvc;
using Hasamba_Library.Services;
using Hasamba_Library.Model;


namespace Hasamba_Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadresController : ControllerBase
    {
        //create new reader
        [HttpPost]
        public ActionResult Post(string readerName)
        {
            return Ok(ReadersService.createNewReaedr(readerName));
        }

        //get all reader
        [HttpGet]
        public ActionResult Get() 
        {
            return Ok(ReadersService.getAllReaders());
        }
    }

}
