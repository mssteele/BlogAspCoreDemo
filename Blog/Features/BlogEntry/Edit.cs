using Core;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Features.BlogEntry
{
    public class Edit
    {
        public class Query : IRequest<Command>
        {
            public string Id { get; set; }
        }

        public class Command : Create.CreateEditCommandBase, IRequest
        {
        }

        public class Validator : Create.CreateEditBaseValidator<Command>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, Command>
        {
            private readonly IBlogEntryRepository _blogRepository;

            public QueryHandler(IBlogEntryRepository blogRepo)
            {
                _blogRepository = blogRepo;
            }

            public async Task<Command> Handle(Query message)
            {
                var entry = await _blogRepository.GetEntry(message.Id);

                return new Command
                {
                    Id = entry.Id,
                    Title = entry.Title,
                    Content = entry.Content
                };
            }
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
                var entryToUpdate = new Core.BlogEntry
                {
                    Id = message.Id,
                    Title = message.Title,
                    Content = message.Content
                };

                await _blogRepository.UpdateEntry(entryToUpdate);
            }
        }
    }
}
