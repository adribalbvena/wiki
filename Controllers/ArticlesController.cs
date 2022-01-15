using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Wiki.Data;
using Wiki.Models;
using Wiki.Models.Constants;
using Wiki.ViewModels;
using Wiki.ViewModels.Articles;

namespace Wiki.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {
        private readonly WikiContext _context;

        private readonly SignInManager<User> _signInManager;

        public ArticlesController(WikiContext context, SignInManager<User> signInManager)
        {
            this._context = context;
            this._signInManager = signInManager;
        }

        // GET: Articles
        [Authorize(Roles = Const.AdminRoleName)]
        public async Task<IActionResult> Index()
        {
            var articles = this._context.Articles.Include(a => a.Header).Include(a => a.Author).Include(a => a.PrimaryCategory);
            return this.View(await articles/*.OrderBy(a => a.Header.Title)*/.ToListAsync());
        }

        [Authorize(Roles = Const.AuthorRoleName)]
        public async Task<IActionResult> MyArticles()
        {
            var user = await this._signInManager.UserManager.GetUserAsync(this.User);
            var articles = this._context.Articles.Include(a => a.Header).Include(a => a.PrimaryCategory).Where(a => a.Author.Id == user.Id);
            return this.View(await articles/*.OrderBy(a => a.CreationDate)*/.ToListAsync());
        }

