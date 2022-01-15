using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Wiki.Models;
using Wiki.Models.Constants;
using Wiki.Models.Constants.Initializer;

namespace Wiki.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly WikiContext _wikiContext;

        public DbInitializer(UserManager<User> usermanager, RoleManager<Role> rolemanager, WikiContext wikicontext)
        {
            this._userManager = usermanager;
            this._roleManager = rolemanager;
            this._wikiContext = wikicontext;
        }

        public void Seed()
        {
            this.CreateRoles();
            this.CreateUsers();
            this.CreateCategories();
            this.CreateKeyWords();
            this.CreateArticles();
        }

        private void CreateRoles()
        {
            foreach (var role in Const.Roles)
            {
                if (!(_roleManager.RoleExistsAsync(role).Result))
                {
                    var resu = _roleManager.CreateAsync(new Role() { Name = role }).Result;
                }
            }
        }

        private void CreateUsers()
        {
            foreach (var admin in InitializerData.Admins)
            {
                this.CreateAdmin(admin);
            }

            foreach (var author in InitializerData.Authors)
            {
                this.CreateAuthor(author);
            }
        }

        private void CreateAdmin(InitializerUser u)
        {
            var user = this._userManager.FindByEmailAsync(u.Email).Result;

            if (user == null)
            {
                user = new User
                {
                    CreationDate = DateTime.Now,
                    UserName = u.Email,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                };

                var userResult = this._userManager.CreateAsync(user, u.Password).Result;

                if (userResult.Succeeded)
                {
                    this._userManager.AddToRoleAsync(user, Const.AdminRoleName).Wait();
                }
            }
        }

        private void CreateAuthor(InitializerUser u)
        {
            var user = this._userManager.FindByEmailAsync(u.Email).Result;

            if (user == null)
            {
                user = new Author
                {
                    CreationDate = DateTime.Now,
                    UserName = u.Email,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                };

                var userResult = this._userManager.CreateAsync(user, u.Password).Result;

                if (userResult.Succeeded)
                {
                    this._userManager.AddToRoleAsync(user, Const.AuthorRoleName).Wait();
                }
            }
        }

        private void CreateCategories()
        {
            var categories = this._wikiContext.Categories;

            foreach (var category in InitializerData.Categories)
            {
                if (!categories.Any(c => c.Name == category))
                {
                    categories.Add(new Category { Id = Guid.NewGuid(), Name = category, Enabled = true });
                }
            }

            this._wikiContext.SaveChanges();
        }

        private void CreateKeyWords()
        {
            var keywords = this._wikiContext.KeyWords;

            foreach (var keyword in InitializerData.KeyWords)
            {
                if (!keywords.Any(kw => kw.Word == keyword))
                {
                    keywords.Add(new KeyWord { Id = Guid.NewGuid(), Word = keyword });
                }
            }

            this._wikiContext.SaveChanges();
        }

        private void CreateArticles()
        {
            foreach (var article in InitializerData.Articles)
            {
                this.CreateArticle(article);
            }

            this._wikiContext.SaveChanges();

            foreach (var article in InitializerData.Articles)
            {
                this.CreateReferences(article);
            }

            this._wikiContext.SaveChanges();
        }

        private void CreateArticle(InitializerArticle ia)
        {
            var articles = this._wikiContext.Articles.Include(a => a.Header);

            /* Si el artículo ya existe, no tengo que crearlo */
            if (articles.Any(a => a.Header.Title == ia.Title)) return;

            var author = this._wikiContext.Authors.FirstOrDefault(a => a.Email == ia.Author);

            /* Si no existe el autor, no hago nada */
            if (author == null) return;

            /* Obtengo la categoría primaria (la primera de la lista) */
            var category = this._wikiContext.Categories.FirstOrDefault(c => c.Name == ia.PrimaryCategory);

            /* Si no existe la categoría, no hago nada */
            if (category == null) return;

            /* Creo el artículo */
            var article = new Article
            {
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Author = author,
                Enabled = true,
                PrimaryCategory = category,
                SecondaryCategories = new List<ArticleCategory>(),
                KeyWords = new List<ArticleKeyWord>(),
                References = new List<Reference>()
            };

            /* Creo la cabecera */
            var header = new Header
            {
                Id = Guid.NewGuid(),
                Article = article,
                Title = ia.Title,
                Description = ia.Description
            };

            /* creo el cuerpo */
            var body = new Body
            {
                Id = Guid.NewGuid(),
                Entries = new List<Entry>(),
                Article = article
            };

            /* Agrego todo al contexto  */
            this._wikiContext.Articles.Add(article);
            this._wikiContext.Headers.Add(header);
            this._wikiContext.Bodies.Add(body);

            /* Agrego las categorías secundarias */
            foreach (var name in ia.SecondaryCategories)
            {
                var sc = this._wikiContext.Categories.FirstOrDefault(x => x.Name == name);

                if (sc == null) continue;

                this._wikiContext.ArticlesCategories.Add(new ArticleCategory { Category = sc, Article = article });
            }

            /* Agrego las palabras clave */
            foreach (var word in ia.KeyWords)
            {
                var kw = this._wikiContext.KeyWords.FirstOrDefault(x => x.Word == word);

                if (kw == null) continue;

                this._wikiContext.ArticlesKeyWords.Add(new ArticleKeyWord { KeyWord = kw, Article = article });
            }

            /* Creo las entradas */
            foreach (var entry in ia.Entries)
            {
                this._wikiContext.Entries.Add(new Entry
                {
                    Id = Guid.NewGuid(),
                    Body = body,
                    Order = ia.Entries.IndexOf(entry),
                    Title = entry.Title,
                    Subtitle = entry.SubTitle,
                    Text = entry.Text
                });
            }

            /* Creo los mensajes */
            foreach (var message in ia.Messages)
            {
                var user = this._wikiContext.Users.FirstOrDefault(u => u.Email == message.User);

                if (user == null) continue;

                this._wikiContext.Messages.Add(new Message
                {
                    Id = Guid.NewGuid(),
                    Article = article,
                    User = user,
                    Date = DateTime.Now,
                    Title = message.Title,
                    Text = message.Text
                });
            }
        }

        private void CreateReferences(InitializerArticle a)
        {
            var articles = this._wikiContext.Articles.Include(a => a.Header);

            /* Creo las referencias */
            foreach (var title in a.References)
            {
                var article = articles.FirstOrDefault(x => x.Header.Title == a.Title);

                if (article == null) continue;

                var reference = articles.FirstOrDefault(x => x.Header.Title == title);

                if (reference == null) continue;

                if (this._wikiContext.References.Any(r => r.MainArticleId == article.Id && r.ReferenceArticleId == reference.Id)) continue;

                this._wikiContext.References.Add(new Reference { ReferenceArticle = reference, MainArticle = article });
            }
        }
    }
}
