using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecruitmentTask.Api.Models;
using RecruitmentTask.Domain;
using RecruitmentTask.Entities;

namespace RecruitmentTask.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private IDocumentProcessor documentProcessor;

        public DocumentsController(IDocumentProcessor documentProcessor)
        {
            this.documentProcessor = documentProcessor;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string author, string from, string to)
        {
            if(string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Author cannot be empty");

            if (!DateTime.TryParse(from, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtFrom)
                || !DateTime.TryParse(to, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtTo))
                throw new ArgumentException("Invalid date format. Expected is dd/mm/yyyy hh:mm:ss.");

            if (dtFrom > dtTo)
                throw new ArgumentException("Date \"from\" should be earlier than \"to\" or equal.");

            IEnumerable<string> docs = await documentProcessor.GetDocumentsAsync(author, dtFrom, dtTo);

            if (docs.Count() == 0)
                return NoContent();

            return Ok(docs);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm]string author, IFormFile file)
        {
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Author cannot be empty");

            if (file == null || file.Length == 0)
                throw new ArgumentException("File cannot be empty");

            if(string.IsNullOrWhiteSpace(file.FileName))
                throw new ArgumentException("File name cannot be empty");

            byte[] fileBytes = FileProcessor.GetFileBytes(file);            
            Document doc = new Document(author, file.FileName, fileBytes, DateTime.Now);

            await documentProcessor.AddDocumentAsync(doc);

            return CreatedAtAction(nameof(Get),
                new { author = doc.Author, additionDate = doc.AdditionDate.ToString(CultureInfo.InvariantCulture) });
        }
    }
}