using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;

namespace RecruitmentTask.Core
{
    public class AWS
    {
        private static readonly AWS instance = new AWS();

        private AWS() { }

        public static AWS Instance => instance;

        public AWSOptions AWSOptions { get; set; }
        
        public IAmazonS3 AmazonS3Client 
        {
            get { return AWSOptions.CreateServiceClient<IAmazonS3>(); }
        }

        public IAmazonDynamoDB DynamoDbClient 
        {
            get { return AWSOptions.CreateServiceClient<IAmazonDynamoDB>(); }
        }

        public string S3BucketName { get; set; }
        public string DynamoDbTableName { get; set; }
    }
}
