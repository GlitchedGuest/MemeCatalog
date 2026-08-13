using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MemeCatalogDatabase
{
    [Index(nameof(name), IsUnique = true)]
    public class Tag
    {
        public int id { get; set; }

        
        [StringLength(100)]
        public string? name { get; set; }

        public virtual ICollection<Meme>
            Memes
        { get; private set; } =
            new ObservableCollection<Meme>();
        public override string ToString()
        {
            return name != null ? name : "";
        }
    }
}
