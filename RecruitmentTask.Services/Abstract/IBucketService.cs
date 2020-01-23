using System.Threading.Tasks;
using RecruitmentTask.Entities;

namespace RecruitmentTask.Services
{
    public interface IBucketService
    {
        Task AddToBucketAsync(Document document);
        Task DeleteFromBucketAsync(Document document);
    }
}
