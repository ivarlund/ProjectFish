using System;
using System.Collections.Generic;

namespace ProjectFish.Models
{
    public partial class Reel
    {
        public Reel()
        {
            Composition = new HashSet<Composition>();
        }

        public int ReelId { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public string Line { get; set; }

        public virtual ICollection<Composition> Composition { get; set; }
    }
}
