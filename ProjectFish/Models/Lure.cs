using System;
using System.Collections.Generic;

namespace ProjectFish.Models
{
    public partial class Lure
    {
        public Lure()
        {
            Attraction = new HashSet<Attraction>();
            CompLure = new HashSet<CompLure>();
        }

        public int LureId { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }

        public virtual ICollection<Attraction> Attraction { get; set; }
        public virtual ICollection<CompLure> CompLure { get; set; }
    }
}
