using Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Features.BlogEntry
{
    public class Details
    {
        public class Query : IRequest<Model>
        {
            public string Id { get; set; }
        }

        public class Model
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Query, Model>
        {
            private readonly IBlogEntryRepository _blogRepository;

            public Handler(IBlogEntryRepository blogRepo)
            {
                _blogRepository = blogRepo;
            }

            public async Task<Model> Handle(Query message)
            {
                var entry = await _blogRepository.GetEntry(message.Id);

                return new Model
                {
                    Id = entry.Id,
                    Title = entry.Title,
                    Content = entry.Content
                };
            }
        }
    }
}
