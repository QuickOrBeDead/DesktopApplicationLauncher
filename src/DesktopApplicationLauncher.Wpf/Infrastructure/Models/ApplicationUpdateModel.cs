﻿namespace DesktopApplicationLauncher.Wpf.Infrastructure.Models
{
    public sealed class ApplicationUpdateModel
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Path { get; set; }

        public string Arguments { get; set; }
    }
}