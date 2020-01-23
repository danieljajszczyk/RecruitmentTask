using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecruitmentTask.Entities;

namespace RecruitmentTask.Domain
{
    public interface IDocumentProcessor
    {
        Task<IEnumerable<string>> GetDocumentsAsync(string author, DateTime from, DateTime to);
        Task AddDocumentAsync(Document doc);
    }
}
