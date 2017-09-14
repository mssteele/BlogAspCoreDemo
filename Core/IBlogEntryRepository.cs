using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core
{
    public interface IBlogEntryRepository
    {
        Task AddEntry(BlogEntry toAdd);

        Task<BlogEntry> GetEntry(string id);

        Task<IEnumerable<BlogEntry>> GetAllEntries();

        Task UpdateEntry(BlogEntry entry);

        Task DeleteEntry(string id);
    }
}
