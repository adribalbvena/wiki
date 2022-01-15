using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Wiki.Data;
using Wiki.Models;
using Wiki.Models.Constants;
using Wiki.ViewModels;

namespace Wiki.Controllers
{
    [Authorize(Roles = Const.AdminRoleName)]
    public class CategoriesController : Controller
    {
        private readonly WikiContext _context;

        public CategoriesController(WikiContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Enabled,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (!CategoryExists(category.Name, category.Id))
                {
                    category.Id = Guid.NewGuid();
                    _context.Add(category);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else { ModelState.AddModelError("Name", ErrorMessages.CategoryAlreadyExists); }

            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Enabled,Description")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (CategoryExists(category.Name, category.Id))
                {
                    ModelState.AddModelError("Name", ErrorMessages.CategoryAlreadyExists);
                    return View(category);
                }

                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public IActionResult Delete(Guid? id)
        {
            try
            {
                return this.View(this.GetDeleteViewModel(id));
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        private DeleteCategoryViewModel GetDeleteViewModel(Guid? id)
        {
            var viewModel = new DeleteCategoryViewModel
            {
                Category = this.GetCategoryAsync(id).Result,
                CanBeDeleted = true
            };

            if (viewModel.Category.PrimaryArticles.Any())
            {
                viewModel.WarningMessage = string.Format(ErrorMessages.WarningMessageLabel, "Esta categoría es primaria y no puede ser borrada");
                viewModel.CanBeDeleted = false;
            }
            else if (viewModel.Category.SecondaryArticles.Any())
            {
                viewModel.WarningMessage = string.Format(ErrorMessages.WarningMessageLabel, "Esta categoría es secundaria");
            }

            return viewModel;
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var category = await this.GetCategoryAsync(id);

                foreach (var ac in category.SecondaryArticles)
                {
                    this._context.ArticlesCategories.Remove(ac);
                }

                this._context.Categories.Remove(category);
                await this._context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> CategoryAvailable(string name)
        {
            var category = await this._context.Categories.AnyAsync(e => e.Name == name);

            if (category)
            {
                return Json($"La categoría '{name}' ya existe");
            }

            return Json(true);
        }

        public bool CategoryExists(Guid id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

        private async Task<Category> GetCategoryAsync(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ArgumentException("Invalid Id");
            }

            var category = await this._context.Categories.Include(c => c.PrimaryArticles).Include(c => c.SecondaryArticles).FirstOrDefaultAsync(m => m.Id == id);

            if (category == null)
            {
                throw new ArgumentException("Invalid Id");
            }

            return category;
        }

        public async Task<IActionResult> ChangeEnabledStatus(Guid id)
        {
            var category = this._context.Categories.FirstOrDefault(c => c.Id == id);

            category.Enabled = !category.Enabled;
            this._context.Categories.Update(category);
            await this._context.SaveChangesAsync();
            return this.RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(string name, Guid id)
        {
            return _context.Categories.Any(e => e.Name == name && e.Id != id);
        }
    }
}
