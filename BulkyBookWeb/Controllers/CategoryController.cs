using BulkyBook.Business.Services.IServices;
using BulkyBook.Models;
using BulkyBookWeb.Data;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var category = await _categoryService.GetAllCategoryAsync();
            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> CreatePost(Category category)
        {
            if (String.IsNullOrEmpty(category.Name) && !await _categoryService.IsCategoryNameUnique(category.Name))            {
                ModelState.AddModelError("", "Category name already exists.");
            }
            if (ModelState.IsValid)
            {
                await _categoryService.CreateCategoryAsync(category);

                TempData["success"] = "Category Created Successfully.";
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var category = await _categoryService.GetCategoryByIdAsync(id); 
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Update")]
        public async Task<IActionResult> UpdatePost(Category category)
        {
            if (!String.IsNullOrEmpty(category.Name) && 
                !await _categoryService.IsCategoryNameUnique(category.Name, category.Id))
            {
                ModelState.AddModelError("", "Category name already exists.");
            }
            if (ModelState.IsValid)
            {
                await _categoryService.UpdateCategoryAsync(category);

                TempData["success"] = "Category Updated Successfully.";
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

           await _categoryService.DeleteCategoryAsync(id);
            TempData["success"] = "Category Deleted Successfully.";
            return RedirectToAction("Index");
        }


    }

}
