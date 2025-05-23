using BulkyWebRazor_temp.Data;
using BulkyWebRazor_temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_temp.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Category category { get; set; }

        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int? id)
        {
            if (id != null && id != 0)
            {
                category = _db.categories.Find(id);
            }
        }
        public IActionResult OnPost()
        {
            Category? obj = _db.categories.Find(category.Id);
            if (obj == null)
            {
                return NotFound();
                
            }
            _db.categories.Remove(obj);
            _db.SaveChanges();
            TempData["message"] = "Category has been deleted successfully";
            return RedirectToPage("index");
            
        }
    }
}
