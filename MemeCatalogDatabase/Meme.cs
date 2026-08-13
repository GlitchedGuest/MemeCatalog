using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MemeCatalogDatabase
{
    public class Meme
    {
        public int id { get; set; }

        [StringLength(200)]
        public string? name { get; set; }
        public string? path { get; set; } 

        public string? fileType
        {
            get => path?.Substring(path.LastIndexOf('.') + 1, path.Length - 1 - path.LastIndexOf('.'));
        }

        public int FileSize { get; set; }
        public DateTime? dateUploaded { get; set; }

        public virtual ICollection<Tag>
            Tags
        { get; private set; } =
            new ObservableCollection<Tag>();

        public override string ToString()
        {
            return name != null ? name : "";
        }
    }
}
