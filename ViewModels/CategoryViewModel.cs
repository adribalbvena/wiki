using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace Wiki.ViewModels
{
    public class CategoryViewModel
    {
        [Required]
        [Display(Name = "Nombre")]
        [Remote(controller: "Categories", action: "CategoryAvailable")]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Habilitada")]
        public bool Enabled { get; set; }
    }
}
