using Core;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Features.BlogEntry
{
    public class Create
    {
        public class CreateEditCommandBase
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
        }

        public class Command : CreateEditCommandBase, IRequest
        {
            public Command()
            {
                Id = Guid.NewGuid().ToString();
            }
        }

        public class CreateEditBaseValidator<T> 
            : AbstractValidator<T> where T : CreateEditCommandBase
        {
            public CreateEditBaseValidator()
            {
                RuleFor(x => x.Title)
                    .NotNull()
                    .NotEmpty()
                    .MinimumLength(3)
                    .MaximumLength(150);
                //.Must(x => x.Contains("Mick")).WithMessage("Must contain 'Mick'");

                RuleFor(x => x.Content)
                    .NotNull();
            }
        }

        public class Validator : CreateEditBaseValidator<Command>
        {
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
                Core.BlogEntry entry = new Core.BlogEntry
                {
                    Id = message.Id,
                    Title = message.Title,
                    Content = message.Content
                };
                await _blogRepository.AddEntry(entry);
            }
        }
    }
}
