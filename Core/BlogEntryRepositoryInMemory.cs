using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class BlogEntryRepositoryInMemory : IBlogEntryRepository
    {
        private static Dictionary<string, BlogEntry> _blogEntries = new Dictionary<string, BlogEntry>();
        private readonly ILogger<BlogEntryRepositoryInMemory> _logger;

        public BlogEntryRepositoryInMemory(ILogger<BlogEntryRepositoryInMemory> logger)
        {
            _logger = logger;
        }

        public Task AddEntry(BlogEntry toAdd)
        {
            _logger.LogInformation("AddEntry {@Entry}", toAdd);
            _blogEntries.Add(toAdd.Id, toAdd);
            return Task.CompletedTask;
        }

        public Task DeleteEntry(string id)
        {
            _logger.LogInformation("DeleteEntry {ID}", id);
            _blogEntries.Remove(id);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<BlogEntry>> GetAllEntries()
        {
            _logger.LogInformation("GetAllEntries");
            IEnumerable<BlogEntry> entries = _blogEntries.Values;
            return Task.FromResult(entries);
        }

        public Task<BlogEntry> GetEntry(string id)
        {
            _logger.LogInformation("GetEntry {ID}", id);
            return Task.FromResult(_blogEntries[id]);
        }

        public Task UpdateEntry(BlogEntry entry)
        {
            _logger.LogInformation("UpdateEntry {@Entry}", entry);
            _blogEntries[entry.Id] = entry;
            return Task.CompletedTask;
        }
    }
}
