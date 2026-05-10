using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MemeCatalogDatabase
{
    public class Meme
    {
        public int MemeId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; } 

        public virtual int FileTypeId { get; set; }
        public virtual FileType FileType { get; set; }

        public virtual ICollection<Tag>
            Tags
        { get; private set; } =
            new ObservableCollection<Tag>();
    }
}
