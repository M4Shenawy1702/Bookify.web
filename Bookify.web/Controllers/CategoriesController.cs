using AutoMapper;
using Bookify.web.Core.Models;
using Bookify.web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookify.web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var Categories = _context.Categories.AsNoTracking().ToList();

            var viewModel = _mapper.Map<IEnumerable<CategoryViewModel>>(Categories);

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
        public IActionResult Create(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var category = _mapper.Map<Category>(model);

            _context.Categories.Add(category);
            _context.SaveChanges();

            var ViewModel = _mapper.Map<CategoryViewModel>(category);

            return PartialView("_CategoryRow", ViewModel);
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Edit(int Id)
        {
            var category = _context.Categories.Find(Id);
            if (category == null) return NotFound();

            var ViewModel = _mapper.Map<CategoryFormViewModel>(category);

            return PartialView("_Form", ViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var category = _context.Categories.Find(model.Id);
            if (category == null) return NotFound();

            category = _mapper.Map(model, category);
            category.LastUpdatedOn = DateTime.Now;

            _context.SaveChanges();

            var ViewModel = _mapper.Map<CategoryViewModel>(category);

            return PartialView("_CategoryRow", ViewModel);
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
        public IActionResult AllowItem(CategoryFormViewModel Model)
        {
            var category = _context.Categories.SingleOrDefault(c => c.Name == Model.Name);
            var IsAllowed = category is null || category.Id.Equals(Model.Id);
            return Json(IsAllowed);
        }
    }
}