        public IActionResult Navigation(Guid? categoryId, Guid? keyWordId)
        {
            try
            {
                return this.View(this.GetNavigationViewModel(categoryId, keyWordId));
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        private ArticleNavigationViewModel GetNavigationViewModel(Guid? categoryId, Guid? keyWordId)
        {
            var viewModel = new ArticleNavigationViewModel();

            if (categoryId.HasValue)
            {
                viewModel.Category = this._context.Categories.FirstOrDefault(c => c.Id == categoryId.Value);

                if (viewModel.Category == null)
                {
                    throw new ArgumentException();
                }
            }

            if (keyWordId.HasValue)
            {
                viewModel.KeyWord = this._context.KeyWords.FirstOrDefault(k => k.Id == keyWordId.Value);

                if (viewModel.KeyWord == null)
                {
                    throw new ArgumentException();
                }
            }

            viewModel.Articles = this.SearchArticles(categoryId: categoryId, keyWordId: keyWordId).Include(a => a.Author).ToList();

            return viewModel;
        }

        public async Task<IActionResult> Reader(Guid? id)
        {
            try
            {
                var article = await this.LoadArticleForReader(id);

                return this.View(this.GetArticleReaderViewModel(article));
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> NewMessage(Guid? articleId, ArticleReaderViewModel viewModel)
        {
            //this.ModelState.Clear();

            try
            {
                var article = await this.LoadArticleForReader(articleId);

                if (string.IsNullOrWhiteSpace(viewModel.NewMessage.Title))
                {
                    ModelState.AddModelError($"{nameof(viewModel.NewMessage)}.{nameof(viewModel.NewMessage.Title)}", "El título no puede estar vacío");
                }

                if (string.IsNullOrWhiteSpace(viewModel.NewMessage.Text))
                {
                    ModelState.AddModelError($"{nameof(viewModel.NewMessage)}.{nameof(viewModel.NewMessage.Text)}", "El texto no puede estar vacío");
                }

                if (this.ModelState.IsValid)
                {
                    this.SaveMessage(article, viewModel.NewMessage);

                    return this.RedirectToAction(nameof(Reader), new { Id = articleId });
                }

                viewModel = this.GetArticleReaderViewModel(article);
            }
            catch (ArgumentException ae)
            {
                ModelState.AddModelError(string.Empty, ae.Message);
            }

            return this.View(nameof(Reader), viewModel);
        }

        private ArticleReaderViewModel GetArticleReaderViewModel(Article article)
        {
            var viewModel = new ArticleReaderViewModel
            {
                ArticleId = article.Id
            };

            var user = this._signInManager.UserManager.GetUserAsync(this.User).Result;


            viewModel.CanEdit = article.Author.Id == user.Id;
            viewModel.Title = article.Title;
            viewModel.Description = article.Description;
            viewModel.PrimaryCategory = new ArticleCategoryViewModel { Id = article.PrimaryCategory.Id, Name = article.PrimaryCategory.Name };
            viewModel.SecondaryCategories = article.SecondaryCategories.Select(sc => new ArticleCategoryViewModel { Id = sc.Category.Id, Name = sc.Category.Name }).ToList();
            viewModel.Entries = article.Body.Entries.OrderBy(e => e.Order).Select(e => new ArticleEntryViewModel { Order = e.Order, Title = e.Title, SubTitle = e.Subtitle, Text = e.Text }).ToList();
            viewModel.KeyWords = article.KeyWords.Select(kw => new KeyWordViewModel { Id = kw.KeyWord.Id, Word = kw.KeyWord.Word }).ToList();
            viewModel.References = article.MainReferences.Select(r => new ArticleReferenceViewModel { Id = r.ReferenceArticle.Id, Title = r.ReferenceArticle.Title }).ToList();
            viewModel.Messages = article.Messages.OrderBy(m => m.Date).Select(m => new ArticleMessageViewModel { AuthorName = m.User.CompleteName, Date = m.Date, Title = m.Title, Text = m.Text }).ToList();

            return viewModel;
        }

        private void SaveMessage(Article article, ArticleMessageViewModel viewModel)
        {
            var user = this._signInManager.UserManager.GetUserAsync(this.User).Result;

            var message = new Message
            {
                Id = Guid.NewGuid(),
                User = this._context.Users.FirstOrDefault(u => u.Id == user.Id),
                Date = DateTime.Now,
                Title = viewModel.Title,
                Text = viewModel.Text,
                Article = article
            };

            this._context.Messages.Add(message);

            this._context.SaveChanges();
        }

        #region Editor

        [Authorize(Roles = Const.AuthorRoleName)]
        public IActionResult Editor(Guid? id)
        {
            try
            {
                return this.View(this.GetArticleViewModel(id));
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return this.RedirectToAction(nameof(MyArticles));
            }
        }

        [HttpPost]
        [Authorize(Roles = Const.AuthorRoleName)]
        public async Task<IActionResult> Editor(ArticleViewModel viewModel, Guid? id = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(viewModel.Title))
                {
                    ModelState.AddModelError(nameof(viewModel.Title), string.Format(ErrorMessages.Required, "título"));
                }
                else if (ArticleExists(id ?? Guid.Empty, viewModel.Title))
                {
                    ModelState.AddModelError(nameof(viewModel.Title), ErrorMessages.ArticleAlreadyExists);
                }

                if (!viewModel.PrimaryCategoryId.HasValue && string.IsNullOrWhiteSpace(viewModel.Name))
                {
                    ModelState.AddModelError(nameof(viewModel.Name), ErrorMessages.CategoryRequired);
                }
                else if (!this.CategoryAvailable(viewModel.Name))
                {
                    ModelState.AddModelError(nameof(viewModel.Name), ErrorMessages.CategoryAlreadyExists);
                }
                else if (!viewModel.ValidatePrimaryCategory())
                {
                    ModelState.AddModelError(nameof(viewModel.PrimaryCategoryId), ErrorMessages.CategoryAlreadySecondary);
                }

                if (ModelState.IsValid)
                {
                    var article = await this.SaveArticle(viewModel, id);

                    return this.RedirectToAction(nameof(EditorEntries), new { id = article.Id });
                }
            }
            catch (ArgumentException ae)
            {
                ModelState.AddModelError(string.Empty, ae.Message);
            }
            catch (UnauthorizedAccessException)
            {
                return this.RedirectToAction(nameof(MyArticles));
            }

            this.LoadViewData();
            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = Const.AuthorRoleName)]
        public async Task<IActionResult> AddSecondaryCategory(ArticleViewModel viewModel)
        {
            /* add to model */
            if (viewModel.SecondaryCategoryId.HasValue)
            {
                if (viewModel.PrimaryCategoryId.HasValue && viewModel.PrimaryCategoryId.Value == viewModel.SecondaryCategoryId.Value)
                {
                    ModelState.AddModelError(nameof(viewModel.SecondaryCategoryId), ErrorMessages.CategoryAlreadyPrimary);
                }
                else
                {
                    var category = await this.GetCategory(viewModel.SecondaryCategoryId.Value);

                    if (category != null)
                    {
                        viewModel.AddSecondaryCategory(category.Id, category.Name);
                        viewModel.SecondaryCategoryId = null;
                        this.ModelState.Clear();
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(viewModel.SecondaryCategoryId), ErrorMessages.CategoryDoesNotExist);
                    }
                }
            }
            else
            {
                ModelState.AddModelError(nameof(viewModel.SecondaryCategoryId), ErrorMessages.CategoryDoesNotExist);
            }

            this.LoadViewData();
            return this.View(nameof(Editor), viewModel);
        }

        [HttpPost]
        [Authorize(Roles = Const.AuthorRoleName)]
        public IActionResult RemoveSecondaryCategory(ArticleViewModel viewModel, Guid? id = null)
        {
            /* validate id */
            if (id != null)
            {
                viewModel.RemoveSecondaryCategory(id.Value);
                viewModel.SecondaryCategoryId = null;
                this.ModelState.Clear();
            }

            this.LoadViewData();
            return this.View(nameof(Editor), viewModel);
        }

