using System;

namespace Wiki.ViewModels
{
    public abstract class DeleteViewModelBase
    {
        public Guid Id { get; set; }

        public bool CanBeDeleted { get; set; }

        public string WarningMessage { get; set; }
    }
}
