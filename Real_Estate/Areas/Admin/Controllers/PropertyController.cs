using Microsoft.AspNetCore.Mvc;
using Real_Estate.Repository;


namespace Real_Estate.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PropertyController : Controller
    {
        private readonly IRepository<Models.Property> _propertyRepository;
        private readonly IRepository<Models.Location> _locationRepository;

        public PropertyController(IRepository<Models.Property> propertyRepository, IRepository<Models.Location> locationRepository)
        {
            _propertyRepository = propertyRepository;
            _locationRepository = locationRepository;
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
            return View("manage");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var property = await _propertyRepository.GetOneAsync(p => p.Id == id);
            if (property == null)
                return NotFound();
            return View(property);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var locations = await _locationRepository.GetAllAsync();

            ViewBag.Locations = locations
                .GroupBy(l => l.City)
                .Select(g => g.First())
                .ToList();

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Models.Property property, IFormFile Img, IFormFileCollection SubImgFiles, CancellationToken cancellationToken)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.Locations = await _locationRepository.GetAllAsync();
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var property = await _propertyRepository.GetOneAsync(e => e.Id == id);
            var locations = await _locationRepository.GetAllAsync();

            ViewBag.Locations = locations
                .GroupBy(l => l.City)
                .Select(g => g.First())
                .ToList();
            if (property == null)
                return NotFound();

            ViewBag.Locations = await _locationRepository.GetAllAsync();
            return View(property);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
    int id,
    Models.Property property,
    IFormFile Img,
    IFormFileCollection SubImgFiles,
    CancellationToken cancellationToken)
        {
            if (id != property.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Locations = await _locationRepository.GetAllAsync();
                return View(property);
            }

            var oldProperty = await _propertyRepository.GetOneAsync(e => e.Id == id);
            if (oldProperty == null)
                return NotFound();

            // تحديث البيانات العادية
            oldProperty.Title = property.Title;
            oldProperty.Price = property.Price;
            oldProperty.AreaSize = property.AreaSize;
            oldProperty.Type = property.Type;
            oldProperty.LocationId = property.LocationId;
            oldProperty.Bedrooms = property.Bedrooms;
            oldProperty.Bathrooms = property.Bathrooms;
            oldProperty.Description = property.Description;

            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img");
            Directory.CreateDirectory(folder);

            // تحديث الصورة الرئيسية (لو اتغيرت)
            if (Img != null && Img.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(Img.FileName);
                var filePath = Path.Combine(folder, fileName);
                using var stream = System.IO.File.Create(filePath);
                await Img.CopyToAsync(stream);

                oldProperty.MainImg = fileName;
            }

            // الصور الإضافية (لو اتبعتت)
            if (SubImgFiles != null && SubImgFiles.Count > 0)
            {
                oldProperty.MainImg = "";
                foreach (var file in SubImgFiles)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(folder, fileName);
                    using var stream = System.IO.File.Create(filePath);
                    await file.CopyToAsync(stream);

                    oldProperty.MainImg += fileName + ";";
                }
            }

            _propertyRepository.Update(oldProperty, cancellationToken: cancellationToken);
            await _propertyRepository.CommitChange(cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var property = await _propertyRepository.GetOneAsync(e => e.Id == id);
            if (property == null)
                return NotFound();

            return View(property);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var property = await _propertyRepository.GetOneAsync(e => e.Id == id);
            if (property == null)
                return NotFound();

            _propertyRepository.Delete(property);
            await _propertyRepository.CommitChange();

            return RedirectToAction("board", "Property", new { area = "Admin" });

        }



    }
}
