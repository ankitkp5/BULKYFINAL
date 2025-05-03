
using Bulky.DataAccess.Repository.IRepository;
using Bulky.models;
using Bullyweb.DataAccess.Data;

using Microsoft.AspNetCore.Mvc;

namespace Bullyweb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }
        public  IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "DisplayOrder cannot exactly match the Name.");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.save();
                TempData["message"] = "Category has been created successfully";
                return RedirectToAction("Index");
            }
            return View();
            
        }
        
        public  IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFormDb = _unitOfWork.Category.Get(u=>u.Id==id);
            //Category? categoryFormDb1=_db.Categories.FirstOrDefault(u=>u.Id==Id);
            //Category? categoryFormDb2=_db.Categories.Where(u=>u.Id==Id).FirstOrDefault();

            if (categoryFormDb == null)
            {
                return NotFound();
            }
            return View(categoryFormDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.update(obj);
                _unitOfWork.save();
                TempData["message"] = "Category has been updated successfully";
                return RedirectToAction("Index");
            }
            return View();
            
        }
        
        
        public  IActionResult Delete(int? Id)
        {
            if(Id == null || Id == 0)
            {
                return NotFound();
            }
           Category? categoryFormDb= _unitOfWork.Category.Get(u=> u.Id==Id);
            //Category? categoryFormDb1=_db.Categories.FirstOrDefault(u=>u.Id==Id);
            //Category? categoryFormDb2=_db.Categories.Where(u=>u.Id==Id).FirstOrDefault();

            if (categoryFormDb == null)
            {
                return NotFound();
            }
            return View(categoryFormDb);
        }

        [HttpPost ,ActionName ("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Remove(obj);
                _unitOfWork.save();
                TempData["message"] = "Category has been deleted successfully";
                return RedirectToAction("Index");
            }
            return View();
            
        }
    }
}
