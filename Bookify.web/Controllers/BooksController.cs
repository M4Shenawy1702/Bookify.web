using AutoMapper;
using Bookify.web.Core.Consts;
using Bookify.web.Core.Models;
using Bookify.Web.Settings;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Bookify.web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;

        private List<string> _allowedExtensions = new() { ".jpg", ".jpeg", ".png" };
        private int _maxAllowedSize = 2097152;

        public BooksController(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment
            ,IOptions<CloudinarySettings> cloudinary)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            var account = new Account
            {
                Cloud = cloudinary.Value.Cloud,
                ApiKey = cloudinary.Value.ApiKey,
                ApiSecret = cloudinary.Value.ApiSecret,
            };
            _cloudinary = new Cloudinary(account);
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = PopulateViewModel();

            return View("Form",viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(BookFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", PopulateViewModel(model)); 
            
            var book = _mapper.Map<Book>(model);

            if (model.Image is not null)
            {
                var extension = Path.GetExtension(model.Image.FileName);
                if (!_allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image),Errors.NotAllowedExtension);
                    return View("Form", PopulateViewModel(model));
                } 
                if (model.Image.Length > _maxAllowedSize )
                {
                    ModelState.AddModelError(nameof(model.Image),Errors.MaxSize);
                    return View("Form", PopulateViewModel(model));
                }

                var imageName = $"{Guid.NewGuid()}{extension}";

                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/Books", imageName);

                using var stream = System.IO.File.Create(path);

                await model.Image.CopyToAsync(stream);

                book.ImageUrl = imageName;
            }

            foreach (var category in model.SelectedCategories)
            {
                book.Categories.Add(new BookCategory { CategoryId = category });
            }

            _context.Books.Add(book);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var book = _context.Books.Include(b => b.Categories).SingleOrDefault(b => b.Id == id);

            if (book is null)
                return NotFound();

            var model = _mapper.Map<BookFormViewModel>(book);

            var viewModel = PopulateViewModel(model);

            viewModel.SelectedCategories = book.Categories.Select(c => c.CategoryId).ToList();

            return View("Form", viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(BookFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", PopulateViewModel(model));

            var book = _context.Books.Include(b => b.Categories).SingleOrDefault(b => b.Id == model.Id);

            if (book is null) return NotFound();

            //var book = _mapper.Map<Book>(model);

            if (model.Image is not null)
            {
                if(!string.IsNullOrEmpty(book.ImageUrl))
                {
                    var oldPath = Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/Books", book.ImageUrl);
                    if (System.IO.File.Exists(oldPath)) 
                        System.IO.File.Delete(oldPath); 
                }
                var extension = Path.GetExtension(model.Image.FileName);
                if (!_allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.NotAllowedExtension);
                    return View("Form", PopulateViewModel(model));
                }
                if (model.Image.Length > _maxAllowedSize)
                {
                    ModelState.AddModelError(nameof(model.Image), Errors.MaxSize);
                    return View("Form", PopulateViewModel(model));
                }

                var imageName = $"{Guid.NewGuid()}{extension}";

                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/Books", imageName);

                using var stream = System.IO.File.Create(path);

                await model.Image.CopyToAsync(stream);
                
                model.ImageUrl = imageName;
            }
            else if (model.Image is null && !string.IsNullOrEmpty(book.ImageUrl))
                model.ImageUrl = book.ImageUrl;
            book = _mapper.Map(model, book);
            book.LastUpdatedOn = DateTime.Now;

            foreach (var category in model.SelectedCategories)
                book.Categories.Add(new BookCategory { CategoryId = category });


            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult AllowItem(BookFormViewModel Model)
        {
            var book = _context.Books.SingleOrDefault(c => c.Title == Model.Title&&c.AuthorId==Model.AuthorId);
            var IsAllowed = book is null || book.Id.Equals(Model.Id);
            return Json(IsAllowed);
        }
        private BookFormViewModel PopulateViewModel(BookFormViewModel? model = null)
        {
            var viewModel = model is null ? new BookFormViewModel() : model ;

            var authors = _context.Authors.Where(a => !a.IsDeleted).OrderBy(a => a.Name).ToList();
            var pulishers = _context.Publishers.Where(a => !a.IsDeleted).OrderBy(a => a.Name).ToList();
            var categories = _context.Categories.Where(a => !a.IsDeleted).OrderBy(a => a.Name).ToList();

            viewModel.Authors = _mapper.Map<IEnumerable<SelectListItem>>(authors);
            viewModel.Categories = _mapper.Map<IEnumerable<SelectListItem>>(categories);
            viewModel.Publishers = _mapper.Map<IEnumerable<SelectListItem>>(pulishers);

            return viewModel;
        }

    }
}
