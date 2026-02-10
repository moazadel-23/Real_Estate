using Microsoft.AspNetCore.Mvc;
using Real_Estate.Repository;
using System.Linq.Expressions;


namespace Real_Estate.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PropertyController : Controller
    {
        private readonly IRepository<Models.Property> _propertyRepository;
        private readonly IRepository<Models.Location> _locationRepository;
        private readonly IRepository<Models.PropertySubImage> _propertySubImageRepository;

        public PropertyController(IRepository<Models.Property> propertyRepository, IRepository<Models.Location> locationRepository,  IRepository<PropertySubImage> propertySubImageRepository)
        {
            _propertyRepository = propertyRepository;
            _locationRepository = locationRepository;
            _propertySubImageRepository = propertySubImageRepository;
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
        public async Task<IActionResult> manage(string type)
        {
            var properties = await _propertyRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(type))
            {
                if (Enum.TryParse<PropertyType>(type, out var propertyType))
                {
                    properties = properties.Where(p => p.Type == propertyType).ToList();
                }
            }
            ViewBag.ApartmentCount = properties.Count(e => e.Type == PropertyType.Apartment);
            ViewBag.VillaCount = properties.Count(e => e.Type == PropertyType.Villa);
            ViewBag.OfficeCount = properties.Count(e => e.Type == PropertyType.Office);
            ViewBag.PalaceCount = properties.Count(e => e.Type == PropertyType.Palace);
            ViewBag.ChaletCount = properties.Count(e => e.Type == PropertyType.Chalet);
            //Filter
            int count = properties.Count();
            ViewBag.PropertyCount = count;


            return View("manage", properties);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var property = await _propertyRepository.GetOneAsync(
     p => p.Id == id,
     include: new Expression<Func<Models.Property, object>>[] { p => p.PropertySubImgs }
 );


            if (property == null) return NotFound();

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
                // 1️⃣ هيّئ الـ List لو مش موجودة
                if (property.PropertySubImgs == null)
                    property.PropertySubImgs = new List<PropertySubImage>();

                // 2️⃣ فولدر تخزين الصور
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
                Directory.CreateDirectory(folder);

                // 3️⃣ احفظ كل صورة
                foreach (var file in SubImgFiles)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(folder, fileName);

                    using var stream = System.IO.File.Create(filePath);
                    await file.CopyToAsync(stream);

                    property.PropertySubImgs.Add(new PropertySubImage
                    {
                        PropertyImgs = fileName // أو ImgPath حسب اسم الخاصية عندك
                    });
                }
            }


            await _propertyRepository.AddAsync(property, cancellationToken);
            await _propertyRepository.CommitChange(cancellationToken);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var property = await _propertyRepository.GetOneAsync(
                e => e.Id == id,
                include: new Expression<Func<Models.Property, object>>[] { p => p.PropertySubImgs }
            );

            if (property == null) return NotFound();

            ViewBag.Locations = await _locationRepository.GetAllAsync();
            return View(property);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
      int id,
      Models.Property property,
      IFormFile? MainImg,
      List<IFormFile>? SubImgFiles,
      string? DeletedSubImgs,
      CancellationToken cancellationToken)
        {
            if (id != property.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                var locations = await _locationRepository.GetAllAsync();
                ViewBag.Locations = locations.GroupBy(l => l.City).Select(g => g.First()).ToList();
                return View(property);
            }

            var oldProperty = await _propertyRepository.GetOneAsync(
                e => e.Id == id,
                include: new Expression<Func<Models.Property, object>>[] { p => p.PropertySubImgs }
            );

            if (oldProperty == null) return NotFound();

            // ===== تحديث البيانات الأساسية =====
            oldProperty.Title = property.Title;
            oldProperty.Price = property.Price;
            oldProperty.AreaSize = property.AreaSize;
            oldProperty.Type = property.Type;
            oldProperty.LocationId = property.LocationId;
            oldProperty.Bedrooms = property.Bedrooms;
            oldProperty.Bathrooms = property.Bathrooms;
            oldProperty.Description = property.Description;

            // ===== مسار تخزين الصور =====
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
            Directory.CreateDirectory(folder);

            // ===== تحديث الصورة الرئيسية =====
            if (MainImg != null && MainImg.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(MainImg.FileName);
                var filePath = Path.Combine(folder, fileName);
                using var stream = System.IO.File.Create(filePath);
                await MainImg.CopyToAsync(stream);

                // حذف الصورة القديمة إذا موجودة
                if (!string.IsNullOrEmpty(oldProperty.MainImg))
                {
                    var oldPath = Path.Combine(folder, oldProperty.MainImg);
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }

                oldProperty.MainImg = fileName;
            }

            // ===== حذف الصور الفرعية المطلوبة =====
            if (!string.IsNullOrEmpty(DeletedSubImgs))
            {
                var idsToDelete = DeletedSubImgs.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                .Select(int.Parse)
                                                .ToList();

                var subImgsToDelete = oldProperty.PropertySubImgs
                                                 .Where(s => idsToDelete.Contains(s.Id))
                                                 .ToList();

                foreach (var subImg in subImgsToDelete)
                {
                    var filePath = Path.Combine(folder, subImg.PropertyImgs);
                    if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);

                    oldProperty.PropertySubImgs.Remove(subImg);
                    _propertySubImageRepository.Delete(subImg);
                }
            }

            // ===== إضافة الصور الفرعية الجديدة =====
            if (SubImgFiles != null && SubImgFiles.Count > 0)
            {
                foreach (var file in SubImgFiles)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(folder, fileName);
                    using var stream = System.IO.File.Create(filePath);
                    await file.CopyToAsync(stream);

                    oldProperty.PropertySubImgs.Add(new Models.PropertySubImage
                    {
                        PropertyImgs = fileName,
                        PropertyId = oldProperty.Id
                    });
                }
            }

            // ===== حفظ التعديلات =====
            _propertyRepository.Update(oldProperty, cancellationToken: cancellationToken);
            await _propertyRepository.CommitChange(cancellationToken);

            // ===== Redirect بعد الحفظ =====
            return RedirectToAction("board", "Property", new { area = "Admin" });
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
