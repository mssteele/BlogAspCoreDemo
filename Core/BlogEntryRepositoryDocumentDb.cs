using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace Core
{
    public class BlogEntryRepositoryDocumentDb : IBlogEntryRepository
    {
        private readonly IDocumentDBWrapper<BlogEntry> _repository;
        private readonly ILogger<BlogEntryRepositoryDocumentDb> _logger;

        public BlogEntryRepositoryDocumentDb(IDocumentDBWrapper<BlogEntry> repository, 
            ILogger<BlogEntryRepositoryDocumentDb> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task AddEntry(BlogEntry toAdd)
        {
            _logger.LogInformation("AddEntry {@Entry}", toAdd);
            await _repository.CreateDocumentAsync(toAdd);
        }

        public async Task DeleteEntry(string id)
        {
            _logger.LogInformation("DeleteEntry {ID}", id);
            await _repository.DeleteDocumentAsync(id);
        }

        public async Task<IEnumerable<BlogEntry>> GetAllEntries()
        {
            _logger.LogInformation("GetAllEntries");
            return await _repository.GetDocumentsAsync(e => true);
        }

        public async Task<BlogEntry> GetEntry(string id)
        {
            _logger.LogInformation("GetEntry {ID}", id);
            return await _repository.GetDocumentByIdAsync(id);
        }

        public async Task UpdateEntry(BlogEntry entry)
        {
            _logger.LogInformation("UpdateEntry {@Entry}", entry);
            await _repository.UpdateDocumentAsync(entry.Id, entry);
        }
    }
}
