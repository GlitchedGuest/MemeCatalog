using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MemeCatalogDatabase
{
    public class FileType
    {
        public int FileTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Meme>
            Memes
        { get; private set; } =
            new ObservableCollection<Meme>();
    }
}
