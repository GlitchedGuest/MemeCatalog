using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MemeCatalogDatabase
{
    public class Meme
    {
        public int MemeId { get; set; }
        public string? Name { get; set; }
        public string? Path { get; set; } 

        public string? FileType
        {
            get => Path?.Substring(Path.LastIndexOf('.') + 1, Path.Length - 1 - Path.LastIndexOf('.'));
            set => Path?.Substring(Path.LastIndexOf('.') + 1, Path.Length - 1 - Path.LastIndexOf('.'));
        }

        public virtual ICollection<Tag>
            Tags
        { get; private set; } =
            new ObservableCollection<Tag>();

        public override string ToString()
        {
            return Name;
        }
    }
}
