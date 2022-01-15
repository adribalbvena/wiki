using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Wiki.Data;
using Wiki.Models;
using Wiki.Models.Constants;
using Wiki.ViewModels;

namespace Wiki.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly WikiContext _context;

        public HomeController(SignInManager<User> signInManager, WikiContext context)
        {
            this._signInManager = signInManager;
            this._context = context;
        }

        public IActionResult Index()
        {
            var viewModel = new HomeViewModel();

            this.LoadFeaturedForHome(viewModel);

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> SearchArticle(string keyWord)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var viewModel = new HomeViewModel();

                viewModel.KeyWords = await this._context.KeyWords.Include(a => a.Articles).Where(a => a.Word.Contains(keyWord)).Take(Const.SearchResults).ToListAsync();

                this.LoadFeaturedForHome(viewModel);

                return this.View("Index", viewModel);
            }

            return RedirectToAction("Login", "Accounts");
        }

        [HttpPost]
        public async Task<IActionResult> SearchCategory(string category)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var viewModel = new HomeViewModel();

                viewModel.Categories = await this._context.Categories.Include(a => a.PrimaryArticles).Where(a => a.Enabled && a.Name.Contains(category)).Take(Const.SearchResults).ToListAsync();
                this.LoadFeaturedForHome(viewModel);

                return this.View("Index", viewModel);
            }

            return RedirectToAction("Login", "Accounts");
        }

        private void LoadFeaturedForHome(HomeViewModel viewModel)
        {
            viewModel.Articles = this._context.Articles.Include(a => a.Header).Include(a => a.Author).Include(a => a.PrimaryCategory).Where(a => a.Enabled && a.PrimaryCategory.Enabled).OrderByDescending(a => a.Date).Take(Const.TopArticles).ToList();
            viewModel.Authors = this._context.Authors.Include(a => a.Articles).OrderByDescending(a => a.Articles.Count).Take(Const.TopAuthors).ToList();
        }
    }
}