using Microsoft.AspNetCore.Mvc;
using BRAND.Models;
using BRAND.DATA;
using Microsoft.EntityFrameworkCore;

namespace BRAND.Controllers
{
    public class BrandController : Controller
    {
        //to retrieve from db we need to perform these steps
        private readonly ApplicationDbContext _dbContext;
        //import an interface to retrive an images 
        private readonly IWebHostEnvironment _webHostEnvironment;
        //inject into our constructor
        public BrandController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpGet]

        public IActionResult Index()
        {

            List<Brand> brands = _dbContext.Brand.ToList();
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
            //To store the images 
            string webRootPath= _webHostEnvironment.WebRootPath;

            var file = HttpContext.Request.Form.Files;

            if (file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();
                var upload = Path.Combine(webRootPath, @"images\brand");

                var extension = Path.GetExtension(file[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }
                brand.BrandLogo=@"\images\brand\" +newFileName + extension;
            }
                                                                    
            
            if (ModelState.IsValid)
            {

                _dbContext.Brand.Add(brand);
                _dbContext.SaveChanges();
                TempData["success"] = "Record created successfully";
                return RedirectToAction("Index");

            }
            return View();
        }
        [HttpGet]
        public IActionResult Details(Guid id)
        {
            Brand brand = _dbContext.Brand.FirstOrDefault(x => x.ID == id);
            return View(brand);
        }
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            Brand brand = _dbContext.Brand.FirstOrDefault(x => x.ID == id);
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

                //delete old image
                var objFromDb=_dbContext.Brand.AsNoTracking().FirstOrDefault(x=>x.ID == brand.ID);
                if (objFromDb != null)
                {

                    var oldImagePath = Path.Combine(webRootPath, objFromDb.BrandLogo.Trim('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                
                using (var fileStream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }
                brand.BrandLogo = @"\images\brand\" + newFileName + extension;
            }

            if (ModelState.IsValid)
            {
                var objFromDb = _dbContext.Brand.AsNoTracking().FirstOrDefault(x => x.ID == brand.ID);
                objFromDb.Name = brand.Name;
                objFromDb.EstabilishedYear = brand.EstabilishedYear;

                if (brand.BrandLogo != null)
                {
                    objFromDb.BrandLogo = brand.BrandLogo;
                }
                _dbContext.Brand.Update(objFromDb);
                _dbContext.SaveChanges();
                TempData["warning"] = "Record updated successfully";
                return RedirectToAction("Index");
            }
            return View();
              


        }
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            Brand brand = _dbContext.Brand.FirstOrDefault(x => x.ID == id);
            return View(brand);
        }
        [HttpPost]
        public IActionResult Delete(Brand brand)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            if (!string.IsNullOrEmpty(brand.BrandLogo))
            {
                //delete old image
                var objFromDb = _dbContext.Brand.AsNoTracking().FirstOrDefault(x => x.ID == brand.ID);
                if (objFromDb != null)
                {

                    var oldImagePath = Path.Combine(webRootPath, objFromDb.BrandLogo.Trim('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _dbContext.Brand.Remove(brand);
                _dbContext.SaveChanges();
                TempData["Error"] = "Record Deleted successfully";
                return RedirectToAction("Index");
            }
            return View();


        }




    }
}
