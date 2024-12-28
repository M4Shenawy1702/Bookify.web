using AutoMapper;
using Bookify.web.Core.Models;
using Bookify.web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookify.web.Controllers
{
    public class PublishersController : Controller
    {
        private readonly ApplicationDbContext _context; 
        private readonly IMapper _mapper;

        public PublishersController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var Publishers = _context.Publishers.AsNoTracking().ToList();

            var viewModel = _mapper.Map<IEnumerable<PublisherViewModel>>(Publishers);

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
        public IActionResult Create(PublisherFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var Publisher = _mapper.Map<Publisher>(model);

            _context.Publishers.Add(Publisher);
            _context.SaveChanges();

            var ViewModel = _mapper.Map<PublisherViewModel>(Publisher);

            return PartialView("_PublisherRow", ViewModel);
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Edit(int Id)
        {
            var Publisher = _context.Publishers.Find(Id);
            if (Publisher == null) return NotFound();

            var ViewModel = _mapper.Map<PublisherFormViewModel>(Publisher);

            return PartialView("_Form", ViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PublisherFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var publisher = _context.Publishers.Find(model.Id);
            if (publisher == null) return NotFound();

            publisher = _mapper.Map(model, publisher);
            publisher.LastUpdatedOn = DateTime.Now;

            _context.SaveChanges();

            var ViewModel = _mapper.Map<PublisherViewModel>(publisher);

            return PartialView("_PublisherRow", ViewModel);
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
        public IActionResult AllowItem(PublisherFormViewModel Model)
        {
            var publisher = _context.Publishers.SingleOrDefault(c => c.Name == Model.Name);
            var IsAllowed = publisher is null || publisher.Id.Equals(Model.Id);
            return Json(IsAllowed);
        }
    }
}
