using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Blog.Features.BlogEntry
{
    public class BlogEntryController : Controller
    {
        private readonly IMediator _mediator;

        public BlogEntryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Details(Details.Query query)
        {
            var model = await _mediator.Send(query);
            return View(model);
        }

        public IActionResult Create()
        {
            return View(new Create.Command());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Create.Command command)
        {
            if(!ModelState.IsValid)
            {
                return View(command);
            }

            await _mediator.Send(command);

            return RedirectToAction(nameof(Details), new { Id = command.Id });
        }

        public async Task<IActionResult> Edit(string id)
        {
            var command = await _mediator.Send(new Edit.Query { Id = id });
            return View(command);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Edit.Command message)
        {
            if (!ModelState.IsValid)
            {
                return View(message);
            }

            await _mediator.Send(message);

            return RedirectToAction(nameof(Details), new { Id = message.Id });
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new Delete.Command { Id = id });

            return RedirectToAction("Index", "Home");
        }
    }
}
