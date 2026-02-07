using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Real_Estate.Repository;
using System.Threading;
using System.Threading.Tasks;


namespace Real_Estate.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PropertyController : Controller
    {
        private readonly IRepository<Models.Property> _propertyRepository;

        public PropertyController(IRepository<Models.Property> propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var properties = await _propertyRepository.GetAllAsync();
            return View(properties.AsEnumerable());
        }
        public async Task<IActionResult> board()
        {
            var properties = await _propertyRepository.GetAllAsync();
            return View(properties.AsEnumerable());
        }
        [HttpGet]
        public async Task<IActionResult> manage()
        {
            var properties = await _propertyRepository.GetAllAsync();
            return View(properties.AsEnumerable());
        }

        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var properties = await _propertyRepository.GetOneAsync();
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var properties =  await _propertyRepository.GetAllAsync(cancellationToken : cancellationToken);
            return View(new Models.Property());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Models.Property property, IFormFile Img, IFormFileCollection SubImgFiles, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(property);
            }

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PropertyImages");
            Directory.CreateDirectory(uploadPath);

            if (Img != null && Img.Length > 0)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img");
                Directory.CreateDirectory(folder);
                var fileName = Guid.NewGuid() + Path.GetExtension(Img.FileName);
                var filePath = Path.Combine(folder, fileName);
                using var stream = System.IO.File.Create(filePath);
                await Img.CopyToAsync(stream);
                property.MainImg = fileName;
            }


            if (SubImgFiles != null && SubImgFiles.Count > 0)
            {
                property.MainImg = "";
                foreach (var file in SubImgFiles)
                {
                    var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img");
                    Directory.CreateDirectory(folder);
                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(folder, fileName);
                    using var stream = System.IO.File.Create(filePath);
                    await file.CopyToAsync(stream);
                    property.MainImg += fileName + ";";
                }
            }

            await _propertyRepository.AddAsync(property, cancellationToken);
            await _propertyRepository.CommitChange(cancellationToken);

            return RedirectToAction("Index");
        }
    }
}
