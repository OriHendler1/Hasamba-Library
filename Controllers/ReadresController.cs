using Microsoft.AspNetCore.Mvc;
using Hasamba_Library.Services;
using Hasamba_Library.Model;


namespace Hasamba_Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadresController : ControllerBase
    {
        private readonly ReadersService i_ReadersService;
        public ReadresController(ReadersService ReadersService)
        {
            i_ReadersService = ReadersService;
        }

        [HttpPost]
        public ActionResult Post(string readerName)
        {
            return Ok(i_ReadersService.createNewReaedr(readerName));
        }

        [HttpGet]
        public ActionResult Get() 
        {
            return Ok(i_ReadersService.getAllReaders());
        }
    }

}
