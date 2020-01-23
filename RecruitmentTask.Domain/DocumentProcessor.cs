using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecruitmentTask.Entities;
using RecruitmentTask.Services;

namespace RecruitmentTask.Domain
{
    public class DocumentProcessor : IDocumentProcessor
    {
        private IBucketService bucketService;
        private IRepository repository;

        public DocumentProcessor()
        {
            this.bucketService = new BucketService();
            this.repository = new DocumentRepository();
        }

        public async Task<IEnumerable<string>> GetDocumentsAsync(string author, DateTime from, DateTime to)
        {
            try
            {
                return await repository.GetItemsAsync(author, from, to);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured while retrieving documents.", ex);
            }
        }

        public async Task AddDocumentAsync(Document document)
        {
            await AddToS3BucketAsync(document);

            try
            {   
                await AddToDynamoDbAsync(document);
            }
            catch(Exception ex)
            {
                await DeleteFromS3BucketAsync(document);
                throw;
            }
        }

        private async Task AddToS3BucketAsync(Document document)
        {
            try
            {
                await bucketService.AddToBucketAsync(document);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured while adding file to the bucket.", ex);
            }
        }

        private async Task DeleteFromS3BucketAsync(Document document)
        {
            try
            {
                await bucketService.DeleteFromBucketAsync(document);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured while adding file to the bucket.", ex);
            }
        }

        private async Task AddToDynamoDbAsync(Document document)
        {
            try
            {
                await repository.AddAsync(document);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured while adding file to database.", ex);
            }
        }
    }
}