        private ArticleViewModel GetArticleViewModel(Guid? id)
        {
            Article article = null;

            var viewModel = new ArticleViewModel();

            if (id.HasValue)
            {
                article = this.LoadArticle(id.Value).Result;

                viewModel.ArticleId = article.Id;
                viewModel.Title = article.Title;
                viewModel.Description = article.Description;

                foreach (var sc in article.SecondaryCategories)
                {
                    viewModel.AddSecondaryCategory(sc.Category.Id, sc.Category.Name);
                }
            }

            this.LoadViewData(article);
            return viewModel;
        }

        private async Task<Category> GetCategory(Guid id)
        {
            return await this._context.Categories.FirstOrDefaultAsync(a => a.Id == id);
        }

        private bool CategoryAvailable(string name)
        {
            return !this._context.Categories.Any(e => e.Name == name);
        }

        #endregion

        #region Entries

        [Authorize(Roles = Const.AuthorRoleName)]
        public IActionResult EditorEntries(Guid? id)
        {
            try
            {
                return this.View(this.GetArticleEntriesViewModel(id));
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize(Roles = Const.AuthorRoleName)]
        public async Task<IActionResult> EditorEntries(Guid id, ArticleEntriesViewModel viewModel)
        {
            try
            {
                var article = await this.LoadArticle(id);

                if (article != null)
                {
                    this.SaveArticleEntries(viewModel, article);

                    this._context.SaveChanges();

                    return this.RedirectToAction(nameof(EditorKeyWords), new { id = article.Id });
                }
            }
            catch (ArgumentException ae)
            {
                ModelState.AddModelError(string.Empty, ae.Message);
            }

            return this.View(viewModel);
        }

        [Authorize(Roles = Const.AuthorRoleName)]
        public IActionResult AddEntry(ArticleEntriesViewModel viewModel)
        {
            if (viewModel.Entry != null)
            {
                if (string.IsNullOrWhiteSpace(viewModel.Entry.Title))
                {
                    ModelState.AddModelError($"{nameof(viewModel.Entry)}.{nameof(viewModel.Entry.Title)}", "El campo título no puede estar vacío");
                }

                if (string.IsNullOrWhiteSpace(viewModel.Entry.Text))
                {
                    ModelState.AddModelError($"{nameof(viewModel.Entry)}.{nameof(viewModel.Entry.Text)}", "El campo texto no puede estar vacío");
                }
                else if (viewModel.Entry.Text.Length > MaxLength.EntryText)
                {
                    ModelState.AddModelError($"{nameof(viewModel.Entry)}.{nameof(viewModel.Entry.Text)}", $"El campo texto no puede superar los {MaxLength.EntryText} caracteres");
                }

                if (ModelState.IsValid)
                {
                    viewModel.AddEntry();
                    ModelState.Clear();
                }
            }

            return this.View(nameof(EditorEntries), viewModel);
        }

        [Authorize(Roles = Const.AuthorRoleName)]
        public IActionResult RemoveEntry(ArticleEntriesViewModel viewModel, int? order = null)
        {
            if (order.HasValue)
            {
                viewModel.RemoveEntry(order.Value);
                this.ModelState.Clear();
            }

            return this.View(nameof(EditorEntries), viewModel);
        }

        [Authorize(Roles = Const.AuthorRoleName)]
        public IActionResult EditEntry(ArticleEntriesViewModel viewModel, int? order = null)
        {
            if (order.HasValue)
            {
                this.ModelState.Clear();
                viewModel.EditEntry(order.Value);
            }

            return this.View(nameof(EditorEntries), viewModel);
        }

        [Authorize(Roles = Const.AuthorRoleName)]
        public IActionResult CancelEntry(ArticleEntriesViewModel viewModel)
        {
            this.ModelState.Clear();
            viewModel.CancelEntry();
            return this.View(nameof(EditorEntries), viewModel);
        }

        [Authorize(Roles = Const.AuthorRoleName)]
        public IActionResult SaveEntry(ArticleEntriesViewModel viewModel, int? order = null)
        {
            ModelState.Clear();

            if (order.HasValue)
            {
                if (viewModel.Entry != null)
                {
                    if (string.IsNullOrWhiteSpace(viewModel.Entry.Title))
                    {
                        ModelState.AddModelError($"{nameof(viewModel.Entry)}.{nameof(viewModel.Entry.Title)}", "El campo título no puede estar vacío");
                    }

                    if (string.IsNullOrWhiteSpace(viewModel.Entry.Text))
                    {
                        ModelState.AddModelError($"{nameof(viewModel.Entry)}.{nameof(viewModel.Entry.Text)}", "El campo texto no puede estar vacío");
                    }
                    else if (viewModel.Entry.Text.Length > MaxLength.EntryText)
                    {
                        ModelState.AddModelError($"{nameof(viewModel.Entry)}.{nameof(viewModel.Entry.Text)}", $"El campo texto no puede superar los {MaxLength.EntryText} caracteres");
                    }

                    if (ModelState.IsValid)
                    {
                        viewModel.SaveEntry(order.Value);
                        this.ModelState.Clear();
                    }
                }
            }

            return this.View(nameof(EditorEntries), viewModel);
        }

        private ArticleEntriesViewModel GetArticleEntriesViewModel(Guid? id)
        {
            var viewModel = new ArticleEntriesViewModel();

            var result = this.LoadArticle(id);

            result.Wait();

            var article = result.Result;

            viewModel.ArticleId = article.Id;

            if (article.Body.Entries.Any())
            {
                foreach (var e in article.Body.Entries)
                {
                    viewModel.AddEntry(e.Order, e.Title, e.Subtitle, e.Text, e.Id);
                }

                viewModel.Entry.Order = article.Body.Entries.Max(e => e.Order) + 1;
            }

            return viewModel;
        }

        private void SaveArticleEntries(ArticleEntriesViewModel viewModel, Article article)
        {
            foreach (var entry in article.Body.Entries)
            {
                if (!viewModel.Entries.Any(e => e.Id == entry.Id))
                {
                    this._context.Entries.Remove(entry);
                }
            }

            foreach (var entry in viewModel.Entries)
            {
                this.SaveEntry(entry, article);
            }
        }

        private void SaveEntry(ArticleEntryViewModel viewModel, Article article)
        {
            Entry entry;

            if (viewModel.Id.HasValue)
            {
                entry = this._context.Entries.FirstOrDefault(e => e.Id == viewModel.Id.Value);

                if (entry == null)
                {
                    throw new ArgumentException("Entrada inválida");
                }
            }
            else
            {
                entry = new Entry
                {
                    Id = Guid.NewGuid(),
                    Body = article.Body
                };

                this._context.Entries.Add(entry);
            }

            entry.Order = viewModel.Order;
            entry.Title = viewModel.Title;
            entry.Subtitle = viewModel.SubTitle;
            entry.Text = viewModel.Text;
        }

        #endregion

        #region KeyWords

        [Authorize(Roles = Const.AuthorRoleName)]
        public IActionResult EditorKeyWords(Guid id)
        {
            try
            {
                return this.View(this.GetArticleKeyWordsViewModel(id));
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditorKeyWords(Guid? id, ArticleKeyWordsViewModel viewModel)
        {
            try
            {
                var article = await this.LoadArticle(id);

                if (article != null)
                {
                    this.SaveArticleKeyWords(viewModel, article);

                    await this._context.SaveChangesAsync();

                    return this.RedirectToAction(nameof(EditorReferences), new { id });
                }
            }
            catch (ArgumentException ae)
            {
                ModelState.AddModelError(string.Empty, ae.Message);
            }

            return this.View(viewModel);
        }

        [Authorize(Roles = Const.AuthorRoleName)]
        public IActionResult SearchKeyWord(ArticleKeyWordsViewModel viewModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(viewModel.SearchText))
                {
                    ModelState.AddModelError(nameof(viewModel.SearchText), ErrorMessages.KeyWordRequired);
                }

                if (ModelState.IsValid)
                {
                    viewModel.Results = this.SearchKeyWords(viewModel.SearchText).OrderBy(kw => kw.Word).Take(Const.SearchResults).Select(kw => new KeyWordViewModel { Id = kw.Id, Word = kw.Word }).ToList();
                }
            }
            catch
            {
                return NotFound();
            }

            return this.View(nameof(EditorKeyWords), viewModel);
        }

        [Authorize(Roles = Const.AuthorRoleName)]
        public async Task<IActionResult> AddKeyWord(ArticleKeyWordsViewModel viewModel, Guid? id = null)
        {
            if (id.HasValue)
            {
                var keyWord = await this.GetKeyWord(id.Value);

                if (keyWord == null)
                {
                    ModelState.AddModelError($"{nameof(viewModel.KeyWord)}.{nameof(viewModel.KeyWord.Id)}", ErrorMessages.KeyWordRequired);
                }

                if (ModelState.IsValid)
                {
                    viewModel.AddKeyWord(keyWord.Id, keyWord.Word);
                }
            }
            else
            {
                ModelState.AddModelError($"{nameof(viewModel.KeyWord)}.{nameof(viewModel.KeyWord.Id)}", ErrorMessages.KeyWordRequired);
            }

            this.LoadViewData();
            return this.View(nameof(EditorKeyWords), viewModel);
        }

        [Authorize(Roles = Const.AuthorRoleName)]
        public async Task<IActionResult> CreateKeyWord(ArticleKeyWordsViewModel viewModel)
        {
            if (viewModel.KeyWord != null)
            {
                if (string.IsNullOrWhiteSpace(viewModel.KeyWord.Word))
                {
                    ModelState.AddModelError($"{nameof(viewModel.KeyWord)}.{nameof(viewModel.KeyWord.Word)}", ErrorMessages.KeyWordRequired);
                }
                else if (await this.ValidateKeyWord(viewModel.KeyWord.Word))
                {
                    ModelState.AddModelError($"{nameof(viewModel.KeyWord)}.{nameof(viewModel.KeyWord.Word)}", ErrorMessages.KeyWordAlreadyExists);
                }

                if (ModelState.IsValid)
                {
                    viewModel.AddKeyWord();
                    ModelState.Clear();
                }
            }
            else
            {
                ModelState.AddModelError($"{nameof(viewModel.KeyWord)}.{nameof(viewModel.KeyWord.Word)}", ErrorMessages.KeyWordRequired);
            }

            this.LoadViewData();
            return this.View(nameof(EditorKeyWords), viewModel);
        }

        [Authorize(Roles = Const.AuthorRoleName)]
        public IActionResult RemoveKeyWord(ArticleKeyWordsViewModel viewModel, string word)
        {
            /* validate id */
            if (!string.IsNullOrWhiteSpace(word))
            {
                viewModel.RemoveKeyWord(word);
                viewModel.KeyWord.Id = null;
                viewModel.KeyWord.Word = string.Empty;
            }

            this.ModelState.Clear();
            this.LoadViewData();
            return this.View(nameof(EditorKeyWords), viewModel);
        }

        private ArticleKeyWordsViewModel GetArticleKeyWordsViewModel(Guid id)
        {
            var viewModel = new ArticleKeyWordsViewModel();

            var result = this.LoadArticle(id);

            result.Wait();

            var article = result.Result;

            viewModel.ArticleId = article.Id;

            if (article.KeyWords.Any())
            {
                foreach (var kw in article.KeyWords.Select(kw => kw.KeyWord))
                {
                    viewModel.AddKeyWord(kw.Id, kw.Word);
                }
            }

            this.LoadViewData(article);
            return viewModel;
        }

        private void AddArticleKeyWord(KeyWordViewModel viewModel, Article article)
        {
            KeyWord keyWord;

            if (viewModel.Id.HasValue)
            {
                keyWord = this._context.KeyWords.FirstOrDefault(kw => kw.Id == viewModel.Id.Value);

                if (keyWord == null)
                {
                    throw new ArgumentException("Palabra clave inválida");
                }

                /* La palabra ya esta vinculada al artículo, no necesito hacer nada */
                if (article.KeyWords.Any(kw => kw.KeyWord.Id == keyWord.Id))
                {
                    return;
                }
            }
            else
            {
                /* TODO: validar que no exista */

                keyWord = new KeyWord
                {
                    Id = Guid.NewGuid(),
                    Word = viewModel.Word
                };

                this._context.KeyWords.Add(keyWord);
            }

            var articleKeyword = new ArticleKeyWord { KeyWord = keyWord, Article = article };

            this._context.ArticlesKeyWords.Add(articleKeyword);
        }

        private void SaveArticleKeyWords(ArticleKeyWordsViewModel viewModel, Article article)
        {
            foreach (var kw in article.KeyWords)
            {
                if (!viewModel.KeyWords.Any(key => key.Id == kw.KeyWord.Id))
                {
                    this._context.ArticlesKeyWords.Remove(kw);
                }
            }

            foreach (var kw in viewModel.KeyWords)
            {
                this.AddArticleKeyWord(kw, article);
            }
        }

        private async Task<KeyWord> GetKeyWord(Guid id)
        {
            return await this._context.KeyWords.FirstOrDefaultAsync(a => a.Id == id);
        }

        private async Task<bool> ValidateKeyWord(string word)
        {
            return await this._context.KeyWords.AnyAsync(kw => kw.Word == word);
        }

        #endregion

        #region References

        [Authorize(Roles = Const.AuthorRoleName)]
        public IActionResult EditorReferences(Guid id)
        {
            try
            {
                return this.View(this.GetArticleReferencesViewModel(id));
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize(Roles = Const.AuthorRoleName)]
        public async Task<IActionResult> EditorReferences(Guid id, ArticleReferencesViewModel viewModel)
        {
            this.ModelState.Clear();

            try
            {
                var article = await this.LoadArticle(id);

                if (article != null)
                {
                    this.SaveArticleReferences(viewModel, article);

                    await this._context.SaveChangesAsync();

                    return this.RedirectToAction(nameof(MyArticles));
                }
            }
            catch (ArgumentException ae)
            {
                ModelState.AddModelError(string.Empty, ae.Message);
            }

            this.LoadViewData();
            return this.View(viewModel);
        }

        [Authorize(Roles = Const.AuthorRoleName)]
        public IActionResult SearchReference(ArticleReferencesViewModel viewModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(viewModel.Title))
                {
                    ModelState.AddModelError(nameof(viewModel.Title), ErrorMessages.ArticleRequired);
                }

                if (ModelState.IsValid)
                {
                    /* Filtro por el artículo editado y obtengo los primeros 10 resultados */
                    viewModel.Results = this.SearchArticles(viewModel.Title).Where(a => a.Id != viewModel.ArticleId).OrderBy(a => a.Header.Title).Take(Const.SearchResults).Select(a => new ArticleReferenceViewModel { Id = a.Id, Title = a.Title }).ToList();
                }
            }
            catch
            {
                return NotFound();
            }

            return this.View(nameof(EditorReferences), viewModel);
        }

