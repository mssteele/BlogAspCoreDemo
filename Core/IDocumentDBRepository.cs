using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace Core
{
    public interface IDocumentDBWrapper<T> where T : new()
    {
        Task<Document> CreateDocumentAsync(T item);
        Task<Document> UpdateDocumentAsync(string id, T itemToUpdate);
        Task<T> GetDocumentByIdAsync(string documentId);
        Task<IEnumerable<T>> GetDocumentsAsync(Expression<Func<T, bool>> predicate);
        Task DeleteDocumentAsync(string id);
    }
}