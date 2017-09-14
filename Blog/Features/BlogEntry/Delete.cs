using Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Features.BlogEntry
{
    public class Delete
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
        }

        public class CommandHandler : IAsyncRequestHandler<Command>
        {
            private readonly IBlogEntryRepository _blogRepository;

            public CommandHandler(IBlogEntryRepository blogRepo)
            {
                _blogRepository = blogRepo;
            }

            public async Task Handle(Command message)
            {
                await _blogRepository.DeleteEntry(message.Id);
            }
        }
    }
}