        [Authorize(Roles = Const.AuthorRoleName)]
        public async Task<IActionResult> AddReference(ArticleReferencesViewModel viewModel, Guid? id = null)
        {
            if (id.HasValue)
            {
                var article = await this._context.Articles.Include(a => a.Header).FirstOrDefaultAsync(a => a.Id == id.Value);

                if (article == null || article.Id == viewModel.ArticleId)
                {
                    ModelState.AddModelError(nameof(viewModel.Title), ErrorMessages.ArticleInvalid);
                }

                if (ModelState.IsValid)
                {
                    viewModel.AddReference(article.Id, article.Title);
                    ModelState.Clear();
                }
            }
            else
            {
                ModelState.AddModelError(nameof(viewModel.Title), ErrorMessages.ArticleInvalid);
            }

            this.LoadViewData();
            return this.View(nameof(EditorReferences), viewModel);
        }

        [Authorize(Roles = Const.AuthorRoleName)]
        public IActionResult RemoveReference(ArticleReferencesViewModel viewModel, Guid? id = null)
        {
            /* validate id */
            if (id.HasValue)
            {
                viewModel.RemoveReference(id.Value);
            }

            this.LoadViewData();
            return this.View(nameof(EditorReferences), viewModel);
        }

        private ArticleReferencesViewModel GetArticleReferencesViewModel(Guid id)
        {
            var viewModel = new ArticleReferencesViewModel();

            var article = this.LoadArticle(id).Result;

            viewModel.ArticleId = article.Id;

            if (article.MainReferences.Any())
            {
                foreach (var a in article.MainReferences.Select(a => a.ReferenceArticle))
                {
                    viewModel.AddReference(a.Id, a.Title);
                }
            }

            this.LoadViewData(article);
            return viewModel;
        }

