using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core;

namespace Blog.Features.Home
{
    public class HomeController : Controller
    {
        private IBlogEntryRepository _blogRepository;

        public HomeController(IBlogEntryRepository blogRepo)
        {
            _blogRepository = blogRepo;
        }

        public async Task<IActionResult> Index()
        {
            var blogEntries = await _blogRepository.GetAllEntries();

            var viewModel = new IndexModel();
            viewModel.Entries = blogEntries.Select(e => new EntryItem { Id = e.Id, Title = e.Title });

            return View(viewModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
