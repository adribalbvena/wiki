
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Wiki.ViewModels.Articles
{
    public class ArticleEntriesViewModel
    {
        public ArticleEntriesViewModel()
        {
            this.Entries = new List<ArticleEntryViewModel>();
            this.Entry = new ArticleEntryViewModel();
        }

        public Guid ArticleId { get; set; }

        [Display(Name = "Entradas")]
        public List<ArticleEntryViewModel> Entries { get; set; }

        public List<ArticleEntryViewModel> OrderedEntries => this.Entries.OrderBy(e => e.Order).ToList();

        public ArticleEntryViewModel Entry { get; set; }

        private int LastOrder => this.Entries.OrderByDescending(e => e.Order).FirstOrDefault()?.Order ?? -1;

        public void AddEntry()
        {
            this.AddEntry(this.Entry.Order, this.Entry.Title, this.Entry.SubTitle, this.Entry.Text);
            this.CleanEntry();
        }

        public void AddEntry(int order, string title, string subTitle, string text, Guid? id = null)
        {
            this.Entries.Add(new ArticleEntryViewModel { Id = id, Order = order, Title = title, SubTitle = subTitle, Text = text });
        }

        public void RemoveEntry(int order)
        {
            this.Entries.RemoveAll(e => e.Order == order);

            var i = 0;

            foreach (var entry in this.Entries.OrderBy(e => e.Order))
            {
                entry.Order = i;
                i++;
            }
        }

        public void EditEntry(int order)
        {
            this.Entry = this.Entries.FirstOrDefault(e => e.Order == order);
            if (this.Entry != null)
            {
                this.Entry.Editor = true;
            }
            else
            {
                this.CleanEntry();
            }
        }

        public void CancelEntry()
        {
            this.CleanEntry();
        }

        public void SaveEntry(int order)
        {
            var entry = this.Entries.FirstOrDefault(e => e.Order == order);

            if (entry != null && this.Entry != null)
            {
                entry.Title = this.Entry.Title;
                entry.SubTitle = this.Entry.SubTitle;
                entry.Text = this.Entry.Text;
            }

            this.CleanEntry();
        }

        private void CleanEntry()
        {
            this.Entry = new ArticleEntryViewModel { Order = this.LastOrder + 1 };

        }
    }
}