        private void AddArticleReference(ArticleReferenceViewModel viewModel, Article article)
        {
            if (!viewModel.Id.HasValue)
            {
                return;
            }

            var reference = this._context.References.FirstOrDefault(r => r.ReferenceArticle.Id == viewModel.Id.Value && r.MainArticle.Id == article.Id);

            /* ya existe la referencia entre estos artículos, no debo hacer nada */
            if (reference != null)
            {
                return;
            }

            var referenceArticle = this._context.Articles.FirstOrDefault(a => a.Id == viewModel.Id.Value);

            if (referenceArticle == null)
            {
                throw new ArgumentException("Artículo de referencia inválido");
            }

            if (referenceArticle.Id == article.Id)
            {
                throw new ArgumentException("No se puede referenciar al mismo artículo");
            }

            reference = new Reference { ReferenceArticle = referenceArticle, MainArticle = article };

            this._context.References.Add(reference);
        }

        private void SaveArticleReferences(ArticleReferencesViewModel viewModel, Article article)
        {
            foreach (var main in article.MainReferences)
            {
                if (!viewModel.References.Any(r => r.Id == main.ReferenceArticle.Id))
                {
                    this._context.References.Remove(main);
                }
            }

            foreach (var reference in viewModel.References)
            {
                this.AddArticleReference(reference, article);
            }
        }

