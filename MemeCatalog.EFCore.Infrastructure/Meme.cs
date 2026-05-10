using System;
using System.Collections.Generic;
using System.Text;

namespace MemeCatalog.EFCore.Infrastructure
{
    public class Meme
    {
        public int MemeId { get; set; }
        public string Name { get; set; }
        public string MemeLocation { get; set; }
    }
}
