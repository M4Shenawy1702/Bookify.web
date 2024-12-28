using AutoMapper;
using Bookify.web.Core.Models;
using Bookify.web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookify.web.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AuthorsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var Authors = _context.Authors.AsNoTracking().ToList();

            var viewModel = _mapper.Map<IEnumerable<AuthorViewModel>>(Authors);

            return View(viewModel);
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Create()
        {
            return PartialView("_Form");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AuthorFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var author = _mapper.Map<Author>(model);

            _context.Authors.Add(author);
            _context.SaveChanges();

            var ViewModel = _mapper.Map<AuthorViewModel>(author);

            return PartialView("_AuthorRow", ViewModel);
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Edit(int Id)
        {
            var author = _context.Authors.Find(Id);
            if (author == null) return NotFound();

            var ViewModel = _mapper.Map<AuthorFormViewModel>(author);

            return PartialView("_Form", ViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AuthorFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var author = _context.Authors.Find(model.Id);
            if (author == null) return NotFound();

            author = _mapper.Map(model, author);
            author.LastUpdatedOn = DateTime.Now;

            _context.SaveChanges();

            var ViewModel = _mapper.Map<AuthorViewModel>(author);

            return PartialView("_AuthorRow", ViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null) return NotFound();

            category.IsDeleted = !category.IsDeleted;
            category.LastUpdatedOn = DateTime.Now;

            _context.SaveChanges();

            return Ok(category.LastUpdatedOn.ToString());
        }
        public IActionResult AllowItem(AuthorFormViewModel Model)
        {
            var Author = _context.Authors.SingleOrDefault(c => c.Name == Model.Name);
            var IsAllowed = Author is null || Author.Id.Equals(Model.Id);
            return Json(IsAllowed);
        }
    }
}