        #endregion

        // GET: Articles/Delete/5
        [Authorize(Roles = Const.AdminRoleName)]
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

        private DeleteArticleViewModel GetDeleteViewModel(Guid? id)
        {
            var viewModel = new DeleteArticleViewModel
            {
                Article = this.LoadArticleForReader(id.Value).Result,
                CanBeDeleted = true
            };

            if (viewModel.Article.References.Any())
            {
                viewModel.WarningMessage = string.Format(ErrorMessages.WarningMessageLabel, "Este artículo esta referenciado por otros artículos");
            }

            return viewModel;
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Const.AdminRoleName)]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var article = await this.LoadArticleForReader(id);

                foreach (var ak in article.KeyWords)
                {
                    this._context.ArticlesKeyWords.Remove(ak);
                }

                foreach (var ac in article.SecondaryCategories)
                {
                    this._context.ArticlesCategories.Remove(ac);
                }

                foreach (var mr in article.MainReferences)
                {
                    this._context.References.Remove(mr);
                }

                foreach (var r in article.References)
                {
                    this._context.References.Remove(r);
                }

                foreach (var m in article.Messages)
                {
                    this._context.Messages.Remove(m);
                }

                this._context.Articles.Remove(article);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction(nameof(Index));
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> ChangeEnabledStatus(Guid id, string returnUrl)
        {
            Article article;

            if (User.IsInRole(Const.AdminRoleName))
            {
                article = await this.LoadArticleForReader(id);
            }
            else
            {
                article = await this.LoadArticle(id);
            }

            article.Enabled = article.PrimaryCategory.Enabled && !article.Enabled;

            this._context.Articles.Update(article);
            await this._context.SaveChangesAsync();
            return this.RedirectToAction(returnUrl);
        }

        public async Task<IActionResult> ArticleAvailable(string title, Guid? articleId = null)
        {
            var article = await this._context.Articles.Include(a => a.Header).AnyAsync(a => a.Header.Title == title && (!articleId.HasValue || articleId.Value != a.Id));

            if (article)
            {
                return Json($"Ya existe un artículo con ese título");
            }

            return Json(true);
        }

        private bool ArticleExists(Guid id)
        {
            return this._context.Articles.Any(e => e.Id == id);
        }

        private bool ArticleExists(Guid id, string title)
        {
            return this._context.Articles.Any(e => e.Header.Title == title && e.Id != id);
        }

        private void LoadViewData(Article a = null)
        {
            //this.ViewData[nameof(Article.AuthorId)] = new SelectList(this._context.Authors, nameof(Author.Id), nameof(Author.CompleteName), a?.AuthorId);

            var categories = this._context.Categories;
            var primaryCategories = new SelectList(categories, nameof(Category.Id), nameof(Category.EnabledName), a?.PrimaryCategoryId).ToList();
            primaryCategories.Add(new SelectListItem { Value = string.Empty, Text = "Crear categoría nueva", Selected = (a == null) });
            this.ViewData[nameof(Article.PrimaryCategoryId)] = primaryCategories;

            var secondaryCategories = new SelectList(categories, nameof(Category.Id), nameof(Category.EnabledName)).ToList();
            secondaryCategories.Add(new SelectListItem { Value = string.Empty, Text = "Seleccione una", Selected = true });
            this.ViewData[nameof(Article.SecondaryCategories)] = secondaryCategories;

            var keyWordsSelectList = new SelectList(this._context.KeyWords, nameof(KeyWord.Id), nameof(KeyWord.Word)).ToList();
            keyWordsSelectList.Add(new SelectListItem { Value = string.Empty, Text = "Seleccione una", Selected = true });
            this.ViewData[nameof(Article.KeyWords)] = keyWordsSelectList;

            var articlesSelectList = new SelectList(this._context.Articles.Include(a => a.Header).Where(x => a == null || x.Id != a.Id), nameof(Article.Id), nameof(Article.Title)).ToList();
            articlesSelectList.Add(new SelectListItem { Value = string.Empty, Text = "Seleccione uno", Selected = true });
            this.ViewData[nameof(Article.References)] = articlesSelectList;
        }

        private async Task<Article> LoadArticle(Guid? id)
        {
            var user = await this._signInManager.UserManager.GetUserAsync(this.User);

            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            if (!id.HasValue)
            {
                throw new ArgumentException("Invalid Article Id");
            }

            var article = await this._context.Articles
                .Include(a => a.Header)
                .Include(a => a.Body)
                .ThenInclude(b => b.Entries)
                .Include(a => a.Author)
                .Include(a => a.PrimaryCategory)
                .Include(a => a.KeyWords)
                .ThenInclude(kw => kw.KeyWord)
                .Include(a => a.SecondaryCategories)
                .ThenInclude(sc => sc.Category)
                .Include(a => a.References)
                .Include(a => a.MainReferences)
                .ThenInclude(r => r.ReferenceArticle)
                .ThenInclude(ra => ra.Header)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (article == null)
            {
                throw new ArgumentException("Invalid Article Id");
            }

            if (article.Author.Id != user.Id)
            {
                throw new UnauthorizedAccessException();
            }

            return article;
        }

        private async Task<Article> LoadArticleForReader(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ArgumentException("Invalid Article Id");
            }

            var article = await this._context.Articles
                .Include(a => a.Header)
                .Include(a => a.Body)
                .ThenInclude(b => b.Entries)
                .Include(a => a.Author)
                .Include(a => a.PrimaryCategory)
                .Include(a => a.KeyWords)
                .ThenInclude(kw => kw.KeyWord)
                .Include(a => a.SecondaryCategories)
                .ThenInclude(sc => sc.Category)
                .Include(a => a.References)
                .Include(a => a.MainReferences)
                .ThenInclude(r => r.ReferenceArticle)
                .ThenInclude(ra => ra.Header)
                .Include(a => a.Messages)
                .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (article == null)
            {
                throw new ArgumentException("Invalid Article Id");
            }

            return article;
        }

        private async Task<Article> SaveArticle(ArticleViewModel viewModel, Guid? id = null)
        {
            Article article = null;

            if (id.HasValue)
            {
                article = await this.LoadArticle(id.Value);
            }
            else
            {
                var user = await this._signInManager.UserManager.GetUserAsync(this.User);

                if (user == null)
                {
                    throw new UnauthorizedAccessException();
                }

                /* creo el artículo */
                article = new Article
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.UtcNow,
                    Author = await this._context.Authors.FirstOrDefaultAsync(a => a.Id == user.Id),
                    Enabled = false
                };

                /* lo agrego al contexto */
                this._context.Articles.Add(article);

                /* creo la cabecera */
                var header = new Header
                {
                    Id = Guid.NewGuid(),
                    Article = article
                };

                /* la agrego al contexto */
                this._context.Headers.Add(header);

                /* creo el cuerpo */
                var body = new Body
                {
                    Id = Guid.NewGuid(),
                    Article = article
                };

                /* lo agrego al contexto */
                this._context.Bodies.Add(body);

                /* creo la lista de categorías secundarias */
                article.SecondaryCategories = new List<ArticleCategory>();
            }

            if (ArticleExists(article.Id, viewModel.Title))
            {
                throw new ArgumentException("Ya existe un artículo con ese título");
            }

            /* aplico los cambios en la cabecera */
            article.Header.Title = viewModel.Title;
            article.Header.Description = viewModel.Description;

            /* aplico los cambios sobre la categoría primaria */
            if (viewModel.PrimaryCategoryId.HasValue)
            {
                article.PrimaryCategory = await this.GetCategory(viewModel.PrimaryCategoryId.Value);

                if (article.PrimaryCategory == null)
                {
                    throw new ArgumentException("Categoría inválida");
                }
            }
            else
            {
                /* creo la categoría */
                var category = new Category
                {
                    Id = Guid.NewGuid(),
                    Name = viewModel.Name,
                    Enabled = false
                };

                this._context.Categories.Add(category);

                /* TODO: agregar notificación al usuario cuando se active la categoría */

                article.PrimaryCategory = category;
            }

            /* borro las categorías secundarias del artículo */
            foreach (var sc in article.SecondaryCategories)
            {
                /* la categoría que ya existe en la base no es parte del viewmodel (se borró) */
                if (!viewModel.SecondaryCategories.Any(c => c.Id == sc.Category.Id))
                {
                    this._context.ArticlesCategories.Remove(sc);
                }
            }

            /* agrego las categorías secundarias */
            foreach (var sc in viewModel.SecondaryCategories)
            {
                /* Si ya tiene la categoría, no debo crear una relación nueva */
                if (!article.SecondaryCategories.Any(c => c.Category.Id == sc.Id))
                {
                    var category = new ArticleCategory
                    {
                        //Article = article,
                        Category = await this.GetCategory(sc.Id)
                    };

                    article.SecondaryCategories.Add(category);
                }
            }

            await this._context.SaveChangesAsync();

            return article;
        }

        private IQueryable<Article> SearchArticles(string title = null, Guid? categoryId = null, Guid? keyWordId = null)
        {
            var user = this._signInManager.UserManager.GetUserAsync(this.User).Result;

            /* Los artículos visibles son aquellos que están habilitados y su categoría primaria también o el usuario que consulta es el autor */
            var articles = this._context.Articles.Include(a => a.Header).Include(a => a.PrimaryCategory).Where(a => (a.Enabled && a.PrimaryCategory.Enabled) || a.AuthorId == user.Id);

            /* busco los artículos con el parámetro en su título */
            if (!string.IsNullOrWhiteSpace(title))
            {
                articles = articles.Where(a => a.Header.Title.Contains(title));
            }

            /* busco los artículos que tengan el parámetro como categoría primaria o secundaria */
            if (categoryId.HasValue)
            {
                articles = articles.Include(a => a.SecondaryCategories).Where(a => a.PrimaryCategoryId == categoryId.Value || a.SecondaryCategories.Any(sc => sc.CategoryId == categoryId.Value));
            }

            /* busco los artículos que tengan el parámetro como palabra clave */
            if (keyWordId.HasValue)
            {
                articles = articles.Include(a => a.KeyWords).Where(a => a.KeyWords.Any(kw => kw.KeyWordId == keyWordId.Value));
            }

            return articles;
        }

        private IQueryable<KeyWord> SearchKeyWords(string word)
        {
            if (!string.IsNullOrWhiteSpace(word))
            {
                return this._context.KeyWords.Where(kw => kw.Word.Contains(word));
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}