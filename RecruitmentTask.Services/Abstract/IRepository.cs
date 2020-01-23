using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecruitmentTask.Entities;

namespace RecruitmentTask.Domain
{
    public interface IRepository
    {
        Task AddAsync(Document item);
        Task<IEnumerable<string>> GetItemsAsync(string Author, DateTime from, DateTime to);
    }
}
