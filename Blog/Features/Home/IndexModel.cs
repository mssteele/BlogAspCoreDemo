using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Features.Home
{
    public class IndexModel
    {
        public IEnumerable<EntryItem> Entries { get; set; }
    }

    public class EntryItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
    }
}
