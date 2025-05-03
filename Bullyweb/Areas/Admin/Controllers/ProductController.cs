
using Bulky.DataAccess.Repository.IRepository;
using Bulky.models;
using Bulky.models.ViewModels;
using Bulky.utility;
using Bullyweb.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bullyweb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork= unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            

            return View(objProductList);
        }
        //public  IActionResult Create()
        //{
        //    IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem()
        //    {
        //        Text = i.Name,
        //        Value = i.Id.ToString()
        //    });
        //    //ViewBag.CategoryList = CategoryList; //view bag
        //    ViewData["CategoryList"] = CategoryList;  //view Data
        //    ProductVm productVm = new()
        //    {
                
        //        CategoryList = CategoryList,
        //        Product = new Product(),
        //    };

        //    return View(productVm);
        //}
        //public  IActionResult Create()
        //{
        //    ProductVm productVm = new()
        //    {
                
        //        CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem()
        //        {
        //            Text = i.Name,
        //            Value = i.Id.ToString()
        //        }),
        //        Product = new Product(),
        //    };

        //    return View(productVm);
        //}
        public  IActionResult Upsert(int? id)
        {
            ProductVm productVm = new()
            {
                
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Product = new Product(),
            };

            if(id ==null || id == 0) {
                //create product
                return View(productVm);
            }
            else
            {
                //update product
                productVm.Product = _unitOfWork.Product.Get(u => u.Id == id);
            }

            return View(productVm);
        }


        [HttpPost]
        public IActionResult Upsert(ProductVm productVm, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null) // Fix for CS8602: Ensure 'file' is not null before accessing its properties
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\products");
                    if(!string.IsNullOrEmpty(productVm.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVm.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // ✅ Ensure the directory exists

                    //if (!Directory.Exists(productPath))
                    //{
                    //    Directory.CreateDirectory(productPath);
                    //}

                    //string fullPath = Path.Combine(productPath, fileName);

                    //using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    //{
                    //    file.CopyTo(fileStream);
                    //}

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVm.Product.ImageUrl = @"\images\products\" + fileName;
                }
                if (productVm.Product.Id == 0)
                {
                    // CREATE
                    _unitOfWork.Product.Add(productVm.Product);
                    TempData["message"] = "Product has been created successfully";
                }
                else
                {
                    // UPDATE
                    _unitOfWork.Product.update(productVm.Product);
                    TempData["message"] = "Product has been updated successfully";
                }

                _unitOfWork.save();
                return RedirectToAction("Index");
                //_unitOfWork.Product.Add(productVm.Product);
                //_unitOfWork.save();
                //TempData["message"] = "Product has been created successfully";
                //return RedirectToAction("Index");
            }
            else
            {
                productVm.CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });

                return View(productVm);
            }
        }

        
        //public  IActionResult Edit(int? id)
        //{
        //    if(id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product? productFormDb = _unitOfWork.Product.Get(u=>u.Id==id);
        //    //Product? productFormDb1=_db.Categories.FirstOrDefault(u=>u.Id==Id);
        //    //Product? productFormDb2=_db.Categories.Where(u=>u.Id==Id).FirstOrDefault();

        //    if (productFormDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(productFormDb);
        //}

        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.update(obj);
        //        _unitOfWork.save();
        //        TempData["message"] = "Product has been updated successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();
            
        //}
        
        
        //public  IActionResult Delete(int? Id)
        //{
        //    if(Id == null || Id == 0)
        //    {
        //        return NotFound();
        //    }
        //   Product? productFormDb= _unitOfWork.Product.Get(u=> u.Id==Id);
        //    //Product? productFormDb1=_db.Categories.FirstOrDefault(u=>u.Id==Id);
        //    //Product? productFormDb2=_db.Categories.Where(u=>u.Id==Id).FirstOrDefault();

        //    if (productFormDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(productFormDb);
        //}

        //[HttpPost ,ActionName ("Delete")]
        //public IActionResult DeletePOST(int? id)
        //{
        //    Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Remove(obj);
        //        _unitOfWork.save();
        //        TempData["message"] = "Product has been deleted successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //   }



        #region API CALLS
        [HttpGet]
        public IActionResult getAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if(productToBeDeleted== null)
            {
                return Json(new { success = false, message = "Error  while deleteing " });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, 
                productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.save();
            return Json(new { success = false, message = "Delete succesfull " });

        }
        #endregion
    }
}
