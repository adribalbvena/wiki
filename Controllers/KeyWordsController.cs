using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Wiki.Data;
using Wiki.Models;
using Wiki.Models.Constants;
using Wiki.ViewModels;

namespace Wiki.Controllers
{
    [Authorize(Roles = Const.AdminRoleName)]
    public class KeyWordsController : Controller
    {
        private readonly WikiContext _context;

        public KeyWordsController(WikiContext context)
        {
            _context = context;
        }

        // GET: KeyWords
        public async Task<IActionResult> Index()
        {
            return View(await _context.KeyWords.ToListAsync());
        }

        // GET: KeyWords/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var keyWord = await _context.KeyWords
                .FirstOrDefaultAsync(m => m.Id == id);
            if (keyWord == null)
            {
                return NotFound();
            }

            return View(keyWord);
        }

        // GET: KeyWords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KeyWords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Word")] KeyWord keyWord)
        {
            if (ModelState.IsValid)
            {
                if (!KeyWordExists(keyWord.Word, keyWord.Id))
                {
                    keyWord.Id = Guid.NewGuid();
                    _context.Add(keyWord);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Word", ErrorMessages.KeyWordAlreadyExists);
                }

            }
            return View(keyWord);
        }

        // GET: KeyWords/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var keyWord = await _context.KeyWords.FindAsync(id);
            if (keyWord == null)
            {
                return NotFound();
            }
            return View(keyWord);
        }

        // POST: KeyWords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Word")] KeyWord keyWord)
        {
            if (id != keyWord.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (KeyWordExists(keyWord.Word, keyWord.Id))
                {
                    ModelState.AddModelError("Word", ErrorMessages.KeyWordAlreadyExists);
                    return View(keyWord);
                }
                try
                {
                    _context.Update(keyWord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KeyWordExists(keyWord.Id))
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
            return View(keyWord);
        }

        // GET: KeyWords/Delete/5
        public IActionResult Delete(Guid? id)
        {
            try
            {
                return View(this.GetDeleteViewModel(id));
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        private DeleteKeyWordViewModel GetDeleteViewModel(Guid? id)
        {
            var viewModel = new DeleteKeyWordViewModel
            {
                KeyWord = this.GetKeyWordAsync(id).Result,
                CanBeDeleted = true
            };

            if (viewModel.KeyWord.Articles.Any())
            {
                viewModel.WarningMessage = string.Format(ErrorMessages.WarningMessageLabel, "Esta palabra clave esta en uso en varios artículos");
            }

            return viewModel;
        }

        private async Task<KeyWord> GetKeyWordAsync(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ArgumentException("Id inválido");
            }

            var keyWord = await this._context.KeyWords.Include(kw => kw.Articles).FirstOrDefaultAsync(kw => kw.Id == id);

            if (keyWord == null)
            {
                throw new ArgumentException("Id inválido");
            }

            return keyWord;
        }


        // POST: KeyWords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var keyWord = await this.GetKeyWordAsync(id);

                foreach (var ak in keyWord.Articles)
                {
                    this._context.ArticlesKeyWords.Remove(ak);
                }

                this._context.KeyWords.Remove(keyWord);

                await this._context.SaveChangesAsync();
                return this.RedirectToAction(nameof(Index));
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> AvailableKeyWord(string word)
        {
            var keyWord = await this._context.KeyWords.AnyAsync(e => e.Word == word);

            if (keyWord)
            {
                return Json($"La palabra clave '{word}' ya existe");
            }

            return Json(true);
        }

        private bool KeyWordExists(Guid id)
        {
            return _context.KeyWords.Any(e => e.Id == id);
        }

        private bool KeyWordExists(string word, Guid id)
        {
            return _context.KeyWords.Any(e => e.Word == word && e.Id != id);
        }
    }
}
