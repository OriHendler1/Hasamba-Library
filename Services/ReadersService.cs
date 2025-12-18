using Hasamba_Library.Data;
using Hasamba_Library.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Hasamba_Library.Services
{
    public class ReadersService
    {
        private readonly LibraryDBContext dbContext;
        public ReadersService(LibraryDBContext i_dbContext)
        {
            dbContext = i_dbContext;
        }
        public ActionResult createNewReaedr(string i_fullName)
        {
            if (string.IsNullOrWhiteSpace(i_fullName))
            {
                return new BadRequestObjectResult("Reader name is required");
            }
            var reader = new Reader
            {
                name = i_fullName,
            };

            dbContext.Readers.Add(reader);
            dbContext.SaveChanges();
            string displayId = $"R{reader.readerId:D4}";
            return new CreatedResult($"/readers/{reader.readerId}", new { reader = displayId });
        }
        public List<Reader> getAllReaders()
        {
            return dbContext.Readers.ToList();
        }

    }
}
