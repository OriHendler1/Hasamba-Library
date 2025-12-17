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
        public static ActionResult createNewReaedr(string i_fullName)
        {
            if (string.IsNullOrWhiteSpace(i_fullName))
            {
                return new BadRequestObjectResult("Reader name is required");
            }
            using (var context = new LibraryDBContext())
            {
                var reader = new Reader
                {
                    name = i_fullName,
                };
                
                context.Readers.Add(reader);
                context.SaveChanges();
                string displayId = $"R{reader.readerId:D4}";

                return new CreatedResult($"/readers/{reader.readerId}", new { reader = displayId });

            }
        }
        public static List<Reader> getAllReaders()
        {
            using (var context = new LibraryDBContext()) 
            {
                return context.Readers.ToList();
            }
        }

    }
}
