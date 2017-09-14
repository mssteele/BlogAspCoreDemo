using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core
{
    public class DocumentDBWrapper<T> : IDocumentDBWrapper<T> where T : new()
    {
        private readonly string _databaseId;
        private readonly string _collectionId;

        // SDK Usage Tip #1: Use a singleton DocumentDB client for the lifetime of your application
        // https://azure.microsoft.com/en-gb/blog/performance-tips-for-azure-documentdb-part-1-2/
        private static DocumentClient client;

        public DocumentDBWrapper(IOptions<DocumentDBWrapperConfig> config)
        {
            _databaseId = config.Value.DatabaseId;
            _collectionId = config.Value.CollectionId;

            if (client == null)
            {
                client = new DocumentClient(new Uri(config.Value.Endpoint), config.Value.AuthKey);
            }

            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(_databaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = _databaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(_databaseId),
                        new DocumentCollection { Id = _collectionId },
                        new RequestOptions { OfferThroughput = 400 });
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<Document> CreateDocumentAsync(T item)
        {
            return await client.CreateDocumentAsync(GetDocumentCollectionUri(), item);
        }

        private Uri GetDocumentCollectionUri()
        {
            return UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId);
        }

        private Uri GetDocumentUri(string documentId)
        {
            return UriFactory.CreateDocumentUri(_databaseId, _collectionId, documentId);
        }

        public async Task<Document> UpdateDocumentAsync(string id, T itemToUpdate)
        {
            // 'upsert' will update existing or create new
            //return await client.UpsertDocumentAsync(GetDocumentCollectionUri(), itemToUpdate);

            return await client.ReplaceDocumentAsync(GetDocumentUri(id), itemToUpdate);
        }

        public async Task<T> GetDocumentByIdAsync(string documentId)
        {
            var response = await client.ReadDocumentAsync<T>(
                    UriFactory.CreateDocumentUri(_databaseId, _collectionId, documentId));
            return response.Document;
        }

        public async Task<IEnumerable<T>> GetDocumentsAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId))
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task DeleteDocumentAsync(string id)
        {
            await client.DeleteDocumentAsync(GetDocumentUri(id));
        }
    }
}
