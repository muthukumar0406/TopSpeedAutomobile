using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TopSpeed.web.data;
using TopSpeed.web.Models;

namespace TopSpeed.web.Controllers
{
    public class BrandController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BrandController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Brand> brands=_dbContext.Brand.ToList();
            return View(brands);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;

            var file = HttpContext.Request.Form.Files;

            if(file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();

                var upload = Path.Combine(webRootPath, @"images\brand");

                var extension = Path.GetExtension(file[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }
                brand.BrandLogo = @"images\brand\" + newFileName + extension;
            }
            if (ModelState.IsValid)
            {
                _dbContext.Brand.Add(brand);
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

                return View();
            
            
        }
        [HttpGet]
        public IActionResult Details(Guid id)
        {
            Brand brand = _dbContext.Brand.FirstOrDefault(x => x.Id == id);
            return View(brand);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            Brand brand = _dbContext.Brand.FirstOrDefault(x => x.Id == id);
            return View(brand);
        }

        [HttpPost]
        public IActionResult Edit(Brand brand)
        {

            string webRootPath = _webHostEnvironment.WebRootPath;

            var file = HttpContext.Request.Form.Files;

            if (file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();

                var upload = Path.Combine(webRootPath, @"images\brand");

                var extension = Path.GetExtension(file[0].FileName);

                var ObjjFroDb = _dbContext.Brand.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);

                if (ObjjFroDb.BrandLogo != null)
                {
                    var oldImgPath = Path.Combine(webRootPath, ObjjFroDb.BrandLogo.Trim('\\'));

                    if (System.IO.File.Exists(oldImgPath))
                    {
                        System.IO.File.Delete(oldImgPath);
                    }
                }

         

                using (var fileStream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }
                brand.BrandLogo = @"images\brand\" + newFileName + extension;
            }


            if (ModelState.IsValid)
            {
                var ObjjFroDb = _dbContext.Brand.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);
                ObjjFroDb.Name = brand.Name;
                ObjjFroDb.EstablishedYear = brand.EstablishedYear;
                if (brand.BrandLogo != null)
                {
                    ObjjFroDb.BrandLogo = brand.BrandLogo;
                }


                _dbContext.Brand.Update(ObjjFroDb);
                _dbContext.SaveChanges();
                TempData["Success"] = "Record Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            Brand brand = _dbContext.Brand.FirstOrDefault(x => x.Id == id);
            return View(brand);
        }

        [HttpPost]

        public IActionResult Delete(Brand brand)
        {

            string webRootPath = _webHostEnvironment.WebRootPath;
            if (string.IsNullOrEmpty(brand.BrandLogo))
                {
                var ObjjFroDb = _dbContext.Brand.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);

                if (ObjjFroDb.BrandLogo != null)
                {
                    var oldImgPath = Path.Combine(webRootPath, ObjjFroDb.BrandLogo.Trim('\\'));

                    if (System.IO.File.Exists(oldImgPath))
                    {
                        System.IO.File.Delete(oldImgPath);
                    }
                }
            }
            _dbContext.Brand.Remove(brand);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



        }
}
