using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using RecruitmentTask.Core;
using RecruitmentTask.Domain;
using RecruitmentTask.Entities;

namespace RecruitmentTask.Services
{
    public class DocumentRepository : IRepository
    {
        private static IAmazonDynamoDB dbClient;
        private static string tableName = AWS.Instance.DynamoDbTableName;

        public async Task<IEnumerable<string>> GetItemsAsync(string Author, DateTime from, DateTime to)
        {
            QueryRequest query = new QueryRequest
            {
                TableName = tableName,
                KeyConditionExpression = "Author = :a and AdditionDate between :from and :to",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":a", new AttributeValue { S = Author } },
                    { ":from", new AttributeValue { N = from.Ticks.ToString() } },
                    { ":to", new AttributeValue { N = to.Ticks.ToString() } },
                },
                ProjectionExpression = "FileName"
            };

            using (dbClient = AWS.Instance.DynamoDbClient)
            {
                var resp = await dbClient.QueryAsync(query);
                return resp.Items.SelectMany(i => i.Select(kv => kv.Value.S));
            }
        }

        public async Task AddAsync(Document document)
        {
            Dictionary<string, AttributeValue> item = new Dictionary<string, AttributeValue>
            {
                { "Author", new AttributeValue { S = document.Author } },
                { "AdditionDate", new AttributeValue { N = document.AdditionDate.Ticks.ToString() } },
                { "FileName", new AttributeValue { S = document.FileName } }
            };

            using (dbClient = AWS.Instance.DynamoDbClient)
            {
                await dbClient.PutItemAsync(tableName, item);
            }
        }
    }
}
