using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    public class UsersController : Controller
    {
        private readonly WikiContext _context;
        private readonly UserManager<User> _userManager;

        public UsersController(WikiContext context, UserManager<User> userManager)
        {

            _context = context;
            _userManager = userManager;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,FirstName,LastName,Phone")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Id = Guid.NewGuid();
                user.CreationDate = DateTime.Now;
                user.UserName = user.Email;
                var resultCreateUser = await _userManager.CreateAsync(user, Const.AdminPassword);

                if (resultCreateUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Const.AdminRoleName);
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in resultCreateUser.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Email,Password,CreationDate,FirstName,LastName,Phone")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Users/Delete/5
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

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private DeleteUserViewModel GetDeleteViewModel(Guid? id)
        {
            var viewModel = new DeleteUserViewModel
            {
                User = this.GetUserAsync(id).Result,
                CanBeDeleted = true
            };

            if (viewModel.User is Author)
            {
                if (this._context.Articles.Any(a => a.Author.Id == viewModel.User.Id))
                {
                    viewModel.CanBeDeleted = false;
                    viewModel.WarningMessage = string.Format(ErrorMessages.WarningMessageLabel, "El usuario es autor de algunos artículos, no puede ser eliminado");
                }
            }

            return viewModel;
        }

        private async Task<User> GetUserAsync(Guid? id)
        {
            if (id == null)
            {
                throw new ArgumentException();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                throw new ArgumentException();
            }

            return user;
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
