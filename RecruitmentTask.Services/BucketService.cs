using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using RecruitmentTask.Core;
using RecruitmentTask.Entities;

namespace RecruitmentTask.Services
{
    public class BucketService : IBucketService
    {
        private static string bucketName = AWS.Instance.S3BucketName;
        private static IAmazonS3 client;

        public async Task AddToBucketAsync(Document document)
        {
            using (client = AWS.Instance.AmazonS3Client)
            {
                PutObjectRequest request = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = document.FileName,
                    InputStream = new MemoryStream(document.FileBytes),
                    TagSet = new System.Collections.Generic.List<Tag>
                    {
                        new Tag { Key="Author", Value = document.Author }
                    }
                };

                PutObjectResponse response = await client.PutObjectAsync(request);
            }
        }

        public async Task DeleteFromBucketAsync(Document document)
        {
            using (client = AWS.Instance.AmazonS3Client)
            {
                DeleteObjectRequest request = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = document.FileName,
                };

                DeleteObjectResponse response = await client.DeleteObjectAsync(request);
            }
        }
    }
}
